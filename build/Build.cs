#:package Microsoft.VisualStudio.SolutionPersistence
#:package Sourcy.Git
#:package Sourcy.DotNet
#:package Rocket.Surgery.ModularPipelines.Extensions
#:property ImportConventions=true
#:property JsonSerializerIsReflectionEnabledByDefault=true

using Build;
using ModularPipelines;
using ModularPipelines.Plugins;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.ModularPipelines.Extensions;

var pipelineBuilder = Pipeline.CreateBuilder(args);
PluginRegistry.Register(new ConventionsPlugin(ConventionContextBuilder.Create(Imports.Instance)
.AddIfMissing("BuildScriptsRoot", Path.Join(Sourcy.Git.RootDirectory.FullName, "build"))));
await pipelineBuilder.Build().RunAsync();
