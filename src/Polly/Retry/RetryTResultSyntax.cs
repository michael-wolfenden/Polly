﻿using System;
using System.Collections.Generic;
using Polly.Retry;
using System.Linq;

namespace Polly
{
    /// <summary>
    /// Fluent API for defining a Retry <see cref="Policy{TResult}"/>. 
    /// </summary>
    public static class RetryTResultSyntax
    {
        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will retry once.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy<TResult> Retry<TResult>(this PolicyBuilder<TResult> policyBuilder)
            => policyBuilder.Retry(1);

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will retry <paramref name="retryCount"/> times.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy<TResult> Retry<TResult>(this PolicyBuilder<TResult> policyBuilder, int retryCount)
        {
            return policyBuilder.Retry(retryCount, onRetry: (Action<DelegateResult<TResult>, int>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will retry once
        /// calling <paramref name="onRetry"/> on retry with the handled exception or result and retry count.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy<TResult> Retry<TResult>(this PolicyBuilder<TResult> policyBuilder, Action<DelegateResult<TResult>, int> onRetry)
            => policyBuilder.Retry(1, onRetry);

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result and retry count.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">retryCount;Value must be greater than or equal to zero.</exception>
        public static ISyncRetryPolicy<TResult> Retry<TResult>(this PolicyBuilder<TResult> policyBuilder, int retryCount, Action<DelegateResult<TResult>, int> onRetry)
        {
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount), "Value must be greater than or equal to zero.");

            return policyBuilder.Retry(retryCount, onRetry: onRetry == null ? (Action<DelegateResult<TResult>, int, Context>)null : (outcome, i, ctx) => onRetry(outcome, i));
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will retry once
        /// calling <paramref name="onRetry"/> on retry with the handled exception or result, retry count and context data.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy<TResult> Retry<TResult>(this PolicyBuilder<TResult> policyBuilder, Action<DelegateResult<TResult>, int, Context> onRetry)
            => policyBuilder.Retry(1, onRetry);

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result, retry count and context data.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">retryCount;Value must be greater than or equal to zero.</exception>
        public static ISyncRetryPolicy<TResult> Retry<TResult>(this PolicyBuilder<TResult> policyBuilder, int retryCount, Action<DelegateResult<TResult>, int, Context> onRetry)
        {
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount), "Value must be greater than or equal to zero.");

            return new RetryPolicy<TResult>(
                policyBuilder,
                onRetry: onRetry == null ? (Action<DelegateResult<TResult>, TimeSpan, int, Context>)null : (outcome, timespan, i, ctx) => onRetry(outcome, i, ctx),
                retryCount);
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will retry indefinitely until the action succeeds.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy<TResult> RetryForever<TResult>(this PolicyBuilder<TResult> policyBuilder)
        {
            return policyBuilder.RetryForever(onRetry: (Action<DelegateResult<TResult>>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will retry indefinitely
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy<TResult> RetryForever<TResult>(this PolicyBuilder<TResult> policyBuilder, Action<DelegateResult<TResult>> onRetry)
        {
            return policyBuilder.RetryForever(onRetry: onRetry == null ? (Action<DelegateResult<TResult>, Context>)null : (DelegateResult<TResult> outcome, Context ctx) => onRetry(outcome));
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will retry indefinitely
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result and retry count.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy<TResult> RetryForever<TResult>(this PolicyBuilder<TResult> policyBuilder, Action<DelegateResult<TResult>, int> onRetry)
        {
            return policyBuilder.RetryForever(onRetry: onRetry == null ? (Action<DelegateResult<TResult>, int, Context>)null : (outcome, i, context) => onRetry(outcome, i));
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will retry indefinitely
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result and context data.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy<TResult> RetryForever<TResult>(this PolicyBuilder<TResult> policyBuilder, Action<DelegateResult<TResult>, Context> onRetry)
        {
            return new RetryPolicy<TResult>(
                policyBuilder,
                onRetry: onRetry == null ? (Action<DelegateResult<TResult>, TimeSpan, int, Context>)null : (outcome, timespan, i, ctx) => onRetry(outcome, ctx)
                );
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will retry indefinitely
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result, retry count and context data.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy<TResult> RetryForever<TResult>(this PolicyBuilder<TResult> policyBuilder, Action<DelegateResult<TResult>, int, Context> onRetry)
        {
            return new RetryPolicy<TResult>(
                policyBuilder,
                onRetry: onRetry == null ? (Action<DelegateResult<TResult>, TimeSpan, int, Context>)null : (outcome, timespan, i, ctx) => onRetry(outcome, i, ctx)
            );
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry <paramref name="retryCount"/> times.
        /// On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        /// the current retry number (1 for first retry, 2 for second etc).
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy<TResult> WaitAndRetry<TResult>(this PolicyBuilder<TResult> policyBuilder, int retryCount, Func<int, TimeSpan> sleepDurationProvider)
        {
            return policyBuilder.WaitAndRetry(retryCount, sleepDurationProvider, onRetry: (Action<DelegateResult<TResult>, TimeSpan, int, Context>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result and the current sleep duration.
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
        public static ISyncRetryPolicy<TResult> WaitAndRetry<TResult>(this PolicyBuilder<TResult> policyBuilder, int retryCount, Func<int, TimeSpan> sleepDurationProvider, Action<DelegateResult<TResult>, TimeSpan> onRetry)
        {
            return policyBuilder.WaitAndRetry(
                retryCount,
                sleepDurationProvider,
                onRetry: onRetry == null ? (Action<DelegateResult<TResult>, TimeSpan, int, Context>)null : (outcome, span, i, ctx) => onRetry(outcome, span)
            );
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result, current sleep duration and context data.
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
        public static ISyncRetryPolicy<TResult> WaitAndRetry<TResult>(this PolicyBuilder<TResult> policyBuilder, int retryCount, Func<int, TimeSpan> sleepDurationProvider, Action<DelegateResult<TResult>, TimeSpan, Context> onRetry)
        {
            return policyBuilder.WaitAndRetry(
                retryCount, 
                sleepDurationProvider,
                onRetry: onRetry == null ? (Action<DelegateResult<TResult>, TimeSpan, int, Context>)null : (outcome, span, i, ctx) => onRetry(outcome, span, ctx)
                );
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result, current sleep duration, retry count, and context data.
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
        public static ISyncRetryPolicy<TResult> WaitAndRetry<TResult>(this PolicyBuilder<TResult> policyBuilder, int retryCount, Func<int, TimeSpan> sleepDurationProvider, Action<DelegateResult<TResult>, TimeSpan, int, Context> onRetry)
        {
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount), "Value must be greater than or equal to zero.");
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            var sleepDurations = Enumerable.Range(1, retryCount)
                                           .Select(sleepDurationProvider);

            return new RetryPolicy<TResult>(
                policyBuilder,
                onRetry,
                retryCount,
                sleepDurationsEnumerable: sleepDurations
            );
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry <paramref name="retryCount"/> times.
        /// On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        /// the current retry number (1 for first retry, 2 for second etc) and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy<TResult> WaitAndRetry<TResult>(this PolicyBuilder<TResult> policyBuilder, int retryCount, Func<int, Context, TimeSpan> sleepDurationProvider)
        {
            return policyBuilder.WaitAndRetry(retryCount, sleepDurationProvider, onRetry: (Action<DelegateResult<TResult>, TimeSpan, int, Context>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result, current sleep duration and context data.
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
        public static ISyncRetryPolicy<TResult> WaitAndRetry<TResult>(this PolicyBuilder<TResult> policyBuilder, int retryCount, Func<int, Context, TimeSpan> sleepDurationProvider, Action<DelegateResult<TResult>, TimeSpan, Context> onRetry)
        {
            return policyBuilder.WaitAndRetry(
                retryCount,
                sleepDurationProvider,
                onRetry: onRetry == null ? (Action<DelegateResult<TResult>, TimeSpan, int, Context>)null : (outcome, span, i, ctx) => onRetry(outcome, span, ctx)
                );
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result, current sleep duration, retry count, and context data.
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
        public static ISyncRetryPolicy<TResult> WaitAndRetry<TResult>(this PolicyBuilder<TResult> policyBuilder, int retryCount, Func<int, Context, TimeSpan> sleepDurationProvider, Action<DelegateResult<TResult>, TimeSpan, int, Context> onRetry)
            => policyBuilder.WaitAndRetry(
                retryCount,
                sleepDurationProvider: (i, outcome, ctx) => sleepDurationProvider(i, ctx),
                onRetry
            );

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry <paramref name="retryCount"/> times.
        /// On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        /// the current retry number (1 for first retry, 2 for second etc), previous execution result and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy<TResult> WaitAndRetry<TResult>(this PolicyBuilder<TResult> policyBuilder, int retryCount, Func<int, DelegateResult<TResult>, Context, TimeSpan> sleepDurationProvider)
        {
            return policyBuilder.WaitAndRetry(retryCount, sleepDurationProvider, onRetry: (Action<DelegateResult<TResult>, TimeSpan, int, Context>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result, current sleep duration and context data.
        /// On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        /// the current retry number (1 for first retry, 2 for second etc), previous execution result and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">retryCount;Value must be greater than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy<TResult> WaitAndRetry<TResult>(this PolicyBuilder<TResult> policyBuilder, int retryCount, Func<int, DelegateResult<TResult>, Context, TimeSpan> sleepDurationProvider, Action<DelegateResult<TResult>, TimeSpan, Context> onRetry)
        {
            return policyBuilder.WaitAndRetry(
                retryCount,
                sleepDurationProvider,
                onRetry: onRetry == null ? (Action<DelegateResult<TResult>, TimeSpan, int, Context>)null : (outcome, span, i, ctx) => onRetry(outcome, span, ctx)
                );
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry <paramref name="retryCount"/> times
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result, current sleep duration, retry count, and context data.
        /// On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider"/> with
        /// the current retry number (1 for first retry, 2 for second etc), previous execution result and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">retryCount;Value must be greater than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy<TResult> WaitAndRetry<TResult>(this PolicyBuilder<TResult> policyBuilder, int retryCount, Func<int, DelegateResult<TResult>, Context, TimeSpan> sleepDurationProvider, Action<DelegateResult<TResult>, TimeSpan, int, Context> onRetry)
        {
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount), "Value must be greater than or equal to zero.");
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            return new RetryPolicy<TResult>(
                policyBuilder,
                onRetry,
                retryCount,
                sleepDurationProvider: sleepDurationProvider
            );
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry as many times as there are provided <paramref name="sleepDurations"/>
        /// On each retry, the duration to wait is the current <paramref name="sleepDurations"/> item.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <returns>The policy instance.</returns>
        public static ISyncRetryPolicy<TResult> WaitAndRetry<TResult>(this PolicyBuilder<TResult> policyBuilder, IEnumerable<TimeSpan> sleepDurations)
        {
            return policyBuilder.WaitAndRetry(sleepDurations, onRetry: (Action<DelegateResult<TResult>, TimeSpan>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry as many times as there are provided <paramref name="sleepDurations"/>
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result and the current sleep duration.
        /// On each retry, the duration to wait is the current <paramref name="sleepDurations"/> item.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurations</exception>
        public static ISyncRetryPolicy<TResult> WaitAndRetry<TResult>(this PolicyBuilder<TResult> policyBuilder, IEnumerable<TimeSpan> sleepDurations, Action<DelegateResult<TResult>, TimeSpan> onRetry)
        {
            return policyBuilder.WaitAndRetry(sleepDurations, onRetry: onRetry == null ? (Action<DelegateResult<TResult>, TimeSpan, int, Context>)null : (outcome, span, i, ctx) => onRetry(outcome, span));
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry as many times as there are provided <paramref name="sleepDurations"/>
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result, current sleep duration and context data.
        /// On each retry, the duration to wait is the current <paramref name="sleepDurations"/> item.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurations</exception>
        public static ISyncRetryPolicy<TResult> WaitAndRetry<TResult>(this PolicyBuilder<TResult> policyBuilder, IEnumerable<TimeSpan> sleepDurations, Action<DelegateResult<TResult>, TimeSpan, Context> onRetry)
        {
            return policyBuilder.WaitAndRetry(sleepDurations, onRetry: onRetry == null ? (Action<DelegateResult<TResult>, TimeSpan, int, Context>)null : (outcome, span, i, ctx) => onRetry(outcome, span, ctx));
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry as many times as there are provided <paramref name="sleepDurations"/>
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result, current sleep duration, retry count and context data.
        /// On each retry, the duration to wait is the current <paramref name="sleepDurations"/> item.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurations">The sleep durations to wait for on each retry.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurations</exception>
        public static ISyncRetryPolicy<TResult> WaitAndRetry<TResult>(this PolicyBuilder<TResult> policyBuilder, IEnumerable<TimeSpan> sleepDurations, Action<DelegateResult<TResult>, TimeSpan, int, Context> onRetry)
        {
            if (sleepDurations == null) throw new ArgumentNullException(nameof(sleepDurations));

            return new RetryPolicy<TResult>(
                policyBuilder,
                onRetry,
                sleepDurationsEnumerable: sleepDurations
            );
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry indefinitely until the action succeeds.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc)
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy<TResult> WaitAndRetryForever<TResult>(this PolicyBuilder<TResult> policyBuilder, Func<int, TimeSpan> sleepDurationProvider)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            return policyBuilder.WaitAndRetryForever(sleepDurationProvider, onRetry: (Action<DelegateResult<TResult>, TimeSpan>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry indefinitely until the action succeeds.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc) and execution context
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy<TResult> WaitAndRetryForever<TResult>(this PolicyBuilder<TResult> policyBuilder, Func<int, Context, TimeSpan> sleepDurationProvider)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            return policyBuilder.WaitAndRetryForever(sleepDurationProvider, onRetry: (Action<DelegateResult<TResult>, TimeSpan, Context>) null);
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry indefinitely until the action succeeds, 
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc)
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">A function providing the duration to wait before retrying.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy<TResult> WaitAndRetryForever<TResult>(this PolicyBuilder<TResult> policyBuilder, Func<int, TimeSpan> sleepDurationProvider, Action<DelegateResult<TResult>, TimeSpan> onRetry)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            return policyBuilder.WaitAndRetryForever(
                sleepDurationProvider: (retryCount, context) => sleepDurationProvider(retryCount),
                onRetry: onRetry == null ? (Action<DelegateResult<TResult>, TimeSpan, Context>)null : (exception, timespan, context) => onRetry(exception, timespan)
            );
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry indefinitely until the action succeeds, 
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result and retry count.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc)
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">A function providing the duration to wait before retrying.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy<TResult> WaitAndRetryForever<TResult>(this PolicyBuilder<TResult> policyBuilder, Func<int, TimeSpan> sleepDurationProvider, Action<DelegateResult<TResult>, int, TimeSpan> onRetry)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            return policyBuilder.WaitAndRetryForever(
                sleepDurationProvider: (retryCount, outcome, context) => sleepDurationProvider(retryCount),
                onRetry: onRetry == null ? (Action<DelegateResult<TResult>, int, TimeSpan, Context>)null : (outcome, i, timespan, context) => onRetry(outcome, i, timespan)
            );
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry indefinitely until the action succeeds, 
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result and execution context.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc) and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">A function providing the duration to wait before retrying.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy<TResult> WaitAndRetryForever<TResult>(this PolicyBuilder<TResult> policyBuilder, Func<int, Context, TimeSpan> sleepDurationProvider, Action<DelegateResult<TResult>, TimeSpan, Context> onRetry)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));
            return policyBuilder.WaitAndRetryForever(
                sleepDurationProvider: (i, outcome, ctx) => sleepDurationProvider(i, ctx),
                onRetry
            );
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry indefinitely until the action succeeds, 
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result, retry count and execution context.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc) and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">A function providing the duration to wait before retrying.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy<TResult> WaitAndRetryForever<TResult>(this PolicyBuilder<TResult> policyBuilder, Func<int, Context, TimeSpan> sleepDurationProvider, Action<DelegateResult<TResult>, int, TimeSpan, Context> onRetry)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));
            return policyBuilder.WaitAndRetryForever(
                sleepDurationProvider: (i, outcome, ctx) => sleepDurationProvider(i, ctx),
                onRetry
            );
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry indefinitely until the action succeeds, 
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result and execution context.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc), previous execution result and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">A function providing the duration to wait before retrying.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy<TResult> WaitAndRetryForever<TResult>(this PolicyBuilder<TResult> policyBuilder, Func<int, DelegateResult<TResult>, Context, TimeSpan> sleepDurationProvider, Action<DelegateResult<TResult>, TimeSpan, Context> onRetry)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            return new RetryPolicy<TResult>(
                policyBuilder,
                onRetry: onRetry == null ? (Action<DelegateResult<TResult>, TimeSpan, int, Context>)null : (outcome, timespan, i, ctx) => onRetry(outcome, timespan, ctx),
                sleepDurationProvider: sleepDurationProvider);
        }

        /// <summary>
        /// Builds a <see cref="Policy{TResult}"/> that will wait and retry indefinitely until the action succeeds, 
        /// calling <paramref name="onRetry"/> on each retry with the handled exception or result, retry count and execution context.
        ///     On each retry, the duration to wait is calculated by calling <paramref name="sleepDurationProvider" /> with 
        ///     the current retry number (1 for first retry, 2 for second etc), previous execution result and execution context.
        /// </summary>
        /// <param name="policyBuilder">The policy builder.</param>
        /// <param name="sleepDurationProvider">A function providing the duration to wait before retrying.</param>
        /// <param name="onRetry">The action to call on each retry.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">sleepDurationProvider</exception>
        public static ISyncRetryPolicy<TResult> WaitAndRetryForever<TResult>(this PolicyBuilder<TResult> policyBuilder, Func<int, DelegateResult<TResult>, Context, TimeSpan> sleepDurationProvider, Action<DelegateResult<TResult>, int, TimeSpan, Context> onRetry)
        {
            if (sleepDurationProvider == null) throw new ArgumentNullException(nameof(sleepDurationProvider));

            return new RetryPolicy<TResult>(
                policyBuilder,
                onRetry: onRetry == null ? (Action<DelegateResult<TResult>, TimeSpan, int, Context>)null : (exception, timespan, i, ctx) => onRetry(exception, i, timespan, ctx),
                sleepDurationProvider: sleepDurationProvider
                );
        }
    }
}