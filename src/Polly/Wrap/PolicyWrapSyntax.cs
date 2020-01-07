﻿using System;
using System.Linq;
using Polly.Wrap;

namespace Polly
{
    public partial class Policy
    {
        /// <summary>
        /// Creates a <see cref="PolicyWrap" /> of the given policies.
        /// </summary>
        /// <param name="policies">The policies to place in the wrap, outermost (at left) to innermost (at right).</param>
        /// <returns>The PolicyWrap.</returns>
        /// <exception cref="ArgumentException">The enumerable of policies to form the wrap must contain at least two policies.</exception>
        public static ISyncPolicyWrap Wrap(params ISyncPolicy[] policies)
        {
            switch (policies.Length)
            {
                case 0:
                case 1:
                    throw new ArgumentException("The enumerable of policies to form the wrap must contain at least two policies.", nameof(policies));
                case 2:
                    return new PolicyWrap(policies[0], policies[1]);

                default:
                    return Wrap(policies[0], Wrap(policies.Skip(1).ToArray()));
            }
        }

        /// <summary>
        /// Creates a <see cref="PolicyWrap" /> of the given policies governing delegates returning values of type <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="policies">The policies to place in the wrap, outermost (at left) to innermost (at right).</param>
        /// <typeparam name="TResult">The return type of delegates which may be executed through the policy.</typeparam>
        /// <returns>The PolicyWrap.</returns>
        /// <exception cref="ArgumentException">The enumerable of policies to form the wrap must contain at least two policies.</exception>
        public static ISyncPolicyWrap<TResult> Wrap<TResult>(params ISyncPolicy<TResult>[] policies)
        {
            switch (policies.Length)
            {
                case 0:
                case 1:
                    throw new ArgumentException("The enumerable of policies to form the wrap must contain at least two policies.", nameof(policies));
                case 2:
                    return new PolicyWrap<TResult>(policies[0], policies[1]);

                default:
                    return Wrap(policies[0], Wrap(policies.Skip(1).ToArray()));
            }
        }
    }

    /// <summary>
    /// Defines extensions for configuring <see cref="PolicyWrap"/> instances on an <see cref="ISyncPolicy"/> or <see cref="ISyncPolicy{TResult}"/>.
    /// </summary>
    public static class ISyncPolicyPolicyWrapExtensions
    {
        /// <summary>
        /// Wraps the given outer policy round the inner policy.
        /// </summary>
        /// <param name="outerPolicy">The outer policy.</param>
        /// <param name="innerPolicy">The inner policy.</param>
        /// <returns>A <see cref="PolicyWrap"/> instance representing the combined wrap.</returns>
        public static ISyncPolicyWrap Wrap(this ISyncPolicy outerPolicy, ISyncPolicy innerPolicy)
        {
            if (outerPolicy == null) throw new ArgumentNullException(nameof(outerPolicy));
            if (innerPolicy == null) throw new ArgumentNullException(nameof(innerPolicy));

            return new PolicyWrap(
                outerPolicy,
                innerPolicy
            );
        }

        /// <summary>
        /// Wraps the given outer policy round the inner policy.
        /// </summary>
        /// <param name="outerPolicy">The outer policy.</param>
        /// <param name="innerPolicy">The inner policy.</param>
        /// <returns>A <see cref="PolicyWrap"/> instance representing the combined wrap.</returns>
        public static ISyncPolicyWrap<TResult> Wrap<TResult>(this ISyncPolicy outerPolicy, ISyncPolicy<TResult> innerPolicy)
        {
            if (outerPolicy == null) throw new ArgumentNullException(nameof(outerPolicy));
            if (innerPolicy == null) throw new ArgumentNullException(nameof(innerPolicy));

            return new PolicyWrap<TResult>(
                outerPolicy,
                innerPolicy
            );
        }

        /// <summary>
        /// Wraps the given outer policy round the inner policy.
        /// </summary>
        /// <param name="outerPolicy">The outer policy.</param>
        /// <param name="innerPolicy">The inner policy.</param>
        /// <returns>A <see cref="PolicyWrap"/> instance representing the combined wrap.</returns>
        public static ISyncPolicyWrap<TResult> Wrap<TResult>(this ISyncPolicy<TResult> outerPolicy, ISyncPolicy innerPolicy)
        {
            if (outerPolicy == null) throw new ArgumentNullException(nameof(outerPolicy));
            if (innerPolicy == null) throw new ArgumentNullException(nameof(innerPolicy));

            return new PolicyWrap<TResult>(
                outerPolicy,
                innerPolicy
                );
        }

        /// <summary>
        /// Wraps the given outer policy round the inner policy.
        /// </summary>
        /// <param name="outerPolicy">The outer policy.</param>
        /// <param name="innerPolicy">The inner policy.</param>
        /// <returns>A <see cref="PolicyWrap"/> instance representing the combined wrap.</returns>
        public static ISyncPolicyWrap<TResult> Wrap<TResult>(this ISyncPolicy<TResult> outerPolicy, ISyncPolicy<TResult> innerPolicy)
        {
            if (outerPolicy == null) throw new ArgumentNullException(nameof(outerPolicy));
            if (innerPolicy == null) throw new ArgumentNullException(nameof(innerPolicy));

            return new PolicyWrap<TResult>(
                outerPolicy,
                innerPolicy
            );
        }
    }
}
