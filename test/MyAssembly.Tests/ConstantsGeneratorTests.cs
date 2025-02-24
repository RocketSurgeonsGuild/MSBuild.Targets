namespace Rocket.Surgery.MyAssembly.Tests;

public class ConstantsGeneratorTests(ITestContextAccessor testOutputHelper) : GeneratorTest(testOutputHelper)
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
                               "something/else/somefile.cs",
                               "something.somefile.cs",
                               "something/else",
                               "comment"
                           )
                          .Build()
                          .GenerateAsync();
        await Verify(result);
    }
}
