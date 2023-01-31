using Microsoft.CodeAnalysis;
using System;

namespace CodeAnalyzeUtility
{
    public class AnalyzePropertyInfo
    {
        public static AnalyzePropertyInfo Analyze<SymbolType>(SymbolType symbol) where SymbolType : IPropertySymbol
        {
            var result = new AnalyzePropertyInfo();
            result.Attributes = symbol.GetAttributes().Select(x => AnalyzeAttributeInfo.Analyze(x)).ToArray();
            result.Name = symbol.Name;
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
