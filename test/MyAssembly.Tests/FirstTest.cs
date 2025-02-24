using System.Runtime.CompilerServices;
using System.Text;
using DiffEngine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Testing;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;
using Xunit.Abstractions;

namespace Rocket.Surgery.MyAssembly.Tests;

public class ConstantsGeneratorTests(ITestOutputHelper testOutputHelper) : GeneratorTest(testOutputHelper)
{
    [Fact]
    public async Task Should_Add_A_Constant()
    {
        var result = await Builder
                          .AddConstant(
                               "Test",
                               "Value",
                               "",
                               "comment",
                               "Constant",
                               "rootComment"
                           )
                          .Build()
                          .GenerateAsync();
        await Verify(result);
    }

    [Fact]
    public async Task Should_Add_A_Nested_Constant()
    {
        var result = await Builder
                          .AddConstant(
                               "Test",
                               "Value",
                               "",
                               "comment",
                               "Some.Nested.Root",
                               "should be some"
                           )
                          .Build()
                          .GenerateAsync();
        await Verify(result);
    }

    [Fact]
    public async Task Should_Add_A_Embedded_Resource()
    {
        var result = await Builder
                          .AddEmbeddedResource(
                               "somefile.cs",
                               "somefile.cs",
                               "kind",
                               "comment"
                           )
                          .Build()
                          .GenerateAsync();
        await Verify(result);
    }

    [Fact]
    public async Task Should_Add_A_Nested_Embedded_Resource()
    {
        var result = await Builder
                          .AddEmbeddedResource(
                               "something/somefile.cs",
                               "something.somefile.cs",
                               "kind",
                               "comment"
                           )
                          .Build()
                          .GenerateAsync();
        await Verify(result);

    }
}

public static class GeneratorExtensions
{
    public static GeneratorTestContextBuilder AddConstant(this GeneratorTestContextBuilder builder, string path, string value, string type, string comment, string root, string rootComment)
    {
        var text = new MyText(path);
        return builder
              .AddAdditionalTexts(text)
              .AddOption(text, "build_metadata.Constant.ItemType", "Constant")
              .AddOption(text, "build_metadata.Constant.Value", value)
              .AddOption(text, "build_metadata.Constant.Type", type)
              .AddOption(text, "build_metadata.Constant.Comment", comment)
              .AddOption(text, "build_metadata.Constant.Root", root)
              .AddOption(text, "build_metadata.Constant.RootComment", rootComment);
    }
    public static GeneratorTestContextBuilder AddEmbeddedResource(this GeneratorTestContextBuilder builder, string path, string resourceName, string area, string comment)
    {
        var text = new MyText(path);
        return builder
              .AddAdditionalTexts(text)
              .AddOption(text, "build_metadata.EmbeddedResource.Value", resourceName)
              .AddOption(text, "build_metadata.EmbeddedResource.Area", area)
              .AddOption(text, "build_metadata.EmbeddedResource.Comment", comment);
    }

    class MyText(string name) : AdditionalText
    {
        public override SourceText? GetText(CancellationToken cancellationToken = new CancellationToken()) => SourceText.From("", Encoding.UTF8);

        public override string Path { get; } = name;
    }
}

public abstract class GeneratorTest(ITestOutputHelper testOutputHelper) : LoggerTest<XUnitTestContext>(XUnitTestContext.Create(testOutputHelper)), IAsyncLifetime
{
    public GeneratorTestContextBuilder Builder { get; private set; }

    public Task InitializeAsync()
    {
        Builder = GeneratorTestContextBuilder
                 .Create()
                 .WithGenerator<ConstantsGenerator>()
                 .WithGenerator<ResourcesGenerator>()
//                 .WithGenerator<StringsGenerator>()
                 .AddReferences(
                      typeof(ActivatorUtilities).Assembly,
                      typeof(IServiceProvider).Assembly
                  );
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifyGeneratorTextContext.Initialize(DiagnosticSeverity.Error, Customizers.Default, Customizers.ExcludeParseOptions);

        DiffRunner.Disabled = true;
        DerivePathInfo((sourceFile, _, type, method) =>
                       {
                           static string GetTypeName(Type type)
                           {
                               // ReSharper disable once NullableWarningSuppressionIsUsed
                               return type.IsNested ? $"{type.ReflectedType!.Name}.{type.Name}" : type.Name;
                           }

                           var typeName = GetTypeName(type);

                           // ReSharper disable once NullableWarningSuppressionIsUsed
                           return new(Path.Combine(Path.GetDirectoryName(sourceFile)!, "snapshots"), typeName, method.Name);
                       }
        );
    }
}
