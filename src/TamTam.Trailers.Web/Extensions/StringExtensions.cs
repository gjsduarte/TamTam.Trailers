﻿namespace TamTam.Trailers.Web.Extensions
{
    using System;

    public static class StringExtensions
    {
        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;

            maxLength = Math.Abs(maxLength);

            return value.Length <= maxLength
                ? value
                : value.Substring(0, maxLength);
        }
    }
}