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

        internal static (bool,string) GetDefaultValue(this ImmutableArray<SyntaxReference> syntaxReferences)
        {
            foreach(var syntaxReference in syntaxReferences)
            {
                var syntax = syntaxReference.GetSyntax();
                if(syntax != null)
                {
                    foreach(var equalsValueClauseSyntax in syntax.DescendantNodes().OfType<EqualsValueClauseSyntax>())
                    {
                        return (true, equalsValueClauseSyntax.Value.ToFullString());
                    }
                }
            }
            return (false, string.Empty);
        }

    }
}
