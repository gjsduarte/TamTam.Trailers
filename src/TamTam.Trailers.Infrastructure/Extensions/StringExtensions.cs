namespace TamTam.Trailers.Infrastructure.Extensions
{
    using System;

    public static class StringExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns the first X characters of a string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The length.</param>
        public static string Left(this string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return value;

            length = Math.Abs(length);

            return value.Length <= length
                ? value
                : value.Substring(0, length);
        }

        #endregion
    }
}