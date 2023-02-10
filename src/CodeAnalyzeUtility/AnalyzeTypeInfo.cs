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
        public bool IsArrayType { get; private set; } = false;

        public static AnalyzeTypeInfo Analyze<SymbolType>(SymbolType symbol, CancellationToken cancellationToken = default) where SymbolType : ISymbol
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(symbol.Kind == SymbolKind.ArrayType && symbol is IArrayTypeSymbol arraySymbolType)
            {
                var result = Analyze(arraySymbolType.ElementType, cancellationToken);
                result.IsArrayType = true;
                result.ShortName += "[]";
                result.ShortNameWithGenerics += "[]";
                result.FullName += "[]";
                result.FullNameWithGenerics += "[]";
                return result;
            }
            else if (symbol.Kind == SymbolKind.NamedType)
            {
                var result = new AnalyzeTypeInfo(symbol);
                result.IsArrayType = false;
                result.Namespace = symbol.ContainingNamespace.ToTypeFullName(true);
                result.ShortName = symbol.ToTypeShortName(false);
                result.ShortNameWithGenerics = symbol.ToTypeShortName(true);
                result.FullName = symbol.ToTypeFullName(false);
                result.FullNameWithGenerics = symbol.ToTypeFullName(true);
                return result;
            }
            else
            {
                throw new ArgumentException(nameof(symbol));
            }
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
