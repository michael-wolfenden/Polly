﻿using Polly.Caching;
using System;

namespace Polly
{
    public partial class Policy
    {
        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}"/> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider"/> holds a value for the cache key specified by <see cref="M:Context.OperationKey"/>.
        /// If the <paramref name="cacheProvider"/> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider"/> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider"/>, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttl">Duration (ttl) for which to cache values.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider cacheProvider, TimeSpan ttl, Action<Context, string, Exception> onCacheError = null)
        {
            if (cacheProvider == null) throw new ArgumentNullException(nameof(cacheProvider));

            return Cache<TResult>(cacheProvider.For<TResult>(), new RelativeTtl(ttl), DefaultCacheKeyStrategy.Instance.GetCacheKey, onCacheError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}"/> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider"/> holds a value for the cache key specified by <see cref="M:Context.OperationKey"/>.
        /// If the <paramref name="cacheProvider"/> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider"/> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider"/>, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider cacheProvider, ITtlStrategy ttlStrategy, Action<Context, string, Exception> onCacheError = null)
        {
            if (cacheProvider == null) throw new ArgumentNullException(nameof(cacheProvider));

            return Cache<TResult>(cacheProvider.For<TResult>(), ttlStrategy, DefaultCacheKeyStrategy.Instance.GetCacheKey, onCacheError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttl">Duration (ttl) for which to cache values.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider cacheProvider, TimeSpan ttl, ICacheKeyStrategy cacheKeyStrategy, Action<Context, string, Exception> onCacheError = null)
        {
            if (cacheProvider == null) throw new ArgumentNullException(nameof(cacheProvider));

            return Cache<TResult>(cacheProvider.For<TResult>(), new RelativeTtl(ttl), cacheKeyStrategy.GetCacheKey, onCacheError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider cacheProvider, ITtlStrategy ttlStrategy, ICacheKeyStrategy cacheKeyStrategy, Action<Context, string, Exception> onCacheError = null)
        {
            if (cacheProvider == null) throw new ArgumentNullException(nameof(cacheProvider));

            return Cache<TResult>(cacheProvider.For<TResult>(), ttlStrategy, cacheKeyStrategy.GetCacheKey, onCacheError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttl">Duration (ttl) for which to cache values.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider cacheProvider, TimeSpan ttl, Func<Context, string> cacheKeyStrategy, Action<Context, string, Exception> onCacheError = null)
        {
            if (cacheProvider == null) throw new ArgumentNullException(nameof(cacheProvider));
            if (cacheKeyStrategy == null) throw new ArgumentNullException(nameof(cacheKeyStrategy));

            return Cache<TResult>(cacheProvider.For<TResult>(), new RelativeTtl(ttl), cacheKeyStrategy, onCacheError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider cacheProvider, ITtlStrategy ttlStrategy, Func<Context, string> cacheKeyStrategy, Action<Context, string, Exception> onCacheError = null)
        {
            if (cacheProvider == null) throw new ArgumentNullException(nameof(cacheProvider));

            return Cache<TResult>(cacheProvider.For<TResult>(), ttlStrategy, cacheKeyStrategy, onCacheError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttl">Duration (ttl) for which to cache values.</param>
        /// <param name="onCacheGet">Delegate to call on a cache hit, when value is returned from cache.</param>
        /// <param name="onCacheMiss">Delegate to call on a cache miss.</param>
        /// <param name="onCachePut">Delegate to call on cache put.</param>
        /// <param name="onCacheGetError">Delegate to call if an exception is thrown when attempting to get a value from the cache, passing the execution context, the cache key, and the exception.</param>
        /// <param name="onCachePutError">Delegate to call if an exception is thrown when attempting to put a value in the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">onCacheGet</exception>
        /// <exception cref="ArgumentNullException">onCacheMiss</exception>
        /// <exception cref="ArgumentNullException">onCachePut</exception>
        /// <exception cref="ArgumentNullException">onCacheGetError</exception>
        /// <exception cref="ArgumentNullException">onCachePutError</exception>
        public static CachePolicy<TResult> Cache<TResult>(
            ISyncCacheProvider cacheProvider,
            TimeSpan ttl,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
        {
            if (cacheProvider == null) throw new ArgumentNullException(nameof(cacheProvider));

            return Cache<TResult>(cacheProvider.For<TResult>(), new RelativeTtl(ttl), DefaultCacheKeyStrategy.Instance.GetCacheKey, onCacheGet, onCacheMiss, onCachePut, onCacheGetError, onCachePutError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="onCacheGet">Delegate to call on a cache hit, when value is returned from cache.</param>
        /// <param name="onCacheMiss">Delegate to call on a cache miss.</param>
        /// <param name="onCachePut">Delegate to call on cache put.</param>
        /// <param name="onCacheGetError">Delegate to call if an exception is thrown when attempting to get a value from the cache, passing the execution context, the cache key, and the exception.</param>
        /// <param name="onCachePutError">Delegate to call if an exception is thrown when attempting to put a value in the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">onCacheGet</exception>
        /// <exception cref="ArgumentNullException">onCacheMiss</exception>
        /// <exception cref="ArgumentNullException">onCachePut</exception>
        /// <exception cref="ArgumentNullException">onCacheGetError</exception>
        /// <exception cref="ArgumentNullException">onCachePutError</exception>
        public static CachePolicy<TResult> Cache<TResult>(
            ISyncCacheProvider cacheProvider,
            ITtlStrategy ttlStrategy,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
        {
            if (cacheProvider == null) throw new ArgumentNullException(nameof(cacheProvider));

            return Cache<TResult>(cacheProvider.For<TResult>(), ttlStrategy, DefaultCacheKeyStrategy.Instance.GetCacheKey, onCacheGet, onCacheMiss, onCachePut, onCacheGetError, onCachePutError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttl">Duration (ttl) for which to cache values.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheGet">Delegate to call on a cache hit, when value is returned from cache.</param>
        /// <param name="onCacheMiss">Delegate to call on a cache miss.</param>
        /// <param name="onCachePut">Delegate to call on cache put.</param>
        /// <param name="onCacheGetError">Delegate to call if an exception is thrown when attempting to get a value from the cache, passing the execution context, the cache key, and the exception.</param>
        /// <param name="onCachePutError">Delegate to call if an exception is thrown when attempting to put a value in the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        /// <exception cref="ArgumentNullException">onCacheGet</exception>
        /// <exception cref="ArgumentNullException">onCacheMiss</exception>
        /// <exception cref="ArgumentNullException">onCachePut</exception>
        /// <exception cref="ArgumentNullException">onCacheGetError</exception>
        /// <exception cref="ArgumentNullException">onCachePutError</exception>
        public static CachePolicy<TResult> Cache<TResult>(
            ISyncCacheProvider cacheProvider,
            TimeSpan ttl,
            ICacheKeyStrategy cacheKeyStrategy,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
        {
            if (cacheProvider == null) throw new ArgumentNullException(nameof(cacheProvider));

            return Cache<TResult>(cacheProvider.For<TResult>(), new RelativeTtl(ttl), cacheKeyStrategy.GetCacheKey, onCacheGet, onCacheMiss, onCachePut, onCacheGetError, onCachePutError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheGet">Delegate to call on a cache hit, when value is returned from cache.</param>
        /// <param name="onCacheMiss">Delegate to call on a cache miss.</param>
        /// <param name="onCachePut">Delegate to call on cache put.</param>
        /// <param name="onCacheGetError">Delegate to call if an exception is thrown when attempting to get a value from the cache, passing the execution context, the cache key, and the exception.</param>
        /// <param name="onCachePutError">Delegate to call if an exception is thrown when attempting to put a value in the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        /// <exception cref="ArgumentNullException">onCacheGet</exception>
        /// <exception cref="ArgumentNullException">onCacheMiss</exception>
        /// <exception cref="ArgumentNullException">onCachePut</exception>
        /// <exception cref="ArgumentNullException">onCacheGetError</exception>
        /// <exception cref="ArgumentNullException">onCachePutError</exception>
        public static CachePolicy<TResult> Cache<TResult>(
            ISyncCacheProvider cacheProvider,
            ITtlStrategy ttlStrategy,
            ICacheKeyStrategy cacheKeyStrategy,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
        {
            if (cacheProvider == null) throw new ArgumentNullException(nameof(cacheProvider));

            return Cache<TResult>(cacheProvider.For<TResult>(), ttlStrategy, cacheKeyStrategy.GetCacheKey, onCacheGet, onCacheMiss, onCachePut, onCacheGetError, onCachePutError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttl">Duration (ttl) for which to cache values.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheGet">Delegate to call on a cache hit, when value is returned from cache.</param>
        /// <param name="onCacheMiss">Delegate to call on a cache miss.</param>
        /// <param name="onCachePut">Delegate to call on cache put.</param>
        /// <param name="onCacheGetError">Delegate to call if an exception is thrown when attempting to get a value from the cache, passing the execution context, the cache key, and the exception.</param>
        /// <param name="onCachePutError">Delegate to call if an exception is thrown when attempting to put a value in the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        /// <exception cref="ArgumentNullException">onCacheGet</exception>
        /// <exception cref="ArgumentNullException">onCacheMiss</exception>
        /// <exception cref="ArgumentNullException">onCachePut</exception>
        /// <exception cref="ArgumentNullException">onCacheGetError</exception>
        /// <exception cref="ArgumentNullException">onCachePutError</exception>
        public static CachePolicy<TResult> Cache<TResult>(
            ISyncCacheProvider cacheProvider,
            TimeSpan ttl,
            Func<Context, string> cacheKeyStrategy,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
        {
            if (cacheProvider == null) throw new ArgumentNullException(nameof(cacheProvider));

            return Cache<TResult>(cacheProvider.For<TResult>(), new RelativeTtl(ttl), cacheKeyStrategy, onCacheGet, onCacheMiss, onCachePut, onCacheGetError, onCachePutError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheGet">Delegate to call on a cache hit, when value is returned from cache.</param>
        /// <param name="onCacheMiss">Delegate to call on a cache miss.</param>
        /// <param name="onCachePut">Delegate to call on cache put.</param>
        /// <param name="onCacheGetError">Delegate to call if an exception is thrown when attempting to get a value from the cache, passing the execution context, the cache key, and the exception.</param>
        /// <param name="onCachePutError">Delegate to call if an exception is thrown when attempting to put a value in the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        /// <exception cref="ArgumentNullException">onCacheGet</exception>
        /// <exception cref="ArgumentNullException">onCacheMiss</exception>
        /// <exception cref="ArgumentNullException">onCachePut</exception>
        /// <exception cref="ArgumentNullException">onCacheGetError</exception>
        /// <exception cref="ArgumentNullException">onCachePutError</exception>
        public static CachePolicy<TResult> Cache<TResult>(
            ISyncCacheProvider cacheProvider,
            ITtlStrategy ttlStrategy, 
            Func<Context, string> cacheKeyStrategy,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
        {
            if (cacheProvider == null) throw new ArgumentNullException(nameof(cacheProvider));

            return Cache<TResult>(cacheProvider.For<TResult>(), ttlStrategy, cacheKeyStrategy, onCacheGet, onCacheMiss, onCachePut, onCacheGetError, onCachePutError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}"/> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider"/> holds a value for the cache key specified by <see cref="M:Context.OperationKey"/>.
        /// If the <paramref name="cacheProvider"/> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider"/> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider"/>, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttl">Duration (ttl) for which to cache values.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider<TResult> cacheProvider, TimeSpan ttl, Action<Context, string, Exception> onCacheError = null)
            => Cache<TResult>(cacheProvider, new RelativeTtl(ttl), DefaultCacheKeyStrategy.Instance.GetCacheKey, onCacheError);

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}"/> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider"/> holds a value for the cache key specified by <see cref="M:Context.OperationKey"/>.
        /// If the <paramref name="cacheProvider"/> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider"/> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider"/>, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider<TResult> cacheProvider, ITtlStrategy ttlStrategy, Action<Context, string, Exception> onCacheError = null)
            => Cache<TResult>(cacheProvider, ttlStrategy, DefaultCacheKeyStrategy.Instance.GetCacheKey, onCacheError);

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}"/> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider"/> holds a value for the cache key specified by <see cref="M:Context.OperationKey"/>.
        /// If the <paramref name="cacheProvider"/> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider"/> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider"/>, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider<TResult> cacheProvider, ITtlStrategy<TResult> ttlStrategy, Action<Context, string, Exception> onCacheError = null)
            => Cache<TResult>(cacheProvider, ttlStrategy, DefaultCacheKeyStrategy.Instance.GetCacheKey, onCacheError);

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttl">Duration (ttl) for which to cache values.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider<TResult> cacheProvider, TimeSpan ttl, ICacheKeyStrategy cacheKeyStrategy, Action<Context, string, Exception> onCacheError = null)
            => Cache<TResult>(cacheProvider, new RelativeTtl(ttl), cacheKeyStrategy.GetCacheKey, onCacheError);

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider<TResult> cacheProvider, ITtlStrategy ttlStrategy, ICacheKeyStrategy cacheKeyStrategy, Action<Context, string, Exception> onCacheError = null)
        {
            return Cache(cacheProvider, ttlStrategy.For<TResult>(), cacheKeyStrategy, onCacheError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider<TResult> cacheProvider, ITtlStrategy<TResult> ttlStrategy, ICacheKeyStrategy cacheKeyStrategy, Action<Context, string, Exception> onCacheError = null)
        {
            if (cacheKeyStrategy == null) throw new ArgumentNullException(nameof(cacheKeyStrategy));

            return Cache(cacheProvider, ttlStrategy, cacheKeyStrategy.GetCacheKey, onCacheError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttl">Duration (ttl) for which to cache values.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider<TResult> cacheProvider, TimeSpan ttl, Func<Context, string> cacheKeyStrategy, Action<Context, string, Exception> onCacheError = null)
            => Cache<TResult>(cacheProvider, new RelativeTtl(ttl), cacheKeyStrategy, onCacheError);

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider<TResult> cacheProvider, ITtlStrategy ttlStrategy, Func<Context, string> cacheKeyStrategy, Action<Context, string, Exception> onCacheError = null)
        {
            return Cache<TResult>(cacheProvider, ttlStrategy.For<TResult>(), cacheKeyStrategy, onCacheError);
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheError">Delegate to call if an exception is thrown when attempting to get a value from or put a value into the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider<TResult> cacheProvider, ITtlStrategy<TResult> ttlStrategy, Func<Context, string> cacheKeyStrategy, Action<Context, string, Exception> onCacheError = null)
        {
            return Cache(cacheProvider, ttlStrategy, options =>
            {
                options.CacheKeyStrategy = cacheKeyStrategy;
                options.OnCacheGetError = onCacheError;
                options.OnCachePutError = onCacheError;
            });
        }

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttl">Duration (ttl) for which to cache values.</param>
        /// <param name="onCacheGet">Delegate to call on a cache hit, when value is returned from cache.</param>
        /// <param name="onCacheMiss">Delegate to call on a cache miss.</param>
        /// <param name="onCachePut">Delegate to call on cache put.</param>
        /// <param name="onCacheGetError">Delegate to call if an exception is thrown when attempting to get a value from the cache, passing the execution context, the cache key, and the exception.</param>
        /// <param name="onCachePutError">Delegate to call if an exception is thrown when attempting to put a value in the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">onCacheGet</exception>
        /// <exception cref="ArgumentNullException">onCacheMiss</exception>
        /// <exception cref="ArgumentNullException">onCachePut</exception>
        /// <exception cref="ArgumentNullException">onCacheGetError</exception>
        /// <exception cref="ArgumentNullException">onCachePutError</exception>
        public static CachePolicy<TResult> Cache<TResult>(
            ISyncCacheProvider<TResult> cacheProvider,
            TimeSpan ttl,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
            => Cache(cacheProvider, new RelativeTtl(ttl).For<TResult>(), onCacheGet, onCacheMiss,
                onCachePut, onCacheGetError, onCachePutError);

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="onCacheGet">Delegate to call on a cache hit, when value is returned from cache.</param>
        /// <param name="onCacheMiss">Delegate to call on a cache miss.</param>
        /// <param name="onCachePut">Delegate to call on cache put.</param>
        /// <param name="onCacheGetError">Delegate to call if an exception is thrown when attempting to get a value from the cache, passing the execution context, the cache key, and the exception.</param>
        /// <param name="onCachePutError">Delegate to call if an exception is thrown when attempting to put a value in the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">onCacheGet</exception>
        /// <exception cref="ArgumentNullException">onCacheMiss</exception>
        /// <exception cref="ArgumentNullException">onCachePut</exception>
        /// <exception cref="ArgumentNullException">onCacheGetError</exception>
        /// <exception cref="ArgumentNullException">onCachePutError</exception>
        public static CachePolicy<TResult> Cache<TResult>(
            ISyncCacheProvider<TResult> cacheProvider,
            ITtlStrategy ttlStrategy,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
            => Cache<TResult>(cacheProvider, ttlStrategy, DefaultCacheKeyStrategy.Instance.GetCacheKey, onCacheGet, onCacheMiss, onCachePut, onCacheGetError, onCachePutError);

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="onCacheGet">Delegate to call on a cache hit, when value is returned from cache.</param>
        /// <param name="onCacheMiss">Delegate to call on a cache miss.</param>
        /// <param name="onCachePut">Delegate to call on cache put.</param>
        /// <param name="onCacheGetError">Delegate to call if an exception is thrown when attempting to get a value from the cache, passing the execution context, the cache key, and the exception.</param>
        /// <param name="onCachePutError">Delegate to call if an exception is thrown when attempting to put a value in the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">onCacheGet</exception>
        /// <exception cref="ArgumentNullException">onCacheMiss</exception>
        /// <exception cref="ArgumentNullException">onCachePut</exception>
        /// <exception cref="ArgumentNullException">onCacheGetError</exception>
        /// <exception cref="ArgumentNullException">onCachePutError</exception>
        public static CachePolicy<TResult> Cache<TResult>(
            ISyncCacheProvider<TResult> cacheProvider,
            ITtlStrategy<TResult> ttlStrategy,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
            => Cache(cacheProvider, ttlStrategy, options =>
            {
                options.OnCacheGet = onCacheGet;
                options.OnCacheMiss = onCacheMiss;
                options.OnCachePut = onCachePut;
                options.OnCacheGetError = onCacheGetError;
                options.OnCachePutError = onCachePutError;
            });

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttl">Duration (ttl) for which to cache values.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheGet">Delegate to call on a cache hit, when value is returned from cache.</param>
        /// <param name="onCacheMiss">Delegate to call on a cache miss.</param>
        /// <param name="onCachePut">Delegate to call on cache put.</param>
        /// <param name="onCacheGetError">Delegate to call if an exception is thrown when attempting to get a value from the cache, passing the execution context, the cache key, and the exception.</param>
        /// <param name="onCachePutError">Delegate to call if an exception is thrown when attempting to put a value in the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        /// <exception cref="ArgumentNullException">onCacheGet</exception>
        /// <exception cref="ArgumentNullException">onCacheMiss</exception>
        /// <exception cref="ArgumentNullException">onCachePut</exception>
        /// <exception cref="ArgumentNullException">onCacheGetError</exception>
        /// <exception cref="ArgumentNullException">onCachePutError</exception>
        public static CachePolicy<TResult> Cache<TResult>(
            ISyncCacheProvider<TResult> cacheProvider,
            TimeSpan ttl,
            ICacheKeyStrategy cacheKeyStrategy,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
            => Cache<TResult>(cacheProvider, new RelativeTtl(ttl), cacheKeyStrategy.GetCacheKey, onCacheGet, onCacheMiss,
                onCachePut, onCacheGetError, onCachePutError);

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheGet">Delegate to call on a cache hit, when value is returned from cache.</param>
        /// <param name="onCacheMiss">Delegate to call on a cache miss.</param>
        /// <param name="onCachePut">Delegate to call on cache put.</param>
        /// <param name="onCacheGetError">Delegate to call if an exception is thrown when attempting to get a value from the cache, passing the execution context, the cache key, and the exception.</param>
        /// <param name="onCachePutError">Delegate to call if an exception is thrown when attempting to put a value in the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        /// <exception cref="ArgumentNullException">onCacheGet</exception>
        /// <exception cref="ArgumentNullException">onCacheMiss</exception>
        /// <exception cref="ArgumentNullException">onCachePut</exception>
        /// <exception cref="ArgumentNullException">onCacheGetError</exception>
        /// <exception cref="ArgumentNullException">onCachePutError</exception>
        public static CachePolicy<TResult> Cache<TResult>(
            ISyncCacheProvider<TResult> cacheProvider,
            ITtlStrategy ttlStrategy,
            ICacheKeyStrategy cacheKeyStrategy,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
            => Cache(cacheProvider, ttlStrategy, cacheKeyStrategy.GetCacheKey, onCacheGet, onCacheMiss, onCachePut, onCacheGetError, onCachePutError);

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheGet">Delegate to call on a cache hit, when value is returned from cache.</param>
        /// <param name="onCacheMiss">Delegate to call on a cache miss.</param>
        /// <param name="onCachePut">Delegate to call on cache put.</param>
        /// <param name="onCacheGetError">Delegate to call if an exception is thrown when attempting to get a value from the cache, passing the execution context, the cache key, and the exception.</param>
        /// <param name="onCachePutError">Delegate to call if an exception is thrown when attempting to put a value in the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        /// <exception cref="ArgumentNullException">onCacheGet</exception>
        /// <exception cref="ArgumentNullException">onCacheMiss</exception>
        /// <exception cref="ArgumentNullException">onCachePut</exception>
        /// <exception cref="ArgumentNullException">onCacheGetError</exception>
        /// <exception cref="ArgumentNullException">onCachePutError</exception>
        public static CachePolicy<TResult> Cache<TResult>(
            ISyncCacheProvider<TResult> cacheProvider,
            ITtlStrategy<TResult> ttlStrategy,
            ICacheKeyStrategy cacheKeyStrategy,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
            => Cache(cacheProvider, ttlStrategy, options =>
            {
                options.CacheKeyStrategy = cacheKeyStrategy.GetCacheKey;
                options.OnCacheGet = onCacheGet;
                options.OnCacheMiss = onCacheMiss;
                options.OnCachePut = onCachePut;
                options.OnCacheGetError = onCacheGetError;
                options.OnCachePutError = onCachePutError;
            });

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttl">Duration (ttl) for which to cache values.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheGet">Delegate to call on a cache hit, when value is returned from cache.</param>
        /// <param name="onCacheMiss">Delegate to call on a cache miss.</param>
        /// <param name="onCachePut">Delegate to call on cache put.</param>
        /// <param name="onCacheGetError">Delegate to call if an exception is thrown when attempting to get a value from the cache, passing the execution context, the cache key, and the exception.</param>
        /// <param name="onCachePutError">Delegate to call if an exception is thrown when attempting to put a value in the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        /// <exception cref="ArgumentNullException">onCacheGet</exception>
        /// <exception cref="ArgumentNullException">onCacheMiss</exception>
        /// <exception cref="ArgumentNullException">onCachePut</exception>
        /// <exception cref="ArgumentNullException">onCacheGetError</exception>
        /// <exception cref="ArgumentNullException">onCachePutError</exception>
        public static CachePolicy<TResult> Cache<TResult>(
            ISyncCacheProvider<TResult> cacheProvider,
            TimeSpan ttl,
            Func<Context, string> cacheKeyStrategy,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
           => Cache(cacheProvider, new RelativeTtl(ttl), cacheKeyStrategy, onCacheGet, onCacheMiss,
                onCachePut, onCacheGetError, onCachePutError);

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the <paramref name="cacheKeyStrategy"/> to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate.  If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying ttl for values to be cached.</param>
        /// <param name="cacheKeyStrategy">The cache key strategy.</param>
        /// <param name="onCacheGet">Delegate to call on a cache hit, when value is returned from cache.</param>
        /// <param name="onCacheMiss">Delegate to call on a cache miss.</param>
        /// <param name="onCachePut">Delegate to call on cache put.</param>
        /// <param name="onCacheGetError">Delegate to call if an exception is thrown when attempting to get a value from the cache, passing the execution context, the cache key, and the exception.</param>
        /// <param name="onCachePutError">Delegate to call if an exception is thrown when attempting to put a value in the cache, passing the execution context, the cache key, and the exception.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">cacheKeyStrategy</exception>
        /// <exception cref="ArgumentNullException">onCacheGet</exception>
        /// <exception cref="ArgumentNullException">onCacheMiss</exception>
        /// <exception cref="ArgumentNullException">onCachePut</exception>
        /// <exception cref="ArgumentNullException">onCacheGetError</exception>
        /// <exception cref="ArgumentNullException">onCachePutError</exception>
        public static CachePolicy<TResult> Cache<TResult>(
            ISyncCacheProvider<TResult> cacheProvider,
            ITtlStrategy ttlStrategy,
            Func<Context, string> cacheKeyStrategy,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
            => Cache(cacheProvider, ttlStrategy.For<TResult>(), options =>
            {
                options.CacheKeyStrategy = cacheKeyStrategy;
                options.OnCacheGet = onCacheGet;
                options.OnCacheMiss = onCacheMiss;
                options.OnCachePut = onCachePut;
                options.OnCacheGetError = onCacheGetError;
                options.OnCachePutError = onCachePutError;
            });

        /// <summary>
        /// <para>Builds a <see cref="Policy{TResult}" /> that will function like a result cache for delegate executions returning a <typeparamref name="TResult"/>.</para>
        /// <para>Before executing a delegate, checks whether the <paramref name="cacheProvider" /> holds a value for the cache key determined by applying the cache key strategy to the execution <see cref="Context"/>.
        /// If the <paramref name="cacheProvider" /> provides a value from cache, returns that value and does not execute the governed delegate. If the <paramref name="cacheProvider" /> does not provide a value, executes the governed delegate, stores the value with the <paramref name="cacheProvider" />, then returns the value.
        /// </para>
        /// </summary>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="ttlStrategy">A strategy for specifying TTL for values to be cached.</param>
        /// <param name="configureOptions">An <see cref="Action{T}"/> to configure the provided <see cref="CachePolicyOptions"/>.</param>
        /// <returns>The policy instance.</returns>
        /// <exception cref="ArgumentNullException">cacheProvider</exception>
        /// <exception cref="ArgumentNullException">ttlStrategy</exception>
        /// <exception cref="ArgumentNullException">configureOptions</exception>
        public static CachePolicy<TResult> Cache<TResult>(ISyncCacheProvider<TResult> cacheProvider, ITtlStrategy<TResult> ttlStrategy, Action<CachePolicyOptions> configureOptions)
        {
            if (cacheProvider == null) throw new ArgumentNullException(nameof(cacheProvider));
            if (ttlStrategy == null) throw new ArgumentNullException(nameof(ttlStrategy));
            if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

            var options = new CachePolicyOptions();
            configureOptions(options);

            Action<Context, string> emptyDelegate = (_, __) => { };
            Action<Context, string, Exception> emptyErrorDelegate = (_, __, ___) => { };

            return new CachePolicy<TResult>(
                cacheProvider,
                ttlStrategy,
                options.CacheKeyStrategy ?? DefaultCacheKeyStrategy.Instance.GetCacheKey,
                options.OnCacheGet ?? emptyDelegate,
                options.OnCacheMiss ?? emptyDelegate,
                options.OnCachePut ?? emptyDelegate,
                options.OnCacheGetError ?? emptyErrorDelegate,
                options.OnCachePutError ?? emptyErrorDelegate);
        }
    }
}