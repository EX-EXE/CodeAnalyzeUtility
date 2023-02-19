using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace CodeAnalyzeUtility
{
    public static class AnalyzeUtilityExtensions
    {
        public static string ToTypeShortName(this ISymbol symbol, bool includeGenerics)
        {
            return symbol.ToDisplayString(new SymbolDisplayFormat(
                globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypes,
                genericsOptions: includeGenerics ? SymbolDisplayGenericsOptions.IncludeTypeParameters : SymbolDisplayGenericsOptions.None,
                miscellaneousOptions:
                    SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers |
                    SymbolDisplayMiscellaneousOptions.UseSpecialTypes));
        }

        public static string ToTypeFullName(this ISymbol symbol, bool includeGenerics)
        {
            return symbol.ToDisplayString(new SymbolDisplayFormat(
                globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                genericsOptions: includeGenerics ? SymbolDisplayGenericsOptions.IncludeTypeParameters : SymbolDisplayGenericsOptions.None,
                miscellaneousOptions:
                    SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers |
                    SymbolDisplayMiscellaneousOptions.UseSpecialTypes));
        }
        internal static string ToAccessibilityString<T>(this T symbol) where T : ISymbol
        {
            return symbol.DeclaredAccessibility switch
            {
                Accessibility.Private => "private",
                Accessibility.ProtectedAndInternal => "protected internal",
                //Accessibility.ProtectedAndFriend => "protected friend",
                Accessibility.Protected => "protected",
                Accessibility.Internal => "internal",
                //Accessibility.Friend => "friend",
                Accessibility.Public => "public",
                _ => string.Empty,
            };
        }
        internal static string ToRefString<T>(this T symbol) where T : ISymbol
        {
            if (symbol is IPropertySymbol propertySymbol)
            {
                return propertySymbol.RefKind.ToRefString();
            }
            else if (symbol is IParameterSymbol parameterSymbol)
            {
                return parameterSymbol.RefKind.ToRefString();
            }
            else if (symbol is IMethodSymbol methodSymbol)
            {
                return methodSymbol.RefKind.ToRefString();
            }
            return string.Empty;
        }
        internal static string ToRefString(this RefKind refKind)
        {
            return refKind switch
            {
                RefKind.Ref => "ref",
                RefKind.Out => "out",
                RefKind.In => "in",
                //RefKind.RefReadOnly => "ref readonly",
                _ => string.Empty,
            };
        }

        internal static (bool, string) GetDefaultValue(this ImmutableArray<SyntaxReference> syntaxReferences)
        {
            foreach (var syntaxReference in syntaxReferences)
            {
                var syntax = syntaxReference.GetSyntax();
                if (syntax != null)
                {
                    foreach (var equalsValueClauseSyntax in syntax.DescendantNodes().OfType<EqualsValueClauseSyntax>())
                    {
                        return (true, equalsValueClauseSyntax.Value.ToFullString());
                    }
                }
            }
            return (false, string.Empty);
        }

    }
}
