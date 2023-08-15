﻿using Polly.Telemetry;

namespace Polly.Utils;

internal abstract partial class PipelineComponent
{
    /// <summary>
    /// A combination of multiple resilience strategies.
    /// </summary>
    [DebuggerDisplay("Pipeline, Strategies = {Strategies.Count}")]
    [DebuggerTypeProxy(typeof(CompositeDebuggerProxy))]
    internal sealed class CompositeComponent : PipelineComponent
    {
        private readonly PipelineComponent _firstComponent;
        private readonly ResilienceStrategyTelemetry _telemetry;
        private readonly TimeProvider _timeProvider;

        public CompositeComponent(
            PipelineComponent first,
            IReadOnlyList<PipelineComponent> components,
            ResilienceStrategyTelemetry telemetry,
            TimeProvider timeProvider)
        {
            Components = components;

            _telemetry = telemetry;
            _timeProvider = timeProvider;
            _firstComponent = first;
        }

        public IReadOnlyList<PipelineComponent> Components { get; }

        internal override async ValueTask<Outcome<TResult>> ExecuteCore<TResult, TState>(
            Func<ResilienceContext, TState, ValueTask<Outcome<TResult>>> callback,
            ResilienceContext context,
            TState state)
        {
            var timeStamp = _timeProvider.GetTimestamp();
            _telemetry.Report(new ResilienceEvent(ResilienceEventSeverity.Debug, TelemetryUtil.PipelineExecuting), context, default(PipelineExecutingArguments));

            Outcome<TResult> outcome;

            if (context.CancellationToken.IsCancellationRequested)
            {
                outcome = Outcome.FromException<TResult>(new OperationCanceledException(context.CancellationToken).TrySetStackTrace());
            }
            else
            {
                outcome = await _firstComponent.ExecuteCore(callback, context, state).ConfigureAwait(context.ContinueOnCapturedContext);
            }

            _telemetry.Report(
                new ResilienceEvent(ResilienceEventSeverity.Information, TelemetryUtil.PipelineExecuted),
                new OutcomeArguments<TResult, PipelineExecutedArguments>(context, outcome, new PipelineExecutedArguments(_timeProvider.GetElapsedTime(timeStamp))));

            return outcome;
        }
    }

    /// <summary>
    /// A resilience strategy that delegates the execution to the next strategy in the chain.
    /// </summary>
    private sealed class DelegatingPipelineComponent : PipelineComponent
    {
        private readonly PipelineComponent _component;

        public DelegatingPipelineComponent(PipelineComponent component) => _component = component;

        public PipelineComponent? Next { get; set; }

        internal override ValueTask<Outcome<TResult>> ExecuteCore<TResult, TState>(
            Func<ResilienceContext, TState, ValueTask<Outcome<TResult>>> callback,
            ResilienceContext context,
            TState state)
        {
            return _component.ExecuteCore(
                static (context, state) =>
                {
                    if (context.CancellationToken.IsCancellationRequested)
                    {
                        return Outcome.FromExceptionAsTask<TResult>(new OperationCanceledException(context.CancellationToken).TrySetStackTrace());
                    }

                    return state.Next!.ExecuteCore(state.callback, context, state.state);
                },
                context,
                (Next, callback, state));
        }
    }

    internal sealed class CompositeDebuggerProxy
    {
        private readonly CompositeComponent _pipeline;

        public CompositeDebuggerProxy(CompositeComponent pipeline) => _pipeline = pipeline;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public IEnumerable<PipelineComponent> Strategies => _pipeline.Components;
    }
}
