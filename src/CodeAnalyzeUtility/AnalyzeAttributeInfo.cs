using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace CodeAnalyzeUtility
{
    public class AnalyzeAttributeInfo
    {
        public static AnalyzeAttributeInfo Analyze(AttributeData attributeData)
        {
            var result = new AnalyzeAttributeInfo();
            if(attributeData.AttributeClass != null)
            {
                result.Type = AnalyzeTypeInfo.Analyze(attributeData.AttributeClass);

                // Arguments
                var syntaxReference = attributeData.ApplicationSyntaxReference;
                if(syntaxReference != null)
                {
                    var attributeSyntax = syntaxReference.GetSyntax() as AttributeSyntax;
                    if(attributeSyntax != null && attributeSyntax.ArgumentList != null)
                    {
                        result.Arguments = attributeSyntax.ArgumentList.DescendantNodes().OfType<AttributeArgumentSyntax>().Select(x => x.ToFullString()).ToArray();
                    }
                }

                // GenericTypes
                var namedTypeSymbol = attributeData.AttributeClass as INamedTypeSymbol;
                if(namedTypeSymbol != null && namedTypeSymbol.IsGenericType)
                {
                    result.GenericTypes = namedTypeSymbol.TypeArguments.Select(x => AnalyzeTypeInfo.Analyze(x)).ToArray();
                }
            }

            return result;
        }

        public AnalyzeTypeInfo Type { get; set; } = new AnalyzeTypeInfo();
        public AnalyzeTypeInfo[] GenericTypes { get; set; } = Array.Empty<AnalyzeTypeInfo>();
        public string[] Arguments { get; set; } = Array.Empty<string>();
    }
}
