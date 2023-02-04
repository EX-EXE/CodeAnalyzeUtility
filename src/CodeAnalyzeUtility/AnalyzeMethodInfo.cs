using Microsoft.CodeAnalysis;
using System;
using System.Threading;

namespace CodeAnalyzeUtility
{
    public class AnalyzeMethodInfo : AnalyzeBase<AnalyzeMethodInfo>
    {
        public IMethodSymbol Symbol { get; private set; }
        public AnalyzeTypeInfo ReturnType { get; private set; }
        public AnalyzeAttributeInfo[] Attributes { get; private set; } = Array.Empty<AnalyzeAttributeInfo>();
        public AnalyzeParameterInfo[] Parameters { get; private set; } = Array.Empty<AnalyzeParameterInfo>();
        public string MethodName { get; private set; } = string.Empty;

        public static AnalyzeMethodInfo Analyze<SymbolType>(SymbolType symbol, CancellationToken cancellationToken = default) where SymbolType : IMethodSymbol
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = new AnalyzeMethodInfo(
                symbol,
                AnalyzeTypeInfo.Analyze(symbol.ReturnType, cancellationToken));
            result.Attributes = symbol.GetAttributes().Select(x => AnalyzeAttributeInfo.Analyze(x, cancellationToken)).ToArray();
            result.MethodName = symbol.Name;
            result.Parameters = symbol.Parameters.Select(x => AnalyzeParameterInfo.Analyze(x, cancellationToken)).ToArray();
            return result;
        }

        public override IEnumerable<AnalyzeInfoType> GetAnalyzeInfos<AnalyzeInfoType>(CancellationToken cancellationToken = default)
            => new[]
            {
                ToTypedEnumerable<AnalyzeInfoType,AnalyzeMethodInfo>(this,cancellationToken),
                ToTypedRecursiveEnumerable<AnalyzeInfoType,AnalyzeAttributeInfo>(Attributes,cancellationToken),
                ToTypedRecursiveEnumerable<AnalyzeInfoType,AnalyzeTypeInfo>(ReturnType,cancellationToken),
                ToTypedRecursiveEnumerable<AnalyzeInfoType,AnalyzeParameterInfo>(Parameters,cancellationToken),
            }.SelectMany(x => x);
        private AnalyzeMethodInfo(IMethodSymbol symbol, AnalyzeTypeInfo returnType)
        {
            Symbol = symbol;
            ReturnType = returnType;
        }

    }
}
