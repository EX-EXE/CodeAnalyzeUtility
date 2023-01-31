using Microsoft.CodeAnalysis;
using System;

namespace CodeAnalyzeUtility
{
    public class AnalyzeTypeInfo
    {
        public static AnalyzeTypeInfo Analyze<SymbolType>(SymbolType symbol) where SymbolType : ISymbol
        {
            var result = new AnalyzeTypeInfo();
            if (symbol != null)
            {
                result.Namespace = symbol.ContainingNamespace.ToTypeFullName(true);
                result.ShortName = symbol.ToTypeShortName(false);
                result.ShortNameWithGenerics = symbol.ToTypeShortName(true);
                result.FullName = symbol.ToTypeFullName(false);
                result.FullNameWithGenerics = symbol.ToTypeFullName(true);
            }
            return result;
        }

        public string Namespace { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string ShortNameWithGenerics { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string FullNameWithGenerics { get; set; } = string.Empty;

    }
}
