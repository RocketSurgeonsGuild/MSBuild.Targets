using System.Collections.Immutable;
using System.Globalization;
using System.Reflection;
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
        var extensions = context
                        .AnalyzerConfigOptionsProvider
                        .SelectMany((p, _) =>
                                    {
                                        if (!p.GlobalOptions.TryGetValue("build_property.EmbeddedResourceStringExtensions", out var extensions) || extensions == null)
                                            return [];

                                        return extensions.Split('|');
                                    }
                         )
                        .WithComparer(StringComparer.OrdinalIgnoreCase)
                        .Collect();
        var files = context
                   .AdditionalTextsProvider
                   .Combine(context.AnalyzerConfigOptionsProvider)
                   .Select((tuple, _) => ( file: tuple.Left, path: tuple.Left.Path, options: tuple.Right.GetOptions(tuple.Left) ))
                   .Where(x =>
                              x.options.TryGetValue("build_metadata.EmbeddedResource.MyAssemblyResource", out var assemblyResource)
                           && bool.TryParse(assemblyResource, out var isAssemblyResource)
                           && isAssemblyResource
                    )
                   .Where(x => x.options.TryGetValue("build_metadata.EmbeddedResource.Value", out var value) && value != null)
                   .Select((x, ct) =>
                           {
                               x.options.TryGetValue("build_metadata.EmbeddedResource.Value", out var resourceName);
                               x.options.TryGetValue("build_metadata.EmbeddedResource.Kind", out var kind);
                               x.options.TryGetValue("build_metadata.EmbeddedResource.Comment", out var comment);
                               return ( x.path, resourceName: resourceName!, kind, comment: string.IsNullOrWhiteSpace(comment) ? null : comment );
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
               .Combine(extensions)
               .Combine(right)
               .Combine(context.ParseOptionsProvider),
            GenerateSource
        );
    }

    static void GenerateSource(
        SourceProductionContext spc,
        ((((string path, string resourceName, string kind, string comment) resource, ImmutableArray<string> extensions) Left, (string, string) Right) Left, ParseOptions Right) valueTuple
    )
    {
        var (((resource, extensions), (ns, visibility)), parse) = valueTuple;

        var final = Path.GetFileName(resource.path);
        var rest = (Path.GetDirectoryName(resource.path)??"").Split(['/', '\\'], StringSplitOptions.RemoveEmptyEntries);

        var property = MethodDeclaration(ParseTypeName("System.IO.Stream"), Identifier(final.Replace(".", "").Replace("-", "_")))
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
                                                       Literal(resource.resourceName)
                                                   )
                                               )
                                           )
                                       )
                                   )
                           )
                       )
                      .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                      .AddSummary(resource.comment);

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

        var cu = CompilationUnit()
                .AddMembers(classDefinition)
                .NormalizeWhitespace();

        spc.AddSource($"{final}.{resource.resourceName}.g.cs", SourceText.From(cu.GetText().ToString(), Encoding.UTF8));
    }
}
