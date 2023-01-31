using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace CodeAnalyzeUtility
{
    public class AnalyzeParameterInfo
    {
        public static AnalyzeParameterInfo Analyze<SymbolType>(SymbolType symbol) where SymbolType : IParameterSymbol
        {
            var result = new AnalyzeParameterInfo();
            result.Attributes = symbol.GetAttributes().Select(x => AnalyzeAttributeInfo.Analyze(x)).ToArray();
            result.Type = AnalyzeTypeInfo.Analyze(symbol.Type);
            (result.HasDefaultValue, result.DefaultValue) = symbol.DeclaringSyntaxReferences.GetDefaultValue();
            return result;
        }

        public AnalyzeAttributeInfo[] Attributes { get; set; } = Array.Empty<AnalyzeAttributeInfo>();
        public AnalyzeTypeInfo Type { get; set; } = new AnalyzeTypeInfo();
        public string Name { get; set; } = string.Empty;
        public bool HasDefaultValue { get; set; } = false;
        public string DefaultValue { get; set; } = string.Empty;

    }
}
