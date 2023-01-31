using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Text;
using System.Reflection;

namespace CodeAnalyzeUtilityGenerator;

[Generator(LanguageNames.CSharp)]
public partial class CodeAnalyzeUtilityGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static context =>
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                using var resourceStream = assembly.GetManifestResourceStream(resourceName);
                using var memoryStream = new MemoryStream();
                resourceStream.CopyTo(memoryStream);
                var code = Encoding.UTF8.GetString(memoryStream.ToArray());
                context.AddSource(resourceName, code);
            }
        });
    }
}