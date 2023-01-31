using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Text;
using CodeAnalyzeUtility;

namespace SourceGeneratorProject;

[Generator(LanguageNames.CSharp)]
public partial class SourceGeneratorProject : IIncrementalGenerator
{
    private static readonly string GeneratorNamespace = "SourceGeneratorProject";
    private static readonly string EnumHookAttribute = $"{GeneratorNamespace}.SourceGeneratorProjectAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // SourceGenerator用のAttribute生成
        GenerateAttribute(context);

        // Hook Attribute
        var source = context.SyntaxProvider.ForAttributeWithMetadataName(
            EnumHookAttribute,
            static (node, token) => true,
            static (context, token) => context);

        // CreateSource
        context.RegisterSourceOutput(source, GenerateSource);
    }

    public static void GenerateAttribute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static context =>
        {
            // Enumフック
            context.AddSource($"{EnumHookAttribute}.cs", """
namespace SourceGeneratorProject;
using System;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal sealed class SourceGeneratorProjectAttribute : Attribute
{
}
""");
        });
    }

    public static void GenerateSource(SourceProductionContext context, GeneratorAttributeSyntaxContext source)
    {
        var semanticModel = source.SemanticModel;
        var typeSymbol = (INamedTypeSymbol)source.TargetSymbol;
        var enumNode = (ClassDeclarationSyntax)source.TargetNode;

        var classInfo = AnalyzeClassInfo.Analyze(source.TargetSymbol);

    }

}