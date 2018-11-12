using System;
using System.Collections.Generic;

namespace Polly.Duration
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ExponentialBackoff : ISleepDurationStrategy
    {
        private static readonly Random s_random = new Random(); // Default ctor uses a time-based seed
        private readonly Random _random;

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
        public TimeSpan MaxDelay { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retryCount"></param>
        /// <param name="minDelay"></param>
        /// <param name="maxDelay"></param>
        /// <param name="random"></param>
        public ExponentialBackoff(int retryCount, TimeSpan minDelay, TimeSpan maxDelay, Random random = null)
        {
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount));
            if (minDelay < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(minDelay));
            if (maxDelay < minDelay) throw new ArgumentOutOfRangeException(nameof(maxDelay));

            _random = random ?? s_random;
            RetryCount = retryCount;
            MinDelay = minDelay;
            MaxDelay = maxDelay;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TimeSpan> Create(Context content = null)
        {
            double ms = MinDelay.TotalMilliseconds;

            for (int i = 0; i < RetryCount; i++)
            {
                ms *= 3.0 * _random.NextDouble(); // [0.0, 3.0)
                ms = Math.Max(MinDelay.TotalMilliseconds, ms); // [min, N]
                ms = Math.Min(MaxDelay.TotalMilliseconds, ms); // [min, max]

                yield return TimeSpan.FromMilliseconds(ms);
            }
        }

        public TimeSpan Next(int i, Context content = null)
        {
            throw new NotImplementedException();
        }
    }
}
