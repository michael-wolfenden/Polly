﻿using System;
using System.Collections.Generic;
using Polly.Shared.CircuitBreaker;
using Polly.Utilities;
using System.Threading;

namespace Polly.CircuitBreaker
{
    /// <summary>
    /// A circuit-breaker policy that can be applied to delegates.
    /// </summary>
    public partial class CircuitBreakerPolicy : Policy, ICircuitBreakerPolicy
    {
        private readonly ICircuitController<EmptyStruct> _breakerController;

        internal CircuitBreakerPolicy(
            Action<Action<CancellationToken>, Context, CancellationToken> exceptionPolicy, 
            IEnumerable<ExceptionPredicate> exceptionPredicates,
            ICircuitController<EmptyStruct> breakerController
            ) : base(exceptionPolicy, exceptionPredicates)
        {
            _breakerController = breakerController;
        }

        /// <summary>
        /// Gets the state of the underlying circuit.
        /// </summary>
        public CircuitState CircuitState
        {
            get { return _breakerController.CircuitState; }
        }

        /// <summary>
        /// Gets the last exception handled by the circuit-breaker.
        /// </summary>
        public Exception LastException
        {
            get { return _breakerController.LastException; }
        }

        /// <summary>
        /// Isolates (opens) the circuit manually, and holds it in this state until a call to <see cref="Reset()"/> is made.
        /// </summary>
        public void Isolate()
        {
            _breakerController.Isolate();
        }

        /// <summary>
        /// Closes the circuit, and resets any statistics controlling automated circuit-breaking.
        /// </summary>
        public void Reset()
        {
            _breakerController.Reset();
        }
    }

    /// <summary>
    /// A circuit-breaker policy that can be applied to delegates returning a value of type <typeparam name="TResult"/>.
    /// </summary>
    public partial class CircuitBreakerPolicy<TResult> : Policy<TResult>, ICircuitBreakerPolicy
    {
        private readonly ICircuitController<TResult> _breakerController;

        internal CircuitBreakerPolicy(
            Func<Func<CancellationToken, TResult>, Context, CancellationToken, TResult> executionPolicy, 
            IEnumerable<ExceptionPredicate> exceptionPredicates, 
            IEnumerable<ResultPredicate<TResult>> resultPredicates, 
            ICircuitController<TResult> breakerController
            ) : base(executionPolicy, exceptionPredicates, resultPredicates)
        {
            _breakerController = breakerController;
        }

        /// <summary>
        /// Gets the state of the underlying circuit.
        /// </summary>
        public CircuitState CircuitState
        {
            get { return _breakerController.CircuitState; }
        }

        /// <summary>
        /// Gets the last exception handled by the circuit-breaker.
        /// </summary>
        public Exception LastException
        {
            get { return _breakerController.LastException; }
        }

        /// <summary>
        /// Gets the last result returned from a user delegate which the circuit-breaker handled.
        /// </summary>
        public TResult LastHandledResult
        {
            get { return _breakerController.LastHandledResult; }
        }

        /// <summary>
        /// Isolates (opens) the circuit manually, and holds it in this state until a call to <see cref="Reset()"/> is made.
        /// </summary>
        public void Isolate()
        {
            _breakerController.Isolate();
        }

        /// <summary>
        /// Closes the circuit, and resets any statistics controlling automated circuit-breaking.
        /// </summary>
        public void Reset()
        {
            _breakerController.Reset();
        }
    }

}
