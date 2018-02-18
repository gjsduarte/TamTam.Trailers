namespace TamTam.Trailers.Infrastructure.Extensions
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Caching.Distributed;

    using Newtonsoft.Json;

    public static class CachingExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the value of type dynamic with the specified key from the cache asynchronously by
        ///     deserializing it from JSON format or returns <c>null</c> if the key was not found.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="encoding">The encoding of the JSON or <c>null</c> to use UTF-8.</param>
        /// <returns>The value of type dynamic or <c>null</c> if the key was not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cache" /> or <paramref name="key" /> is <c>null</c>.</exception>
        public static Task<dynamic> GetAsJsonAsync(this IDistributedCache cache,
            string key,
            Encoding encoding = null)
        {
            return GetAsJsonAsync<dynamic>(cache, key, encoding);
        }
        
        /// <summary>
        ///     Gets the value of type <typeparamref name="T" /> with the specified key from the cache asynchronously by
        ///     deserializing it from JSON format or returns <c>null</c> if the key was not found.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="encoding">The encoding of the JSON or <c>null</c> to use UTF-8.</param>
        /// <returns>The value of type <typeparamref name="T" /> or <c>null</c> if the key was not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cache" /> or <paramref name="key" /> is <c>null</c>.</exception>
        public static async Task<T> GetAsJsonAsync<T>(this IDistributedCache cache,
            string key,
            Encoding encoding = null)
            where T : class
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var json = await GetStringAsync(cache, key, encoding).ConfigureAwait(false);
            if (json == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        ///     Gets the <see cref="string" /> value with the specified key from the cache asynchronously or returns
        ///     <c>null</c> if the key was not found.
        /// </summary>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="encoding">The encoding of the <see cref="string" /> value or <c>null</c> to use UTF-8.</param>
        /// <returns>The <see cref="string" /> value or <c>null</c> if the key was not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cache" /> or <paramref name="key" /> is <c>null</c>.</exception>
        public static async Task<string> GetStringAsync(this IDistributedCache cache,
            string key,
            Encoding encoding = null)
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var bytes = await cache.GetAsync(key).ConfigureAwait(false);
            if (bytes == null)
            {
                return null;
            }

            return encoding.GetString(bytes);
        }

        /// <summary>
        ///     Sets the value of type <typeparamref name="T" /> with the specified key in the cache asynchronously by
        ///     serializing it to JSON format.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="value">The cache item value.</param>
        /// <param name="options">The cache options or <c>null</c> to use the default cache options.</param>
        /// <returns>The value of type <typeparamref name="T" /> or <c>null</c> if the key was not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cache" /> or <paramref name="key" /> is <c>null</c>.</exception>
        public static Task SetAsJsonAsync<T>(this IDistributedCache cache,
            string key,
            T value,
            DistributedCacheEntryOptions options = null)
            where T : class
        {
            return SetAsJsonAsync(cache, key, value, null, options);
        }

        /// <summary>
        ///     Sets the value of type <typeparamref name="T" /> with the specified key in the cache asynchronously by
        ///     serializing it to JSON format.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="value">The cache item value.</param>
        /// <param name="encoding">The encoding to use for the JSON or <c>null</c> to use UTF-8.</param>
        /// <param name="options">The cache options or <c>null</c> to use the default cache options.</param>
        /// <returns>The value of type <typeparamref name="T" /> or <c>null</c> if the key was not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cache" /> or <paramref name="key" /> is <c>null</c>.</exception>
        public static Task SetAsJsonAsync<T>(this IDistributedCache cache,
            string key,
            T value,
            Encoding encoding = null,
            DistributedCacheEntryOptions options = null)
            where T : class
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            if (options == null)
            {
                options = new DistributedCacheEntryOptions();
            }

            var json = JsonConvert.SerializeObject(value, Formatting.None);
            return SetAsync(cache, key, json, encoding, options);
        }

        /// <summary>
        ///     Sets the <see cref="string" /> value with the specified key in the cache asynchronously.
        /// </summary>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="value">The cache item value.</param>
        /// <param name="options">The cache options or <c>null</c> to use the default cache options.</param>
        /// <returns>A task representing this action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cache" /> or <paramref name="key" /> is <c>null</c>.</exception>
        public static Task SetAsync(this IDistributedCache cache,
            string key,
            string value,
            DistributedCacheEntryOptions options = null)
        {
            return SetAsync(cache, key, value, null, options);
        }

        /// <summary>
        ///     Sets the <see cref="string" /> value with the specified key in the cache asynchronously.
        /// </summary>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="value">The cache item value.</param>
        /// <param name="encoding">The <see cref="string" /> values encoding or <c>null</c> for UTF-8.</param>
        /// <param name="options">The cache options or <c>null</c> to use the default cache options.</param>
        /// <returns>A task representing this action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cache" /> or <paramref name="key" /> is <c>null</c>.</exception>
        public static Task SetAsync(this IDistributedCache cache,
            string key,
            string value,
            Encoding encoding = null,
            DistributedCacheEntryOptions options = null)
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            if (options == null)
            {
                options = new DistributedCacheEntryOptions();
            }

            var bytes = encoding.GetBytes(value);
            return cache.SetAsync(key, bytes, options);
        }

        #endregion
    }
}