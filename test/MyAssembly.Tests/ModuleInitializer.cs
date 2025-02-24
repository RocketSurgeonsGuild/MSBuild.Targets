using System.Runtime.CompilerServices;
using DiffEngine;
using Microsoft.CodeAnalysis;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Rocket.Surgery.MyAssembly.Tests;

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