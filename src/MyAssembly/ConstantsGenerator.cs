using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.Text;

namespace Rocket.Surgery.MyAssembly;

[Generator]
public class ConstantsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var files = context
                   .AdditionalTextsProvider
                   .Combine(context.AnalyzerConfigOptionsProvider)
                   .Select((tuple, _) => ( file: tuple.Left, path: tuple.Left.Path, options: tuple.Right.GetOptions(tuple.Left) ))
                   .Where(x => x.options.TryGetValue("build_metadata.Constant.ItemType", out var itemType) && itemType is "Constant")
                   .Select((x, _) =>
                           {
                               x.options.TryGetValue("build_metadata.Constant.Value", out var value);
                               x.options.TryGetValue("build_metadata.Constant.Type", out var type);
                               x.options.TryGetValue("build_metadata.Constant.Comment", out var comment);
                               x.options.TryGetValue("build_metadata.Constant.Root", out var root);
                               x.options.TryGetValue("build_metadata.Constant.RootComment", out var rootComment);

                               if (string.IsNullOrEmpty(rootComment))
                                   rootComment = "Provides access project-defined constants.";

                               // Revert auto-escaping due to https://github.com/dotnet/roslyn/issues/51692
                               if (value != null && value.StartsWith("|") && value.EndsWith("|"))
                                   value = value[1..^1].Replace('|', ';');

                               var name = Path.GetFileName(x.path);
                               if (string.IsNullOrEmpty(root))
                               {
                                   root = "Constants";
                               }
                               else if (root == ".")
                               {
                                   var parts = name.Split(['.'], StringSplitOptions.RemoveEmptyEntries);
                                   root = string.Join(".", parts.Take(parts.Length - 1));
                                   name = parts.Last();
                               }

                               return ( name,
                                        value: value ?? "",
                                        type: string.IsNullOrWhiteSpace(type) ? null : type,
                                        comment: string.IsNullOrWhiteSpace(comment) ? null : comment, root!,
                                        rootComment!
                                   );
                           }
                    );

        // Read the MyAssemblyNamespace property or default to null
        var right = context
                   .AnalyzerConfigOptionsProvider
                   .Select((c, _) => (
                               c.GlobalOptions.TryGetValue("build_property.MyAssemblyNamespace", out var ns) && !string.IsNullOrEmpty(ns) ? ns : null,
                               c.GlobalOptions.TryGetValue("build_property.MyAssemblyVisibility", out var visibility) && !string.IsNullOrEmpty(visibility) ? visibility : null
                           )
                    );

        var inputs = files.Combine(right);
        context.RegisterSourceOutput(inputs, GenerateConstant);
    }

    void GenerateConstant(
        SourceProductionContext spc,
        ((string name, string value, string? type, string? comment, string root, string rootComment), (string? ns, string? visibility)) args
    )
    {
        var ((name, value, type, comment, root, rootComment), (ns, visibility)) = args;
        comment ??= value;

        var parts = root.Split(['.'], StringSplitOptions.RemoveEmptyEntries);
        var final = parts.Last();

        var property = PropertyDeclaration(PredefinedType(Token(SyntaxKind.StringKeyword)), Identifier(name.Replace("-", "_")))
                      .WithModifiers(TokenList(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword)))
                      .WithExpressionBody(ArrowExpressionClause(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value))))
                      .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                      .AddSummary(comment);

        var classDefinition = parts
                             .SkipLast(1)
                             .Reverse()
                             .Aggregate(
                                  ClassDeclaration(final)
                                     .WithModifiers(TokenList(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword)))
                                     .AddMembers(property),
                                  (definition, parent) => ClassDeclaration(parent)
                                                         .WithModifiers(TokenList(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword)))
                                                         .AddMembers(definition)
                              )
                             .AddSummary(rootComment);

        classDefinition = ClassDeclaration("MyAssembly")
                         .WithModifiers(TokenList(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword)))
                         .AddMembers(classDefinition);

        var cu = CompilationUnit();
        cu = ns is { Length: > 0 } ? cu.AddMembers(NamespaceDeclaration(ParseName(ns)).AddMembers(classDefinition)) : cu.AddMembers(classDefinition);

        spc.AddSource(
            $"{root}.{name}.g.cs",
            SourceText.From(
                cu
                   .NormalizeWhitespace()
                   .GetText()
                   .ToString(),
                Encoding.UTF8
            )
        );
    }
}

internal static class Extensions
{
    private static TMember AddSimple<TMember>(this TMember member, XmlElementSyntax xmlElement) where TMember : MemberDeclarationSyntax
    {
        return member.WithLeadingTrivia(
            TriviaList(
                Trivia(
                    DocumentationComment(
                        xmlElement,
                        XmlText()
                           .WithTextTokens(
                                TokenList(
                                    Token(
                                        TriviaList(),
                                        SyntaxKind.XmlTextLiteralNewLineToken,
                                        "\n",
                                        "\n",
                                        TriviaList()
                                    )
                                )
                            )
                    )
                )
            )
        );
    }

    public static PropertyDeclarationSyntax AddValue(this PropertyDeclarationSyntax property, string value)
    {
        return property.AddSimple(
            XmlValueElement(
                XmlText(value)
            )
        );
    }

    public static TMember AddSummary<TMember>(this TMember member, string value) where TMember : MemberDeclarationSyntax
    {
        return member.AddSimple(
            XmlSummaryElement(
                XmlText(value)
            )
        );
    }
}
