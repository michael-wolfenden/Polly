﻿using System;
using System.Collections.Generic;
using System.Linq;
using Polly.Retry;

namespace Polly
{
    /// <summary>
    /// Fluent API for defining a Retry <see cref="Policy"/>. 
    /// </summary>
    public static class RetrySyntax
    {
        /// <summary>
        /// Builds a <see cref="Policy"/> that will retry once.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy Retry(this PolicyBuilder policyBuilder)
            => policyBuilder.Retry(1);

        /// <summary>
        /// Builds a <see cref="Policy"/> that will retry <paramref name="retryCount"/> times.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy Retry(this PolicyBuilder policyBuilder, int retryCount)
        {
            return policyBuilder.Retry(retryCount, onRetry: (Action<Exception, int>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will retry once
        /// calling <paramref name="onRetry"/> on retry with the raised exception and retry count.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy Retry(this PolicyBuilder policyBuilder, Action<Exception, int> onRetry)
            => policyBuilder.Retry(1, onRetry);

        /// <summary>
        /// Builds a <see cref="Policy"/> that will retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the raised exception and retry count.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">retryCount;Value must be greater than or equal to zero.</exception>
        public static ISyncRetryPolicy Retry(this PolicyBuilder policyBuilder, int retryCount, Action<Exception, int> onRetry)
        {
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount), "Value must be greater than or equal to zero.");

            return policyBuilder.Retry(retryCount, onRetry: onRetry == null ? (Action<Exception, int, Context>)null : (outcome, i, ctx) => onRetry(outcome, i));
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will retry once
        /// calling <paramref name="onRetry"/> on retry with the raised exception, retry count and context data.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy Retry(this PolicyBuilder policyBuilder, Action<Exception, int, Context> onRetry)
            => policyBuilder.Retry(1, onRetry);

        /// <summary>
        /// Builds a <see cref="Policy"/> that will retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the raised exception, retry count and context data.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">retryCount;Value must be greater than or equal to zero.</exception>
        public static ISyncRetryPolicy Retry(this PolicyBuilder policyBuilder, int retryCount, Action<Exception, int, Context> onRetry)
        {
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount), "Value must be greater than or equal to zero.");

            return new RetryPolicy(
                policyBuilder,
                onRetry: onRetry == null ? (Action<Exception, TimeSpan, int, Context>)null : (outcome, timespan, i, ctx) => onRetry(outcome, i, ctx),
                retryCount);
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will retry indefinitely until the action succeeds.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy RetryForever(this PolicyBuilder policyBuilder)
        {
            return policyBuilder.RetryForever(onRetry: (Action<Exception>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will retry indefinitely
        /// calling <paramref name="onRetry"/> on each retry with the raised exception.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy RetryForever(this PolicyBuilder policyBuilder, Action<Exception> onRetry)
        {
            return policyBuilder.RetryForever(onRetry: onRetry == null ? (Action<Exception, Context>)null : (Exception outcome, Context ctx) => onRetry(outcome));
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will retry indefinitely
        /// calling <paramref name="onRetry"/> on each retry with the raised exception and retry count.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy RetryForever(this PolicyBuilder policyBuilder, Action<Exception, int> onRetry)
        {
            return policyBuilder.RetryForever(onRetry: onRetry == null ? (Action<Exception, int, Context>)null : (outcome, i, ctx) => onRetry(outcome, i));
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will retry indefinitely
        /// calling <paramref name="onRetry"/> on each retry with the raised exception and context data.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy RetryForever(this PolicyBuilder policyBuilder, Action<Exception, Context> onRetry)
        {
                return new RetryPolicy(
                    policyBuilder,
                    onRetry: onRetry == null ? (Action<Exception, TimeSpan, int, Context>)null : (outcome, timespan, i, ctx) => onRetry(outcome, ctx)
                    );
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will retry indefinitely
        /// calling <paramref name="onRetry"/> on each retry with the raised exception, retry count and context data.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy RetryForever(this PolicyBuilder policyBuilder, Action<Exception, int, Context> onRetry)
        {
            return new RetryPolicy(
                policyBuilder,
                onRetry: onRetry == null ? (Action<Exception, TimeSpan, int, Context>)null : (outcome, timespan, i, ctx) => onRetry(outcome, i, ctx)
            );
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry <paramref name="retryCount"/> times.
        /// On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        /// the current retry number (1 for first retry, 2 for second etc).
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy WaitAndRetry(this PolicyBuilder policyBuilder, int retryCount, Func<int, TimeSpan> sleepDurationProvider)
        {
            return policyBuilder.WaitAndRetry(retryCount, sleepDurationProvider, onRetry: (Action<Exception, TimeSpan, int, Context>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the raised exception and the current sleep duration.
        /// On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        /// the current retry number (1 for first retry, 2 for second etc).
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">retryCount;Value must be greater than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy WaitAndRetry(this PolicyBuilder policyBuilder, int retryCount, Func<int, TimeSpan> sleepDurationProvider, Action<Exception, TimeSpan> onRetry)
        {
            return policyBuilder.WaitAndRetry(
                retryCount,
                sleepDurationProvider,
                onRetry: onRetry == null ? (Action<Exception, TimeSpan, int, Context>)null : (outcome, span, i, ctx) => onRetry(outcome, span)
                );
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the raised exception, current sleep duration and context data.
        /// On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        /// the current retry number (1 for first retry, 2 for second etc).
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">retryCount;Value must be greater than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy WaitAndRetry(this PolicyBuilder policyBuilder, int retryCount, Func<int, TimeSpan> sleepDurationProvider, Action<Exception, TimeSpan, Context> onRetry)
        {
            return policyBuilder.WaitAndRetry(
                retryCount, 
                sleepDurationProvider,
                onRetry: onRetry == null ? (Action<Exception, TimeSpan, int, Context>)null : (outcome, span, i, ctx) => onRetry(outcome, span, ctx)
                );
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the raised exception, current sleep duration, retry count, and context data.
        /// On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        /// the current retry number (1 for first retry, 2 for second etc).
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">retryCount;Value must be greater than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">
        /// timeSpanProvider
        /// </exception>
        public static ISyncRetryPolicy WaitAndRetry(this PolicyBuilder policyBuilder, int retryCount, Func<int, TimeSpan> sleepDurationProvider, Action<Exception, TimeSpan, int, Context> onRetry)
        {
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount), "Value must be greater than or equal to zero.");
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            var sleepDurations = Enumerable.Range(1, retryCount)
                                           .Select(sleepDurationProvider);

            return new RetryPolicy(
                policyBuilder,
                onRetry,
                retryCount, 
                sleepDurationsEnumerable: sleepDurations
                );
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry <paramref name="retryCount"/> times.
        /// On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        /// the current retry number (1 for first retry, 2 for second etc) and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy WaitAndRetry(this PolicyBuilder policyBuilder, int retryCount, Func<int, Context, TimeSpan> sleepDurationProvider)
        {
            return policyBuilder.WaitAndRetry(retryCount, sleepDurationProvider, onRetry: (Action<Exception, TimeSpan, int, Context>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the raised exception, current sleep duration and context data.
        /// On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        /// the current retry number (1 for first retry, 2 for second etc) and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">retryCount;Value must be greater than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy WaitAndRetry(this PolicyBuilder policyBuilder, int retryCount, Func<int, Context, TimeSpan> sleepDurationProvider, Action<Exception, TimeSpan, Context> onRetry)
        {
            return policyBuilder.WaitAndRetry(
                retryCount,
                sleepDurationProvider,
                onRetry: onRetry == null ? (Action<Exception, TimeSpan, int, Context>)null : (outcome, span, i, ctx) => onRetry(outcome, span, ctx)
                );
        }


        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the raised exception, current sleep duration, retry count, and context data.
        /// On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        /// the current retry number (1 for first retry, 2 for second etc) and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">retryCount;Value must be greater than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">
        /// timeSpanProvider
        /// </exception>
        public static ISyncRetryPolicy WaitAndRetry(this PolicyBuilder policyBuilder, int retryCount, Func<int, Context, TimeSpan> sleepDurationProvider, Action<Exception, TimeSpan, int, Context> onRetry)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));
            return policyBuilder.WaitAndRetry(
                retryCount,
                sleepDurationProvider: (i, outcome, ctx) => sleepDurationProvider(i, ctx),
                onRetry);
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the raised exception, current sleep duration, retry count, and context data.
        /// On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        /// the current retry number (1 for first retry, 2 for second etc), previous exception and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">retryCount;Value must be greater than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">
        /// timeSpanProvider
        /// </exception>
        public static ISyncRetryPolicy WaitAndRetry(this PolicyBuilder policyBuilder, int retryCount, Func<int, Exception, Context, TimeSpan> sleepDurationProvider, Action<Exception, TimeSpan, int, Context> onRetry)
        {
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount), "Value must be greater than or equal to zero.");
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

                return new RetryPolicy(
                    policyBuilder,
                    onRetry,
                    retryCount,
                    sleepDurationProvider: sleepDurationProvider
                    );
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry as many times as there are provided <paramref name="sleepDurations"/>
        /// On each retry, the duration to wait is the current <paramref name="sleepDurations"/> item.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy  WaitAndRetry(this PolicyBuilder policyBuilder, IEnumerable<TimeSpan> sleepDurations)
        {
            return policyBuilder.WaitAndRetry(sleepDurations, onRetry: (Action<Exception, TimeSpan>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry as many times as there are provided <paramref name="sleepDurations"/>
        /// calling <paramref name="onRetry"/> on each retry with the raised exception and the current sleep duration.
        /// On each retry, the duration to wait is the current <paramref name="sleepDurations"/> item.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurations</exception>
        public static ISyncRetryPolicy  WaitAndRetry(this PolicyBuilder policyBuilder, IEnumerable<TimeSpan> sleepDurations, Action<Exception, TimeSpan> onRetry)
        {
            return policyBuilder.WaitAndRetry(sleepDurations,  onRetry: onRetry == null ? (Action<Exception, TimeSpan, int, Context>)null : (outcome, span, i, ctx) => onRetry(outcome, span));
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry as many times as there are provided <paramref name="sleepDurations"/>
        /// calling <paramref name="onRetry"/> on each retry with the raised exception, current sleep duration and context data.
        /// On each retry, the duration to wait is the current <paramref name="sleepDurations"/> item.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurations</exception>
        public static ISyncRetryPolicy WaitAndRetry(this PolicyBuilder policyBuilder, IEnumerable<TimeSpan> sleepDurations, Action<Exception, TimeSpan, Context> onRetry)
        {
            return policyBuilder.WaitAndRetry(sleepDurations, onRetry: onRetry == null ? (Action<Exception, TimeSpan, int, Context>)null : (outcome, span, i, ctx) => onRetry(outcome, span, ctx));
        }


        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry as many times as there are provided <paramref name="sleepDurations"/>
        /// calling <paramref name="onRetry"/> on each retry with the raised exception, current sleep duration, retry count and context data.
        /// On each retry, the duration to wait is the current <paramref name="sleepDurations"/> item.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurations</exception>
        public static ISyncRetryPolicy WaitAndRetry(this PolicyBuilder policyBuilder, IEnumerable<TimeSpan> sleepDurations, Action<Exception, TimeSpan, int, Context> onRetry)
        {
            if (sleepDurations == null) throw new ArgumentNullException(nameof(sleepDurations));

            return new RetryPolicy(
                policyBuilder,
                onRetry,
                sleepDurationsEnumerable: sleepDurations
                );
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry indefinitely until the action succeeds.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc)
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy WaitAndRetryForever(this PolicyBuilder policyBuilder, Func<int, TimeSpan> sleepDurationProvider)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            return policyBuilder.WaitAndRetryForever(sleepDurationProvider, onRetry: (Action<Exception, TimeSpan>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry indefinitely until the action succeeds.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc) and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy WaitAndRetryForever(this PolicyBuilder policyBuilder, Func<int, Context, TimeSpan> sleepDurationProvider)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            return policyBuilder.WaitAndRetryForever(sleepDurationProvider, onRetry: (Action<Exception, TimeSpan, Context>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry indefinitely until the action succeeds, 
        /// calling <paramref name="onRetry"/> on each retry with the raised exception.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc)
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">A function providing the duration to wait before retrying.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy WaitAndRetryForever(this PolicyBuilder policyBuilder, Func<int, TimeSpan> sleepDurationProvider, Action<Exception, TimeSpan> onRetry)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            return policyBuilder.WaitAndRetryForever(
                (retryCount, context) => sleepDurationProvider(retryCount),
                onRetry: onRetry == null ? (Action<Exception, TimeSpan, Context>)null : (exception, timespan, context) => onRetry(exception, timespan)
            );
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry indefinitely until the action succeeds, 
        /// calling <paramref name="onRetry"/> on each retry with the raised exception and retry count.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc)
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">A function providing the duration to wait before retrying.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy WaitAndRetryForever(this PolicyBuilder policyBuilder, Func<int, TimeSpan> sleepDurationProvider, Action<Exception, int, TimeSpan> onRetry)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            return policyBuilder.WaitAndRetryForever(
                (retryCount, exception, context) => sleepDurationProvider(retryCount),
                onRetry: onRetry == null ? (Action<Exception, int, TimeSpan, Context>)null : (exception, i, timespan, context) => onRetry(exception, i, timespan)
            );
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry indefinitely until the action succeeds, 
        /// calling <paramref name="onRetry"/> on each retry with the raised exception and execution context.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc) and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">A function providing the duration to wait before retrying.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy WaitAndRetryForever(this PolicyBuilder policyBuilder, Func<int, Context, TimeSpan> sleepDurationProvider, Action<Exception, TimeSpan, Context> onRetry)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));
            return policyBuilder.WaitAndRetryForever(
                (i, outcome, ctx) => sleepDurationProvider(i, ctx),
                onRetry
            );
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry indefinitely until the action succeeds, 
        /// calling <paramref name="onRetry"/> on each retry with the raised exception, retry count and execution context.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc) and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">A function providing the duration to wait before retrying.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy WaitAndRetryForever(this PolicyBuilder policyBuilder, Func<int, Context, TimeSpan> sleepDurationProvider, Action<Exception, int, TimeSpan, Context> onRetry)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));
            return policyBuilder.WaitAndRetryForever(
                (i, outcome, ctx) => sleepDurationProvider(i, ctx),
                onRetry
            );
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry indefinitely until the action succeeds, 
        /// calling <paramref name="onRetry"/> on each retry with the raised exception and execution context.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc), previous execution and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">A function providing the duration to wait before retrying.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy WaitAndRetryForever(this PolicyBuilder policyBuilder, Func<int, Exception, Context, TimeSpan> sleepDurationProvider, Action<Exception, TimeSpan, Context> onRetry)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            return new RetryPolicy(
                policyBuilder,
                onRetry: onRetry == null ? (Action<Exception, TimeSpan, int, Context>)null : (outcome, timespan, i, ctx) => onRetry(outcome, timespan, ctx),
                sleepDurationProvider: sleepDurationProvider);
        }

        /// <summary>
        /// Builds a <see cref="Policy"/> that will wait and retry indefinitely until the action succeeds, 
        /// calling <paramref name="onRetry"/> on each retry with the raised exception, retry count and execution context.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc), previous exception and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">A function providing the duration to wait before retrying.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy WaitAndRetryForever(this PolicyBuilder policyBuilder, Func<int, Exception, Context, TimeSpan> sleepDurationProvider, Action<Exception, int, TimeSpan, Context> onRetry)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            return new RetryPolicy(
                policyBuilder, 
                onRetry: onRetry == null ? (Action<Exception, TimeSpan, int, Context>) null : (exception, timespan, i, ctx) => onRetry(exception, i, timespan, ctx),
                sleepDurationProvider: sleepDurationProvider
                );
        }
    }
}