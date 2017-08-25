﻿using System;

namespace Polly
{
    /// <summary>
    /// The captured result of executing a policy
    /// </summary>
    public class PolicyResult
    {
        internal PolicyResult(OutcomeType outcome, Exception finalException, ExceptionType? exceptionType, Context context)
        {
            Outcome = outcome;
            FinalException = finalException;
            ExceptionType = exceptionType;
            Context = context;
        }

        /// <summary>
        ///   The outcome of executing the policy
        /// </summary>
        public OutcomeType Outcome { get; }

        /// <summary>
        ///  The final exception captured. Will be null if policy executed successfully
        /// </summary>
        public Exception FinalException { get; }

        /// <summary>
        ///  The exception type of the final exception captured. Will be null if policy executed successfully
        /// </summary>
        public ExceptionType? ExceptionType { get; }

        /// <summary>
        ///  The context for this execution.
        /// </summary>
        public Context Context { get; }

        /// <summary>
        /// Create a new Successful PolicyResult
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns>PolicyResult with OutcomeType Successful</returns>
        public static PolicyResult Successful(Context context)
        {
            return new PolicyResult(OutcomeType.Successful, null, null, context);
        }

        /// <summary>
        /// Create a new Failure PolicyResult
        /// </summary>
        /// <param name="exception">Exception that was raised</param>
        /// <param name="exceptionType">Type of exception resulting from a failed policy</param>
        /// <param name="context">Context</param>
        /// <returns>PolicyResult with OutcomeType Failure</returns>
        public static PolicyResult Failure(Exception exception, ExceptionType exceptionType, Context context)
        {
            return new PolicyResult(OutcomeType.Failure, exception, exceptionType, context);
        }
    }

    /// <summary>
    /// The captured result of executing a policy
    /// </summary>
    public class PolicyResult<TResult>
    {
        internal PolicyResult(TResult result, OutcomeType outcome, Exception finalException, ExceptionType? exceptionType, Context context)
            : this(result, outcome, finalException, exceptionType, default(TResult), null, context)
        {

        }

        internal PolicyResult(TResult result, OutcomeType outcome, Exception finalException, ExceptionType? exceptionType, TResult finalHandledResult, FaultType? faultType, Context context)
        {
            Result = result;
            Outcome = outcome;
            FinalException = finalException;
            ExceptionType = exceptionType;
            FinalHandledResult = finalHandledResult;
            FaultType = faultType;
            Context = context;
        }

        /// <summary>
        ///   The outcome of executing the policy
        /// </summary>
        public OutcomeType Outcome { get; }

        /// <summary>
        ///  The final exception captured. Will be null if policy executed without exception.
        /// </summary>
        public Exception FinalException { get; }

        /// <summary>
        ///  The exception type of the final exception captured. Will be null if policy executed successfully
        /// </summary>
        public ExceptionType? ExceptionType { get; }

        /// <summary>
        /// The result of executing the policy. Will be default(TResult) if the policy failed
        /// </summary>
        public TResult Result { get; }

        /// <summary>
        /// The final handled result captured. Will be default(TResult) if the policy executed successfully, or terminated with an exception.
        /// </summary>
        public TResult FinalHandledResult { get; }

        /// <summary>
        ///  The fault type of the final fault captured. Will be null if policy executed successfully
        /// </summary>
        public FaultType? FaultType { get; }

        /// <summary>
        ///  The context for this execution.
        /// </summary>
        public Context Context { get; }

        /// <summary>
        /// Create a new Successful PolicyResult
        /// </summary>
        /// <param name="result">Result value</param>
        /// <param name="context">Context</param>
        /// <returns>PolicyResult with OutcomeType Successful</returns>
        public static PolicyResult<TResult> Successful(TResult result, Context context)
        {
            return new PolicyResult<TResult>(result, OutcomeType.Successful, null, null, context);
        }

        /// <summary>
        /// Create a new Failure PolicyResult
        /// </summary>
        /// <param name="exception">Exception that was raised</param>
        /// <param name="exceptionType">Type of exception resulting from a failed policy</param>
        /// <param name="context">Context</param>
        /// <returns>PolicyResult with OutcomeType Failure</returns>
        public static PolicyResult<TResult> Failure(Exception exception, ExceptionType exceptionType, Context context)
        {
            return new PolicyResult<TResult>(default(TResult), OutcomeType.Failure, exception, exceptionType, default(TResult), 
                exceptionType == Polly.ExceptionType.HandledByThisPolicy 
                ? Polly.FaultType.ExceptionHandledByThisPolicy 
                : Polly.FaultType.UnhandledException,
                context);
        }

        /// <summary>
        /// Create a new Failure PolicyResult
        /// </summary>
        /// <param name="handledResult">Result value that was handled</param>
        /// <param name="context">Context</param>
        /// <returns>PolicyResult with OutcomeType Failure</returns>
        public static PolicyResult<TResult> Failure(TResult handledResult, Context context)
        {
            return new PolicyResult<TResult>(default(TResult), OutcomeType.Failure, null, null, handledResult, Polly.FaultType.ResultHandledByThisPolicy, context);
        }
    }

    /// <summary>
    /// Represents the outcome of executing a policy
    /// </summary>
    public enum OutcomeType
    {
        /// <summary>
        /// Indicates that the policy ultimately executed successfully
        /// </summary>
        Successful,

        /// <summary>
        /// Indicates that the policy ultimately failed
        /// </summary>
        Failure
    };

    /// <summary>
    /// Represents the type of exception resulting from a failed policy
    /// </summary>
    public enum ExceptionType
    {
        /// <summary>
        /// An exception type that has been defined to be handled by this policy
        /// </summary>
        HandledByThisPolicy,

        /// <summary>
        /// An exception type that has been not been defined to be handled by this policy
        /// </summary>
        Unhandled
    }

    /// <summary>
    /// Represents the type of outcome from a failed policy
    /// </summary>
    public enum FaultType
    {
        /// <summary>
        /// An exception type that has been defined to be handled by this policy
        /// </summary>
        ExceptionHandledByThisPolicy,

        /// <summary>
        /// An exception type that has been not been defined to be handled by this policy
        /// </summary>
        UnhandledException,

        /// <summary>
        /// A result value that has been defined to be handled by this policy
        /// </summary>
        ResultHandledByThisPolicy
    }
}