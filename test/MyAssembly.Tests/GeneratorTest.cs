using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Testing;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Rocket.Surgery.MyAssembly.Tests;

public abstract class GeneratorTest(ITestContextAccessor testContextAccessor) : LoggerTest<XUnitTestContext>(new (testContextAccessor)), IAsyncLifetime
{
    public GeneratorTestContextBuilder Builder { get; private set; }

    public ValueTask InitializeAsync()
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
        return ValueTask.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}
