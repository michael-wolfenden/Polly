﻿namespace Polly.Caching
{
    /// <summary>
    /// A cache policy that can be applied to asynchronous delegate executions.
    /// </summary>
    public interface IAsyncCachePolicy : IAsyncPolicy, ICachePolicy
    {
    }

    /// <summary>
    /// A cache policy that can be applied to asynchronous delegate executions.
    /// </summary>
    /// <typeparam name="TResult">The return type of delegates which may be executed through the policy.</typeparam>
    public interface IAsyncCachePolicy<TResult> : IAsyncPolicy<TResult>, ICachePolicy<TResult>
    {
    }
}
