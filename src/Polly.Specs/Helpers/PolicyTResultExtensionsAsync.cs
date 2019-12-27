﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Scenario = Polly.Specs.Helpers.PolicyTResultExtensionsAsync.ResultAndOrCancellationScenario;

namespace Polly.Specs.Helpers
{
    public static class PolicyTResultExtensionsAsync
    {
        public static Task<TResult> RaiseResultSequenceAsync<TResult>(this IAsyncPolicy<TResult> policy, params TResult[] resultsToRaise)
        {
            return policy.RaiseResultSequenceAsync(resultsToRaise.ToList());
        }

        public static Task<TResult> RaiseResultSequenceAsync<TResult>(this IAsyncPolicy<TResult> policy, IEnumerable<TResult> resultsToRaise)
        {
            return policy.RaiseResultSequenceAsync(default, resultsToRaise);
        }

        public static Task<TResult> RaiseResultSequenceAsync<TResult>(this IAsyncPolicy<TResult> policy,
            CancellationToken cancellationToken, IEnumerable<TResult> resultsToRaise)
        {
            return policy.RaiseResultSequenceAsync(new Dictionary<string, object>(0), cancellationToken, resultsToRaise);
        }

        public static Task<TResult> RaiseResultSequenceAsync<TResult>(this IAsyncPolicy<TResult> policy,
            IDictionary<string, object> contextData,
            params TResult[] resultsToRaise)
        {
            return policy.RaiseResultSequenceAsync(contextData, CancellationToken.None, resultsToRaise.ToList());
        }

        public static Task<TResult> RaiseResultSequenceAsync<TResult>(this IAsyncPolicy<TResult> policy,
            IDictionary<string, object> contextData, CancellationToken cancellationToken,
            IEnumerable<TResult> resultsToRaise)
        {
            using (var enumerator = resultsToRaise.GetEnumerator())
            {
                return policy.ExecuteAsync((ctx, ct) =>
                {
                    if (!enumerator.MoveNext())
                    {
                        throw new ArgumentOutOfRangeException(nameof(resultsToRaise), $"Not enough {typeof(TResult).Name}  values in {nameof(resultsToRaise)}.");
                    }

                    return Task.FromResult(enumerator.Current);
                }, contextData, cancellationToken);
            }
        }

        public static Task<TResult> RaiseResultAndOrExceptionSequenceAsync<TResult>(this IAsyncPolicy<TResult> policy, params object[] resultsOrExceptionsToRaise)
        {
            return policy.RaiseResultAndOrExceptionSequenceAsync(resultsOrExceptionsToRaise.ToList());
        }

        public static Task<TResult> RaiseResultAndOrExceptionSequenceAsync<TResult>(this IAsyncPolicy<TResult> policy,
            IEnumerable<object> resultsOrExceptionsToRaise)
        {
            return policy.RaiseResultAndOrExceptionSequenceAsync(CancellationToken.None, resultsOrExceptionsToRaise);
        }

        public static async Task<TResult> RaiseResultAndOrExceptionSequenceAsync<TResult>(this IAsyncPolicy<TResult> policy,
            CancellationToken cancellationToken, IEnumerable<object> resultsOrExceptionsToRaise)
        {
            using (var enumerator = resultsOrExceptionsToRaise.GetEnumerator())
            {
                return await policy.ExecuteAsync(ct =>
                {
                    if (!enumerator.MoveNext())
                    {
                        throw new ArgumentOutOfRangeException(nameof(resultsOrExceptionsToRaise), $"Not enough {typeof(TResult).Name} values in {nameof(resultsOrExceptionsToRaise)}.");
                    }

                    object current = enumerator.Current;
                    if (current is Exception)
                    {
                        throw (Exception) current;
                    }
                    else if (current is TResult)
                    {
                        return Task.FromResult((TResult) current);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(nameof(resultsOrExceptionsToRaise),
                            $"Value is not either an {typeof(Exception).Name} or {typeof(TResult).Name}.");
                    }
                }, cancellationToken);
            }
        }

        public class ResultAndOrCancellationScenario
        {
            public int? AttemptDuringWhichToCancel = null;

            public bool ActionObservesCancellation = true;
        }

        public static Task<TResult> RaiseResultSequenceAndOrCancellationAsync<TResult>(this IAsyncPolicy<TResult> policy,
            Scenario scenario, CancellationTokenSource cancellationTokenSource, Action onExecute,
            params TResult[] resultsToRaise)
        {
            return policy.RaiseResultSequenceAndOrCancellationAsync(scenario, cancellationTokenSource, onExecute,
                resultsToRaise.ToList());
        }

        public static async Task<TResult> RaiseResultSequenceAndOrCancellationAsync<TResult>(
            this IAsyncPolicy<TResult> policy, Scenario scenario, CancellationTokenSource cancellationTokenSource,
            Action onExecute, IEnumerable<TResult> resultsToRaise)
        {
            int counter = 0;

            CancellationToken cancellationToken = cancellationTokenSource.Token;

            using (var enumerator = resultsToRaise.GetEnumerator())
            {
                return await policy.ExecuteAsync(ct =>
                {
                    onExecute();

                    counter++;

                    if (!enumerator.MoveNext())
                    {
                        throw new ArgumentOutOfRangeException(nameof(resultsToRaise), $"Not enough {typeof(TResult).Name}  values in {nameof(resultsToRaise)}.");
                    }

                    if (scenario.AttemptDuringWhichToCancel.HasValue && counter >= scenario.AttemptDuringWhichToCancel.Value)
                    {
                        cancellationTokenSource.Cancel();
                    }

                    if (scenario.ActionObservesCancellation)
                    {
                        ct.ThrowIfCancellationRequested();
                    }

                    return Task.FromResult(enumerator.Current);
                }, cancellationToken);
            }
        }
            
        public static Task<PolicyResult<TResult>> RaiseResultSequenceOnExecuteAndCaptureAsync<TResult>(this IAsyncPolicy<TResult> policy, IDictionary<string, object> contextData, params TResult[] resultsToRaise)
        {
            return policy.RaiseResultSequenceOnExecuteAndCaptureAsync(contextData, resultsToRaise.ToList());
        }

        public static Task<PolicyResult<TResult>> RaiseResultSequenceOnExecuteAndCaptureAsync<TResult>(
            this IAsyncPolicy<TResult> policy, IDictionary<string, object> contextData,
            IEnumerable<TResult> resultsToRaise)
        {
            using (var enumerator = resultsToRaise.GetEnumerator())
            {
                return policy.ExecuteAndCaptureAsync(ctx =>
                {
                    if (!enumerator.MoveNext())
                    {
                        throw new ArgumentOutOfRangeException(nameof(resultsToRaise),
                            "Not enough TResult values in resultsToRaise.");
                    }

                    return Task.FromResult(enumerator.Current);
                }, contextData);
            }
        }
    }
}
