using Microsoft.CodeAnalysis;
using System;
using System.Threading;
using System.Xml.Linq;

namespace CodeAnalyzeUtility
{
    public class AnalyzeTypeInfo : AnalyzeBase<AnalyzeTypeInfo>
    {
        public ISymbol Symbol { get; private set; }
        public string Namespace { get; private set; } = string.Empty;
        public string ShortName { get; private set; } = string.Empty;
        public string ShortNameWithGenerics { get; private set; } = string.Empty;
        public string FullName { get; private set; } = string.Empty;
        public string FullNameWithGenerics { get; private set; } = string.Empty;

        public static AnalyzeTypeInfo Analyze<SymbolType>(SymbolType symbol, CancellationToken cancellationToken = default) where SymbolType : ISymbol
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = new AnalyzeTypeInfo(symbol);
            result.Namespace = symbol.ContainingNamespace.ToTypeFullName(true);
            result.ShortName = symbol.ToTypeShortName(false);
            result.ShortNameWithGenerics = symbol.ToTypeShortName(true);
            result.FullName = symbol.ToTypeFullName(false);
            result.FullNameWithGenerics = symbol.ToTypeFullName(true);
            return result;
        }

        public override IEnumerable<AnalyzeInfoType> GetAnalyzeInfos<AnalyzeInfoType>(CancellationToken cancellationToken = default)
            => new[]
            {
                ToTypedEnumerable<AnalyzeInfoType,AnalyzeTypeInfo>(this,cancellationToken),
            }.SelectMany(x => x);

        public string GetNamespaceDefine()
            => Symbol.ContainingNamespace.IsGlobalNamespace
                ? string.Empty
                : $"namespace {Namespace};";

        private AnalyzeTypeInfo(ISymbol symbol)
        {
            Symbol = symbol;
        }

    }
}
