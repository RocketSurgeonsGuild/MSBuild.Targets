using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Rocket.Surgery.MyAssembly;

[Generator(LanguageNames.CSharp)]
public class ResourcesGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var files = context
                   .AdditionalTextsProvider
                   .Combine(context.AnalyzerConfigOptionsProvider)
                   .Select((tuple, _) => ( file: tuple.Left, path: tuple.Left.Path, options: tuple.Right.GetOptions(tuple.Left) ))
                   .Where(x => x.options.TryGetValue("build_metadata.EmbeddedResource.Value", out _))
                   .Select((x, ct) =>
                           {
                               x.options.TryGetValue("build_metadata.EmbeddedResource.Value", out var resourceName);
                               x.options.TryGetValue("build_metadata.EmbeddedResource.Area", out var area);
                               x.options.TryGetValue("build_metadata.EmbeddedResource.Comment", out var comment);
                               return ( x.path, resourceName: resourceName!, area: string.IsNullOrWhiteSpace(area) ? null : area, comment: string.IsNullOrWhiteSpace(comment) ? null : comment );
                           }
                    );

        // Read the MyAssemblyNamespace property or default to null
        var right = context.AnalyzerConfigOptionsProvider
                           .Select((c, t) => (
                                       c.GlobalOptions.TryGetValue("build_property.MyAssemblyNamespace", out var ns) && !string.IsNullOrEmpty(ns) ? ns : null,
                                       c.GlobalOptions.TryGetValue("build_property.MyAssemblyVisibility", out var visibility) && !string.IsNullOrEmpty(visibility) ? visibility : null
                                   )
                            );

        context.RegisterSourceOutput(
            files
               .Combine(right),
            GenerateSource
        );
    }

    static void GenerateSource(
        SourceProductionContext spc,
        ((string path, string resourceName, string area, string comment) resource, (string, string) Right) valueTuple
    )
    {
        var ((path, resourceName, area, comment), (ns, visibility)) = valueTuple;

        var rest = area.Split('.', StringSplitOptions.RemoveEmptyEntries);

        var property = MethodDeclaration(ParseTypeName("System.IO.Stream"), Identifier(resourceName.Replace(".", "").Replace("-", "_")))
                      .WithModifiers(TokenList(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword)))
                      .WithExpressionBody(
                           ArrowExpressionClause(
                               InvocationExpression(
                                       MemberAccessExpression(
                                           SyntaxKind.SimpleMemberAccessExpression,
                                           MemberAccessExpression(
                                               SyntaxKind.SimpleMemberAccessExpression,
                                               TypeOfExpression(IdentifierName("MyAssembly")),
                                               IdentifierName("Assembly")
                                           ),
                                           IdentifierName("GetManifestResourceStream")
                                       )
                                   )
                                  .WithArgumentList(
                                       ArgumentList(
                                           SingletonSeparatedList(
                                               Argument(
                                                   LiteralExpression(
                                                       SyntaxKind.StringLiteralExpression,
                                                       Literal(path)
                                                   )
                                               )
                                           )
                                       )
                                   )
                           )
                       )
                      .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                      .AddSummary(comment);

        var classDefinition = rest
                             .AsEnumerable()
                             .Reverse()
                             .Aggregate<string, MemberDeclarationSyntax>(
                                  property,
                                  (definition, parent) => ClassDeclaration(parent)
                                                         .WithModifiers(TokenList(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword)))
                                                         .AddMembers(definition)
                                                         .AddSummary($"Provides access to embedded resources under {parent}")
                              );
        classDefinition = ClassDeclaration("Resources")
                         .WithModifiers(TokenList(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword)))
                         .AddMembers(classDefinition)
                         .AddSummary("Provides access to embedded resources");

        classDefinition = ClassDeclaration("MyAssembly")
                         .WithModifiers(TokenList(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword)))
                         .AddMembers(classDefinition);

        var cu = CompilationUnit();
        cu = ns is { Length: > 0 } ? cu.AddMembers(NamespaceDeclaration(ParseName(ns)).AddMembers(classDefinition)) : cu.AddMembers(classDefinition);

        spc.AddSource($"{area}.{resourceName}.g.cs", SourceText.From(cu.NormalizeWhitespace().GetText().ToString(), Encoding.UTF8));
    }
}
