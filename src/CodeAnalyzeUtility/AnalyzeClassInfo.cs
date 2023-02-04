using Microsoft.CodeAnalysis;
using System;
using System.Threading;

namespace CodeAnalyzeUtility
{
    public class AnalyzeClassInfo : AnalyzeBase<AnalyzeClassInfo>
    {
        public INamedTypeSymbol Symbol { get; private set; }
        public AnalyzeTypeInfo Type { get; private set; }
        public AnalyzeTypeInfo BaseType { get; private set; }
        public AnalyzeTypeInfo[] InterfaceTypes { get; private set; } = Array.Empty<AnalyzeTypeInfo>();
        public AnalyzeAttributeInfo[] Attributes { get; private set; } = Array.Empty<AnalyzeAttributeInfo>();
        public AnalyzeFieldInfo[] Fields { get; private set; } = Array.Empty<AnalyzeFieldInfo>();
        public AnalyzePropertyInfo[] Properties { get; private set; } = Array.Empty<AnalyzePropertyInfo>();
        public AnalyzeMethodInfo[] Methods { get; private set; } = Array.Empty<AnalyzeMethodInfo>();
        public AnalyzeClassInfo[] Classes { get; private set; } = Array.Empty<AnalyzeClassInfo>();

        public static AnalyzeClassInfo Analyze<SymbolType>(SymbolType symbol, CancellationToken cancellationToken = default) where SymbolType : INamedTypeSymbol
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (symbol.BaseType == null)
            {
                throw new ArgumentNullException(nameof(symbol.BaseType));
            }

            var result = new AnalyzeClassInfo(
                symbol,
                AnalyzeTypeInfo.Analyze(symbol, cancellationToken),
                AnalyzeTypeInfo.Analyze(symbol.BaseType, cancellationToken));

            result.InterfaceTypes = symbol.Interfaces.Select(x => AnalyzeTypeInfo.Analyze(x, cancellationToken)).ToArray();

            result.Attributes = symbol.GetAttributes().Select(x => AnalyzeAttributeInfo.Analyze(x, cancellationToken)).ToArray();

            var fieldSymbols = symbol.GetMembers().OfType<IFieldSymbol>().Where(x => !x.IsImplicitlyDeclared);
            result.Fields = fieldSymbols.Select(x => AnalyzeFieldInfo.Analyze(x, cancellationToken)).ToArray();

            var propertySymbols = symbol.GetMembers().OfType<IPropertySymbol>().Where(x => !x.IsImplicitlyDeclared);
            result.Properties = propertySymbols.Select(x => AnalyzePropertyInfo.Analyze(x, cancellationToken)).ToArray();

            var methodSymbols = symbol.GetMembers().OfType<IMethodSymbol>().Where(x => !x.IsImplicitlyDeclared && x.MethodKind != MethodKind.PropertyGet && x.MethodKind != MethodKind.PropertySet);
            result.Methods = methodSymbols.Select(x => AnalyzeMethodInfo.Analyze(x, cancellationToken)).ToArray();

            var classSymbols = symbol.GetMembers().OfType<INamedTypeSymbol>().Where(x => !x.IsImplicitlyDeclared);
            result.Classes = classSymbols.Select(x => AnalyzeClassInfo.Analyze(x, cancellationToken)).ToArray();
            return result;
        }

        public override IEnumerable<AnalyzeInfoType> GetAnalyzeInfos<AnalyzeInfoType>(CancellationToken cancellationToken = default)
            => new[]
            {
                ToTypedEnumerable<AnalyzeInfoType,AnalyzeClassInfo>(this,cancellationToken),
                ToTypedRecursiveEnumerable<AnalyzeInfoType,AnalyzeTypeInfo>(Type,cancellationToken),
                ToTypedRecursiveEnumerable<AnalyzeInfoType,AnalyzeAttributeInfo>(Attributes,cancellationToken),
                ToTypedRecursiveEnumerable<AnalyzeInfoType,AnalyzeFieldInfo>(Fields,cancellationToken),
                ToTypedRecursiveEnumerable<AnalyzeInfoType,AnalyzePropertyInfo>(Properties,cancellationToken),
                ToTypedRecursiveEnumerable<AnalyzeInfoType,AnalyzeMethodInfo>(Methods,cancellationToken),
                ToTypedRecursiveEnumerable<AnalyzeInfoType,AnalyzeClassInfo>(Classes,cancellationToken),
            }.SelectMany(x => x);

        private AnalyzeClassInfo(INamedTypeSymbol symbol, AnalyzeTypeInfo type, AnalyzeTypeInfo baseType)
        {
            Symbol = symbol;
            Type = type;
            BaseType = baseType;
        }
    }
}
