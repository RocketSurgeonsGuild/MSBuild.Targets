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
                   .Combine(context.CompilationProvider)
                   .Select((a, ct) =>
                           {
                               var x = a.Left;
                               var c = a.Right;
                               x.options.TryGetValue("build_metadata.EmbeddedResource.Value", out var resourceName);
                               x.options.TryGetValue("build_metadata.EmbeddedResource.Area", out var area);
                               x.options.TryGetValue("build_metadata.EmbeddedResource.Comment", out var comment);
                               x.options.TryGetValue("build_metadata.EmbeddedResource.LogicalName", out var logicalName);
                               resourceName = string.Join(".", ( resourceName ?? "" ).Split(['/', '\\', '.', '-'], StringSplitOptions.RemoveEmptyEntries));
                               area = string.IsNullOrWhiteSpace(area) ? null : string.Join("/", area!.Split(['/', '\\'], StringSplitOptions.RemoveEmptyEntries));

                               return (
                                   logicalName ?? $"{c.AssemblyName}.{resourceName}",
                                   resourceName,
                                   area,
                                   comment: string.IsNullOrWhiteSpace(comment) ? null : comment
                               );
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
        ((string logicalName, string resourceName, string? area, string? comment) resource, (string?, string?) Right) valueTuple
    )
    {
        var ((logicalName, resourceName, area, comment), (ns, visibility)) = valueTuple;

        var property = MethodDeclaration(ParseTypeName("System.IO.Stream"), Identifier($"{area}/{resourceName}".Replace("/", "_").Replace(".", "_")))
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
                                                       Literal(logicalName)
                                                   )
                                               )
                                           )
                                       )
                                   )
                           )
                       )
                      .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
        if (comment is { Length: > 0 }) property = property.AddSummary(comment);

        var classDefinition = ClassDeclaration("Resources")
                         .WithModifiers(TokenList(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword)))
                         .AddMembers(property)
                         .AddSummary("Provides access to embedded resources");

        classDefinition = ClassDeclaration("MyAssembly")
                         .WithModifiers(TokenList(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword)))
                         .AddMembers(classDefinition);

        var cu = CompilationUnit();
        cu = ns is { Length: > 0 } ? cu.AddMembers(NamespaceDeclaration(ParseName(ns)).AddMembers(classDefinition)) : cu.AddMembers(classDefinition);

        spc.AddSource($"{resourceName}.g.cs", SourceText.From(cu.NormalizeWhitespace().GetText().ToString(), Encoding.UTF8));
    }
}
