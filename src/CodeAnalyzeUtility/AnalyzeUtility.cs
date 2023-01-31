using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace CodeAnalyzeUtility
{
    public static class AnalyzeUtility
    {
        public static (bool,string) GetDefaultValue(this ImmutableArray<SyntaxReference> syntaxReferences)
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
        public static bool GetArguments(this SyntaxReference syntaxReference)
        {
            var syntax = syntaxReference.GetSyntax();
            if (syntax != null)
            {
                foreach (var equalsValueClauseSyntax in syntax.DescendantNodes().OfType<AttributeArgumentSyntax>())
                {
                    var a = equalsValueClauseSyntax.ToFullString();
                    //return (true, equalsValueClauseSyntax.Value.ToFullString());
                }
            }
            return true;
            // return (false, string.Empty);
        }
        public static bool GetArguments(this ImmutableArray<SyntaxReference> syntaxReferences)
        {
            foreach (var syntaxReference in syntaxReferences)
            {
                var syntax = syntaxReference.GetSyntax();
                if (syntax != null)
                {
                    foreach (var equalsValueClauseSyntax in syntax.DescendantNodes().OfType<AttributeArgumentSyntax>())
                    {
                        var a = equalsValueClauseSyntax.ToFullString();
                        //return (true, equalsValueClauseSyntax.Value.ToFullString());
                    }
                }
            }
            return true;
           // return (false, string.Empty);
        }

        public static string ToTypeShortName(this ISymbol symbol,bool includeGenerics)
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
    }
}
