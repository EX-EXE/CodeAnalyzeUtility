using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace CodeAnalyzeUtility
{
    public class AnalyzeAttributeInfo : AnalyzeBase<AnalyzeAttributeInfo>
    {
        public AttributeData Attribute { get; private set; }
        public AnalyzeTypeInfo Type { get; private set; }
        public AnalyzeTypeInfo[] GenericTypes { get; private set; } = Array.Empty<AnalyzeTypeInfo>();
        public string[] ArgumentStrings { get; private set; } = Array.Empty<string>();
        public object?[] ArgumentObjects { get; private set; } = Array.Empty<object?>();

        public static AnalyzeAttributeInfo Analyze(AttributeData attributeData, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var attributeClass = attributeData.AttributeClass;
            if (attributeClass == null)
            {
                throw new ArgumentException($"{nameof(attributeData.AttributeClass)} is null.");
            }
            var result = new AnalyzeAttributeInfo(
                attributeData,
                AnalyzeTypeInfo.Analyze(attributeClass, cancellationToken));


            // ArgumentObjects
            object? Convert(TypedConstant typedConstant)
            {
                if (typedConstant.Type != null)
                {
                    if (typedConstant.Type.Kind == SymbolKind.ArrayType)
                    {
                        return typedConstant.Values.Select(x => Convert(x)).ToArray();
                    }
                    else
                    {
                        return typedConstant.Value;
                    }
                }
                return null;
            }
            result.ArgumentObjects = attributeData.ConstructorArguments.Select(x => Convert(x)).ToArray();

            // ArgumentStrings
            var syntaxReference = attributeData.ApplicationSyntaxReference;
            if (syntaxReference != null)
            {
                var attributeSyntax = syntaxReference.GetSyntax() as AttributeSyntax;
                if (attributeSyntax != null && attributeSyntax.ArgumentList != null)
                {
                    result.ArgumentStrings = attributeSyntax.ArgumentList.DescendantNodes().OfType<AttributeArgumentSyntax>().Select(x => x.ToFullString()).ToArray();
                }
            }

            // GenericTypes
            if (attributeClass.IsGenericType)
            {
                result.GenericTypes = attributeClass.TypeArguments
                    .Select(x => AnalyzeTypeInfo.Analyze(x, cancellationToken)).ToArray();
            }

            return result;
        }

        public override IEnumerable<AnalyzeInfoType> GetAnalyzeInfos<AnalyzeInfoType>(CancellationToken cancellationToken = default)
            => new[]
            {
                ToTypedEnumerable<AnalyzeInfoType,AnalyzeAttributeInfo>(this,cancellationToken),
                ToTypedRecursiveEnumerable<AnalyzeInfoType,AnalyzeTypeInfo>(Type,cancellationToken),
                ToTypedRecursiveEnumerable<AnalyzeInfoType,AnalyzeTypeInfo>(GenericTypes,cancellationToken),
            }.SelectMany(x => x);

        private AnalyzeAttributeInfo(AttributeData attribute, AnalyzeTypeInfo analyzeTypeInfo)
        {
            Attribute = attribute;
            Type = analyzeTypeInfo;
        }

    }
}
