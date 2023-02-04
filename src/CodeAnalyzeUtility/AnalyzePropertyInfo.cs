using Microsoft.CodeAnalysis;
using System;
using System.Threading;

namespace CodeAnalyzeUtility
{
    public class AnalyzePropertyInfo : AnalyzeBase<AnalyzePropertyInfo>
    {
        public IPropertySymbol Symbol { get; private set; }
        public AnalyzeTypeInfo Type { get; private set; }
        public AnalyzeAttributeInfo[] Attributes { get; private set; } = Array.Empty<AnalyzeAttributeInfo>();
        public string Name { get; private set; } = string.Empty;
        public bool HasDefaultValue { get; private set; } = false;
        public string DefaultValue { get; private set; } = string.Empty;

        public static AnalyzePropertyInfo Analyze<SymbolType>(SymbolType symbol, CancellationToken cancellationToken = default) where SymbolType : IPropertySymbol
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = new AnalyzePropertyInfo(
                symbol,
                AnalyzeTypeInfo.Analyze(symbol.Type, cancellationToken));
            result.Attributes = symbol.GetAttributes().Select(x => AnalyzeAttributeInfo.Analyze(x, cancellationToken)).ToArray();
            result.Name = symbol.Name;
            (result.HasDefaultValue, result.DefaultValue) = symbol.DeclaringSyntaxReferences.GetDefaultValue();
            return result;
        }

        public override IEnumerable<AnalyzeInfoType> GetAnalyzeInfos<AnalyzeInfoType>(CancellationToken cancellationToken = default)
            => new[]
            {
                ToTypedEnumerable<AnalyzeInfoType,AnalyzePropertyInfo>(this,cancellationToken),
                ToTypedRecursiveEnumerable<AnalyzeInfoType,AnalyzeAttributeInfo>(Attributes,cancellationToken),
                ToTypedRecursiveEnumerable<AnalyzeInfoType,AnalyzeTypeInfo>(Type,cancellationToken),
            }.SelectMany(x => x);

        private AnalyzePropertyInfo(IPropertySymbol symbol, AnalyzeTypeInfo type)
        {
            Symbol = symbol;
            Type = type;
        }
    }
}
