namespace TamTam.Trailers.Infrastructure
{
    using System;
    using Polly;

    public class Policies
    {
        #region Public Properties

        /// <summary>
        /// Retry policy that attempts an operation up to 3 times untul success with incresing wait intervals.
        /// </summary>
        public static Policy Retry =>
            Policy.Handle<Exception>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                });

        #endregion
    }
}