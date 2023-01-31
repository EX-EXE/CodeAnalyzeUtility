using Microsoft.CodeAnalysis;
using System;

namespace CodeAnalyzeUtility
{
    public class AnalyzeClassInfo
    {
        public static AnalyzeClassInfo Analyze<SymbolType>(SymbolType symbol) where SymbolType : ISymbol
        {
            var result = new AnalyzeClassInfo();

            var namedTypeSymbol = symbol as INamedTypeSymbol;
            if (namedTypeSymbol != null)
            {
                result.Type = AnalyzeTypeInfo.Analyze(namedTypeSymbol);

                var members = namedTypeSymbol.GetMembers();

                result.Attributes = namedTypeSymbol.GetAttributes().Select(x => AnalyzeAttributeInfo.Analyze(x)).ToArray();

                var fieldSymbols = namedTypeSymbol.GetMembers().OfType<IFieldSymbol>().Where(x => !x.IsImplicitlyDeclared);
                result.Fields = fieldSymbols.Select(x => AnalyzeFieldInfo.Analyze(x)).ToArray();

                var propertySymbols = namedTypeSymbol.GetMembers().OfType<IPropertySymbol>().Where(x => !x.IsImplicitlyDeclared);
                result.Properties = propertySymbols.Select(x => AnalyzePropertyInfo.Analyze(x)).ToArray();

                var methodSymbols = namedTypeSymbol.GetMembers().OfType<IMethodSymbol>().Where(x => !x.IsImplicitlyDeclared && x.MethodKind != MethodKind.PropertyGet && x.MethodKind != MethodKind.PropertySet);
                result.Methods = methodSymbols.Select(x => AnalyzeMethodInfo.Analyze(x)).ToArray();

                var classSymbols = namedTypeSymbol.GetMembers().OfType<INamedTypeSymbol>().Where(x => !x.IsImplicitlyDeclared);
                result.Classes = classSymbols.Select(x => AnalyzeClassInfo.Analyze(x)).ToArray();
            }
            return result;
        }

        public AnalyzeTypeInfo Type { get; set; } = new AnalyzeTypeInfo();
        public AnalyzeAttributeInfo[] Attributes { get; set; } = Array.Empty<AnalyzeAttributeInfo>();
        public AnalyzeFieldInfo[] Fields { get; set; } = Array.Empty<AnalyzeFieldInfo>();
        public AnalyzePropertyInfo[] Properties { get; set; } = Array.Empty<AnalyzePropertyInfo>();
        public AnalyzeMethodInfo[] Methods { get; set; } = Array.Empty<AnalyzeMethodInfo>();
        public AnalyzeClassInfo[] Classes { get; set; } = Array.Empty<AnalyzeClassInfo>();

    }
}
