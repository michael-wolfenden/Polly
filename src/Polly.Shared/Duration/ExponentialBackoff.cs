using System;
using System.Collections.Generic;

namespace Polly.Duration
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ExponentialBackoff : ISleepDurationStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        public int RetryCount { get; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan MinDelay { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retryCount"></param>
        /// <param name="minDelay"></param>
        public ExponentialBackoff(int retryCount, TimeSpan minDelay)
        {
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount));
            if (minDelay < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(minDelay));

            RetryCount = retryCount;
            MinDelay = minDelay;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<TimeSpan> Generate()
        {
            TimeSpan[] delays = new TimeSpan[RetryCount];

            double ms = MinDelay.TotalMilliseconds;
            for (int i = 0; i < delays.Length; i++)
            {
                delays[i] = TimeSpan.FromMilliseconds(ms);

                ms *= 2.0;
            }

            return delays;
        }
    }
}
