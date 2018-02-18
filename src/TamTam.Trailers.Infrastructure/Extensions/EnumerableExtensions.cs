namespace TamTam.Trailers.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public static class EnumerableExtensions
    {
        #region Public Methods and Operators

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static Task<T[]> ToArray<T>(this Task<IEnumerable<T>> source,
            CancellationToken token = default(CancellationToken))
        {
            return ExecuteAsync(source, x => x?.ToArray(), token);
        }

        public static Task<List<T>> ToList<T>(this Task<IEnumerable<T>> source,
            CancellationToken token = default(CancellationToken))
        {
            return ExecuteAsync(source, x => x?.ToList(), token);
        }

        #endregion

        #region Methods

        private static async Task<TResult> ExecuteAsync<T, TResult>(Task<IEnumerable<T>> source,
            Func<IEnumerable<T>, TResult> projection,
            CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();
            var result = await source.ConfigureAwait(false);
            return projection(result);
        }

        #endregion
    }
}