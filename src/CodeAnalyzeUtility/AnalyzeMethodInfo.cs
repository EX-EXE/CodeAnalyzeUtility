using Microsoft.CodeAnalysis;
using System;
namespace CodeAnalyzeUtility
{
    public class AnalyzeMethodInfo
    {
        public static AnalyzeMethodInfo Analyze<SymbolType>(SymbolType symbol) where SymbolType : IMethodSymbol
        {
            var result = new AnalyzeMethodInfo();
            result.Attributes = symbol.GetAttributes().Select(x => AnalyzeAttributeInfo.Analyze(x)).ToArray();
            result.MethodName = symbol.Name;
            result.ReturnType = AnalyzeTypeInfo.Analyze(symbol.ReturnType);
            result.Parameters = symbol.Parameters.Select(x => AnalyzeParameterInfo.Analyze(x)).ToArray();
            return result;
        }
        public AnalyzeAttributeInfo[] Attributes { get; set; } = Array.Empty<AnalyzeAttributeInfo>();
        public AnalyzeTypeInfo ReturnType { get; set; } = new AnalyzeTypeInfo();
        public AnalyzeParameterInfo[] Parameters { get; set; } = Array.Empty<AnalyzeParameterInfo>();

        public string MethodName { get; set; } = string.Empty;
    }
}
