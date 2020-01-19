# Rocket Surgeons Build Metadata

Every good Rocket Surgeon needs a way to know where there code came from.  This package embededs metadata into your assemblies for a few purposes:


# What does it do?
1) Build Validation
  * Know where your assembly came from.  Was it built on appveyor, gitlab, or azure pipelines
    * Current supports:
    * AppVeyor
    * GitLab
    * Azure Pipelines
2) Storing `GitVersion` information, useful for validating versions of assemblies in your application.
3) Build Source Linking
  * Enables some sane defaults for SourceLink packages
4) JetBrains.Annotations
  * Brings in `JetBrains.Annotations` automagically as a source file.
5) Adds support for a new `ItemGroup` Item `<InternalsVisibleTo Include="MyAssembly" />`
5) Adds support for a new `ItemGroup` Item `<AssemblyMetadata Include="Key" Value="Value" />`
6) The information package allows for exatracting

# Status
<!-- badges -->
[![github-release-badge]][github-release]
[![github-license-badge]][github-license]
[![codecov-badge]][codecov]
<!-- badges -->

<!-- history badges -->
| Azure Pipelines | AppVeyor |
| --------------- | -------- |
| [![azurepipelines-badge]][azurepipelines] | [![appveyor-badge]][appveyor] |
| [![azurepipelines-history-badge]][azurepipelines-history] | [![appveyor-history-badge]][appveyor-history] |
<!-- history badges -->

<!-- nuget packages -->
| Package | NuGet |
| ------- | ----- |
| Rocket.Surgery.MSBuild.CI | [![nuget-version-crojfy8iotja-badge]![nuget-downloads-crojfy8iotja-badge]][nuget-crojfy8iotja] |
| Rocket.Surgery.MSBuild.GitVersion | [![nuget-version-mqvqrnlrlsyw-badge]![nuget-downloads-mqvqrnlrlsyw-badge]][nuget-mqvqrnlrlsyw] |
| Rocket.Surgery.MSBuild.JetBrains.Annotations | [![nuget-version-58lqc0lqtjla-badge]![nuget-downloads-58lqc0lqtjla-badge]][nuget-58lqc0lqtjla] |
| Rocket.Surgery.MSBuild.Metadata | [![nuget-version-q/inj3fd2l6a-badge]![nuget-downloads-q/inj3fd2l6a-badge]][nuget-q/inj3fd2l6a] |
| Rocket.Surgery.MSBuild.SourceLink | [![nuget-version-su+gulvppohw-badge]![nuget-downloads-su+gulvppohw-badge]][nuget-su+gulvppohw] |
<!-- nuget packages -->

# Whats next?
TBD

<!-- generated references -->
[github-release]: https://github.com/RocketSurgeonsGuild/MSBuild.Targets/releases/latest
[github-release-badge]: https://img.shields.io/github/release/RocketSurgeonsGuild/MSBuild.Targets.svg?logo=github&style=flat "Latest Release"
[github-license]: https://github.com/RocketSurgeonsGuild/MSBuild.Targets/blob/master/LICENSE
[github-license-badge]: https://img.shields.io/github/license/RocketSurgeonsGuild/MSBuild.Targets.svg?style=flat "License"
[codecov]: https://codecov.io/gh/RocketSurgeonsGuild/MSBuild.Targets
[codecov-badge]: https://img.shields.io/codecov/c/github/RocketSurgeonsGuild/MSBuild.Targets.svg?color=E03997&label=codecov&logo=codecov&logoColor=E03997&style=flat "Code Coverage"
[azurepipelines]: https://rocketsurgeonsguild.visualstudio.com/Libraries/_build/latest?definitionId=5&branchName=master
[azurepipelines-badge]: https://img.shields.io/azure-devops/build/rocketsurgeonsguild/Libraries/5.svg?color=98C6FF&label=azure%20pipelines&logo=azuredevops&logoColor=98C6FF&style=flat "Azure Pipelines Status"
[azurepipelines-history]: https://rocketsurgeonsguild.visualstudio.com/Libraries/_build?definitionId=5&branchName=master
[azurepipelines-history-badge]: https://buildstats.info/azurepipelines/chart/rocketsurgeonsguild/Libraries/5?includeBuildsFromPullRequest=false "Azure Pipelines History"
[appveyor]: https://ci.appveyor.com/project/RocketSurgeonsGuild/MSBuild.Targets
[appveyor-badge]: https://img.shields.io/appveyor/ci/RocketSurgeonsGuild/MSBuild.Targets.svg?color=00b3e0&label=appveyor&logo=appveyor&logoColor=00b3e0&style=flat "AppVeyor Status"
[appveyor-history]: https://ci.appveyor.com/project/RocketSurgeonsGuild/MSBuild.Targets/history
[appveyor-history-badge]: https://buildstats.info/appveyor/chart/RocketSurgeonsGuild/MSBuild.Targets?includeBuildsFromPullRequest=false "AppVeyor History"
[nuget-crojfy8iotja]: https://www.nuget.org/packages/Rocket.Surgery.MSBuild.CI/
[nuget-version-crojfy8iotja-badge]: https://img.shields.io/nuget/v/Rocket.Surgery.MSBuild.CI.svg?color=004880&logo=nuget&style=flat-square "NuGet Version"
[nuget-downloads-crojfy8iotja-badge]: https://img.shields.io/nuget/dt/Rocket.Surgery.MSBuild.CI.svg?color=004880&logo=nuget&style=flat-square "NuGet Downloads"
[nuget-mqvqrnlrlsyw]: https://www.nuget.org/packages/Rocket.Surgery.MSBuild.GitVersion/
[nuget-version-mqvqrnlrlsyw-badge]: https://img.shields.io/nuget/v/Rocket.Surgery.MSBuild.GitVersion.svg?color=004880&logo=nuget&style=flat-square "NuGet Version"
[nuget-downloads-mqvqrnlrlsyw-badge]: https://img.shields.io/nuget/dt/Rocket.Surgery.MSBuild.GitVersion.svg?color=004880&logo=nuget&style=flat-square "NuGet Downloads"
[nuget-58lqc0lqtjla]: https://www.nuget.org/packages/Rocket.Surgery.MSBuild.JetBrains.Annotations/
[nuget-version-58lqc0lqtjla-badge]: https://img.shields.io/nuget/v/Rocket.Surgery.MSBuild.JetBrains.Annotations.svg?color=004880&logo=nuget&style=flat-square "NuGet Version"
[nuget-downloads-58lqc0lqtjla-badge]: https://img.shields.io/nuget/dt/Rocket.Surgery.MSBuild.JetBrains.Annotations.svg?color=004880&logo=nuget&style=flat-square "NuGet Downloads"
[nuget-q/inj3fd2l6a]: https://www.nuget.org/packages/Rocket.Surgery.MSBuild.Metadata/
[nuget-version-q/inj3fd2l6a-badge]: https://img.shields.io/nuget/v/Rocket.Surgery.MSBuild.Metadata.svg?color=004880&logo=nuget&style=flat-square "NuGet Version"
[nuget-downloads-q/inj3fd2l6a-badge]: https://img.shields.io/nuget/dt/Rocket.Surgery.MSBuild.Metadata.svg?color=004880&logo=nuget&style=flat-square "NuGet Downloads"
[nuget-su+gulvppohw]: https://www.nuget.org/packages/Rocket.Surgery.MSBuild.SourceLink/
[nuget-version-su+gulvppohw-badge]: https://img.shields.io/nuget/v/Rocket.Surgery.MSBuild.SourceLink.svg?color=004880&logo=nuget&style=flat-square "NuGet Version"
[nuget-downloads-su+gulvppohw-badge]: https://img.shields.io/nuget/dt/Rocket.Surgery.MSBuild.SourceLink.svg?color=004880&logo=nuget&style=flat-square "NuGet Downloads"
<!-- generated references -->

<!-- nuke-data
github:
  owner: RocketSurgeonsGuild
  repository: MSBuild.Targets
azurepipelines:
  account: rocketsurgeonsguild
  teamproject: Libraries
  builddefinition: 5
appveyor:
  account: RocketSurgeonsGuild
  build: MSBuild.Targets
-->