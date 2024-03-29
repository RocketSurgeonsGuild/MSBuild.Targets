# Rocket Surgeons Build Metadata

Every good Rocket Surgeon needs a way to know where there code came from. This package embededs metadata into your assemblies for a few purposes:

# What does it do?

1. Build Validation

-   Know where your assembly came from. Was it built on appveyor, gitlab, or azure pipelines
    -   Current supports:
    -   AppVeyor
    -   GitLab
    -   Azure Pipelines

2. Storing `GitVersion` information, useful for validating versions of assemblies in your application.
3. Build Source Linking

-   Enables some sane defaults for SourceLink packages

4. JetBrains.Annotations

-   Brings in `JetBrains.Annotations` automagically as a source file.

5. Adds support for a new `ItemGroup` Item `<InternalsVisibleTo Include="MyAssembly" />`
6. Adds support for a new `ItemGroup` Item `<AssemblyMetadata Include="Key" Value="Value" />`
7. The information package allows for exatracting

# Status

<!-- badges -->
[![github-release-badge]][github-release]
[![github-license-badge]][github-license]
[![codecov-badge]][codecov]
<!-- badges -->

<!-- history badges -->
| GitHub Actions |
| -------------- |
| [![github-badge]][github] |
| [![github-history-badge]][github] |
<!-- history badges -->

<!-- nuget packages -->
| Package | NuGet |
| ------- | ----- |
| Rocket.Surgery.MSBuild.CI | [![nuget-version-crojfy8iotja-badge]![nuget-downloads-crojfy8iotja-badge]][nuget-crojfy8iotja] |
| Rocket.Surgery.MSBuild.GitVersion | [![nuget-version-mqvqrnlrlsyw-badge]![nuget-downloads-mqvqrnlrlsyw-badge]][nuget-mqvqrnlrlsyw] |
| Rocket.Surgery.MSBuild.GlobalAnalyzerConfig | [![nuget-version-u3i45q7/mnfg-badge]![nuget-downloads-u3i45q7/mnfg-badge]][nuget-u3i45q7/mnfg] |
| Rocket.Surgery.MSBuild.JetBrains.Annotations | [![nuget-version-58lqc0lqtjla-badge]![nuget-downloads-58lqc0lqtjla-badge]][nuget-58lqc0lqtjla] |
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
[github]: https://github.com/RocketSurgeonsGuild/MSBuild.Targets/actions?query=workflow%3Aci
[github-badge]: https://img.shields.io/github/workflow/status/RocketSurgeonsGuild/MSBuild.Targets/ci.svg?label=github&logo=github&color=b845fc&logoColor=b845fc&style=flat "GitHub Actions Status"
[github-history-badge]: https://buildstats.info/github/chart/RocketSurgeonsGuild/MSBuild.Targets?includeBuildsFromPullRequest=false "GitHub Actions History"
[nuget-crojfy8iotja]: https://www.nuget.org/packages/Rocket.Surgery.MSBuild.CI/
[nuget-version-crojfy8iotja-badge]: https://img.shields.io/nuget/v/Rocket.Surgery.MSBuild.CI.svg?color=004880&logo=nuget&style=flat-square "NuGet Version"
[nuget-downloads-crojfy8iotja-badge]: https://img.shields.io/nuget/dt/Rocket.Surgery.MSBuild.CI.svg?color=004880&logo=nuget&style=flat-square "NuGet Downloads"
[nuget-mqvqrnlrlsyw]: https://www.nuget.org/packages/Rocket.Surgery.MSBuild.GitVersion/
[nuget-version-mqvqrnlrlsyw-badge]: https://img.shields.io/nuget/v/Rocket.Surgery.MSBuild.GitVersion.svg?color=004880&logo=nuget&style=flat-square "NuGet Version"
[nuget-downloads-mqvqrnlrlsyw-badge]: https://img.shields.io/nuget/dt/Rocket.Surgery.MSBuild.GitVersion.svg?color=004880&logo=nuget&style=flat-square "NuGet Downloads"
[nuget-u3i45q7/mnfg]: https://www.nuget.org/packages/Rocket.Surgery.MSBuild.GlobalAnalyzerConfig/
[nuget-version-u3i45q7/mnfg-badge]: https://img.shields.io/nuget/v/Rocket.Surgery.MSBuild.GlobalAnalyzerConfig.svg?color=004880&logo=nuget&style=flat-square "NuGet Version"
[nuget-downloads-u3i45q7/mnfg-badge]: https://img.shields.io/nuget/dt/Rocket.Surgery.MSBuild.GlobalAnalyzerConfig.svg?color=004880&logo=nuget&style=flat-square "NuGet Downloads"
[nuget-58lqc0lqtjla]: https://www.nuget.org/packages/Rocket.Surgery.MSBuild.JetBrains.Annotations/
[nuget-version-58lqc0lqtjla-badge]: https://img.shields.io/nuget/v/Rocket.Surgery.MSBuild.JetBrains.Annotations.svg?color=004880&logo=nuget&style=flat-square "NuGet Version"
[nuget-downloads-58lqc0lqtjla-badge]: https://img.shields.io/nuget/dt/Rocket.Surgery.MSBuild.JetBrains.Annotations.svg?color=004880&logo=nuget&style=flat-square "NuGet Downloads"
[nuget-su+gulvppohw]: https://www.nuget.org/packages/Rocket.Surgery.MSBuild.SourceLink/
[nuget-version-su+gulvppohw-badge]: https://img.shields.io/nuget/v/Rocket.Surgery.MSBuild.SourceLink.svg?color=004880&logo=nuget&style=flat-square "NuGet Version"
[nuget-downloads-su+gulvppohw-badge]: https://img.shields.io/nuget/dt/Rocket.Surgery.MSBuild.SourceLink.svg?color=004880&logo=nuget&style=flat-square "NuGet Downloads"
<!-- generated references -->

<!-- nuke-data
github:
  owner: RocketSurgeonsGuild
  repository: MSBuild.Targets
-->
