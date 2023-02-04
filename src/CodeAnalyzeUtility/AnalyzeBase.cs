using Microsoft.CodeAnalysis;
using System;
using System.Threading;

namespace CodeAnalyzeUtility
{
    public interface IAnalyzeInfo<AnalyzeType>
    {
        public IEnumerable<AnalyzeInfoType> GetAnalyzeInfos<AnalyzeInfoType>(CancellationToken cancellationToken = default) where AnalyzeInfoType : AnalyzeBase<AnalyzeInfoType>;
    }

    public abstract class AnalyzeBase<AnalyzeType> : IAnalyzeInfo<AnalyzeType>
    {
        private Type type = typeof(AnalyzeType);

        public abstract IEnumerable<AnalyzeInfoType> GetAnalyzeInfos<AnalyzeInfoType>(CancellationToken cancellationToken = default) where AnalyzeInfoType : AnalyzeBase<AnalyzeInfoType>;

        internal static IEnumerable<AnalyzeInfoType> ToTypedEnumerable<AnalyzeInfoType, T>(T info, CancellationToken cancellationToken = default)
             where AnalyzeInfoType : AnalyzeBase<AnalyzeInfoType>
             where T : AnalyzeBase<T>
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (info is AnalyzeInfoType typed)
            {
                yield return typed;
            }
        }
        internal static IEnumerable<AnalyzeInfoType> ToTypedRecursiveEnumerable<AnalyzeInfoType, T>(AnalyzeBase<T> info, CancellationToken cancellationToken = default)
            where AnalyzeInfoType : AnalyzeBase<AnalyzeInfoType>
            where T : AnalyzeBase<T>
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var typedInfo in info.GetAnalyzeInfos<AnalyzeInfoType>(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return typedInfo;
            }
        }
        internal static IEnumerable<AnalyzeInfoType> ToTypedRecursiveEnumerable<AnalyzeInfoType, T>(AnalyzeBase<T>[] infoArray, CancellationToken cancellationToken = default)
            where AnalyzeInfoType : AnalyzeBase<AnalyzeInfoType>
            where T : AnalyzeBase<T>
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var typedInfo in infoArray.SelectMany(x => x.GetAnalyzeInfos<AnalyzeInfoType>(cancellationToken)))
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return typedInfo;
            }
        }
    }
}
