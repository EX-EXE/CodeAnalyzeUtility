[![NuGet version](https://badge.fury.io/nu/CodeAnalyzeUtilityGenerator.svg)](https://badge.fury.io/nu/CodeAnalyzeUtilityGenerator)

# CodeAnalyzeUtility
Utility for C# Source Generator.

## How To Use
### Install by nuget
PM> Install-Package [CodeAnalyzeUtilityGenerator](https://www.nuget.org/packages/CodeAnalyzeUtilityGenerator/)

```
// source(GeneratorAttributeSyntaxContext)
var typeSymbol = (INamedTypeSymbol)source.TargetSymbol;

// context(SourceProductionContext)
var cancellationToken = context.CancellationToken;

var classInfo = AnalyzeClassInfo.Analyze(typeSymbol, cancellationToken);
```

### Sample
#### Generator
```
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using CodeAnalyzeUtility;

namespace SourceGeneratorProject;

[Generator(LanguageNames.CSharp)]
public partial class SourceGeneratorProject : IIncrementalGenerator
{
    private static readonly string GeneratorNamespace = "SourceGeneratorProject";
    private static readonly string HookAttribute = $"{GeneratorNamespace}.HookAttribute";
    private static readonly string EnumFlagAttribute = $"{GeneratorNamespace}.EnumFlagAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        GenerateAttribute(context);
        // Hook Attribute
        var source = context.SyntaxProvider.ForAttributeWithMetadataName(
            HookAttribute,
            static (node, token) => true,
            static (context, token) => context);
        // CreateSource
        context.RegisterSourceOutput(source, GenerateSource);
    }

    public static void GenerateAttribute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static context =>
        {
            // HookAttribute
            context.AddSource($"{HookAttribute}.cs", """
namespace SourceGeneratorProject;
using System;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal sealed class HookAttribute : Attribute
{
}
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
internal sealed class EnumFlagAttribute : Attribute
{
}

""");
        });
    }

    public static void GenerateSource(SourceProductionContext context, GeneratorAttributeSyntaxContext source)
    {
        var cancellationToken = context.CancellationToken;
        var semanticModel = source.SemanticModel;
        var typeSymbol = (INamedTypeSymbol)source.TargetSymbol;
        var enumNode = (ClassDeclarationSyntax)source.TargetNode;

        var classInfo = AnalyzeClassInfo.Analyze(typeSymbol, cancellationToken);

        var enumFlagMembers = classInfo.GetAnalyzeInfos<AnalyzePropertyInfo>()
            .Where(x => x.Attributes.Where(x => x.Type.FullName.Equals(EnumFlagAttribute)).Any())
            .ToArray();

        var code = $$"""
// <auto-generated/>
using System.Collections.Generic;

{{classInfo.Type.GetNamespaceDefine()}}

partial class {{classInfo.Type.ShortName}}
{
    public enum MemberFlagType
    {
        None = 0,
{{enumFlagMembers
.ForEachIndexLines((i,x)=> $"{x.Name} = 1 << {i + 1},")
.OutputLine(2)}}
    }

    public static MemberFlagType[] MemberFlags => new[] { 
{{enumFlagMembers.ForEachLines(x => $"MemberFlagType.{x.Name},").OutputLine(2)}} };

    public static IEnumerable<MemberFlagType> GetFlags1()
    {
{{"MemberFlags".OutputForEachStatement(x => $"yield return {x};").OutputLine(2)}}
    }

    public static IEnumerable<MemberFlagType> GetFlags2()
    {
{{"i".OutputForStatement(0, enumFlagMembers.Length,(x => $"yield return MemberFlags[{x}];")).OutputLine(2)}}
    }
}
""";
        context.AddSource($"{classInfo.Type.FullName}.g.cs", code);
    }
}
```

#### Output
```
// <auto-generated/>
using System.Collections.Generic;

namespace CodeProject;

partial class TestClass1
{
    public enum MemberFlagType
    {
        None = 0,
		intProperty = 1 << 1,
		stringProperty = 1 << 2,
		longProperty = 1 << 3,
    }

    public static MemberFlagType[] MemberFlags => new[] { 
		MemberFlagType.intProperty,
		MemberFlagType.stringProperty,
		MemberFlagType.longProperty, };

    public static IEnumerable<MemberFlagType> GetFlags1()
    {
		foreach(var itemMemberFlags in MemberFlags)
		{
			yield return itemMemberFlags;
		}
    }

    public static IEnumerable<MemberFlagType> GetFlags2()
    {
		for(int i = 0; i < 3; ++i)
		{
			yield return MemberFlags[i];
		}
    }
}
```
