namespace TamTam.Trailers.Web.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public static class EnumerableExtensions
    {
        public static Task<T[]> ToArray<T>(this Task<IEnumerable<T>> source,
            CancellationToken token = default(CancellationToken))
        {
            return ExecuteAsync(source, x => x.ToArray(), token);
        }

        #region Methods

        private static async Task<TResult> ExecuteAsync<T, TResult>(Task<IEnumerable<T>> source,
            Func<IEnumerable<T>, TResult> projection,
            CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();
            var result = await source;
            return projection(result);
        }

        #endregion
    }
}