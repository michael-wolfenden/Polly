using Microsoft.Extensions.Time.Testing;
using Polly.Retry;
using Polly.Telemetry;
using Polly.Testing;
using Xunit.Sdk;

namespace Polly.Core.Tests.Retry;

public class RetryResilienceStrategyTests
{
    private readonly RetryStrategyOptions _options = new();
    private readonly FakeTimeProvider _timeProvider = new();
    private readonly List<TelemetryEventArguments<object, object>> _args = [];
    private ResilienceStrategyTelemetry _telemetry;

    public RetryResilienceStrategyTests()
    {
        _telemetry = TestUtilities.CreateResilienceTelemetry(_args.Add);
        _options.ShouldHandle = _ => new ValueTask<bool>(false);
        _options.Randomizer = () => 1;
    }

    [Fact]
    public void ExecuteAsync_EnsureResultNotDisposed()
    {
        SetupNoDelay();
        var sut = CreateSut();

        var result = sut.Execute(() => new DisposableResult());
        result.IsDisposed.ShouldBeFalse();
    }

    [Fact]
    public async Task ExecuteAsync_CanceledBeforeExecution_EnsureNotExecuted()
    {
        var sut = CreateSut();
        var executed = false;

        var result = await sut.ExecuteOutcomeAsync(
            (_, _) =>
            {
                executed = true;
                return Outcome.FromResultAsValueTask(new object());
            },
            ResilienceContextPool.Shared.Get(new CancellationToken(canceled: true)),
            default(object));

        result.Exception.ShouldBeAssignableTo<OperationCanceledException>();
        executed.ShouldBeFalse();
    }

    [Fact]
    public async Task ExecuteAsync_CanceledDuringExecution_EnsureResultReturned()
    {
        var sut = CreateSut();
        using var cancellation = new CancellationTokenSource();
        var executions = 0;

        var result = await sut.ExecuteOutcomeAsync(
            (_, _) =>
            {
                executions++;
                cancellation.Cancel();
                return Outcome.FromResultAsValueTask(new object());
            },
            ResilienceContextPool.Shared.Get(cancellation.Token),
            default(object));

        result.Exception.ShouldBeNull();
        executions.ShouldBe(1);
    }

    [Fact]
    public async Task ExecuteAsync_CanceledDuringExecution_EnsureNotExecutedAgain()
    {
        var reported = false;

        _options.ShouldHandle = _ => PredicateResult.True();
        _options.OnRetry =
            args =>
            {
                reported = true;
                return default;
            };

        var sut = CreateSut();
        using var cancellation = new CancellationTokenSource();
        var executions = 0;

        var result = await sut.ExecuteOutcomeAsync(
            (_, _) =>
            {
                executions++;
                cancellation.Cancel();
                return Outcome.FromResultAsValueTask(new object());
            },
            ResilienceContextPool.Shared.Get(cancellation.Token),
            default(object));

        result.Exception.ShouldBeAssignableTo<OperationCanceledException>();
        executions.ShouldBe(1);
        reported.ShouldBeTrue();
    }

    [Fact]
    public async Task ExecuteAsync_CanceledAfterExecution_EnsureNotExecutedAgain()
    {
        using var cancellation = new CancellationTokenSource();

        _options.ShouldHandle = _ => PredicateResult.True();
        _options.OnRetry =
            args =>
            {
                cancellation.Cancel();
                return default;
            };

        var sut = CreateSut();
        var executions = 0;

        var result = await sut.ExecuteOutcomeAsync(
            (_, _) =>
            {
                executions++;
                return Outcome.FromResultAsValueTask(new object());
            },
            ResilienceContextPool.Shared.Get(cancellation.Token),
            default(object));

        result.Exception.ShouldBeAssignableTo<OperationCanceledException>();
        executions.ShouldBe(1);
    }

    [Fact]
    public async Task ExecuteAsync_CanceledDuringDelay_EnsureNotExecutedAgain()
    {
        _options.ShouldHandle = _ => PredicateResult.True();

        using var cancellation = _timeProvider.CreateCancellationTokenSource(_options.Delay);

        var sut = CreateSut();
        var executions = 0;

        var resultTask = sut.ExecuteOutcomeAsync(
            (_, _) =>
            {
                executions++;
                return Outcome.FromResultAsValueTask(new object());
            },
            ResilienceContextPool.Shared.Get(cancellation.Token),
            default(object));

        _timeProvider.Advance(_options.Delay);
        var result = await resultTask;

        result.Exception.ShouldBeAssignableTo<OperationCanceledException>();
        executions.ShouldBe(1);
    }

    [Fact]
    public void ExecuteAsync_MultipleRetries_EnsureDiscardedResultsDisposed()
    {
        // arrange
        _options.MaxRetryAttempts = 5;
        SetupNoDelay();
        _options.ShouldHandle = _ => PredicateResult.True();
        var results = new List<DisposableResult>();
        var sut = CreateSut();

        // act
        var result = sut.Execute(_ =>
        {
            var r = new DisposableResult();
            results.Add(r);
            return r;
        });

        // assert
        result.IsDisposed.ShouldBeFalse();
        results.Count.ShouldBe(_options.MaxRetryAttempts + 1);
        results[results.Count - 1].IsDisposed.ShouldBeFalse();

        results.Remove(results[results.Count - 1]);
        results.ShouldAllBe(r => r.IsDisposed);
    }

    [Fact]
    public void Retry_RetryCount_Respected()
    {
        int calls = 0;
        _options.OnRetry = _ => { calls++; return default; };
        _options.ShouldHandle = args => args.Outcome.ResultPredicateAsync(0);
        _options.MaxRetryAttempts = 12;
        SetupNoDelay();
        var sut = CreateSut();

        sut.Execute(() => 0);

        calls.ShouldBe(12);
    }

    [Fact]
    public void RetryException_RetryCount_Respected()
    {
        int calls = 0;
        _options.OnRetry = args =>
        {
            args.Outcome.Exception.ShouldBeOfType<InvalidOperationException>();
            calls++;
            return default;
        };

        _options.ShouldHandle = args => args.Outcome.ExceptionPredicateAsync<InvalidOperationException>();
        _options.MaxRetryAttempts = 3;
        SetupNoDelay();
        var sut = CreateSut();

        Assert.Throws<InvalidOperationException>(() => sut.Execute<int>(() => throw new InvalidOperationException()));

        calls.ShouldBe(3);
    }

    [Fact]
    public void RetryDelayGenerator_Respected()
    {
        var retries = 0;
        var generatedValues = 0;
        var delay = TimeSpan.FromMilliseconds(120);

        _options.ShouldHandle = _ => PredicateResult.True();
        _options.MaxRetryAttempts = 3;
        _options.BackoffType = DelayBackoffType.Constant;

        _options.OnRetry = args =>
        {
            retries++;
            args.RetryDelay.ShouldBe(delay);
            return default;
        };
        _options.DelayGenerator = _ =>
        {
            generatedValues++;
            return new ValueTask<TimeSpan?>(delay);
        };

        CreateSut(TimeProvider.System).Execute(_ => "dummy");

        retries.ShouldBe(3);
        generatedValues.ShouldBe(3);
    }

    [Fact]
    public async Task RetryDelayGenerator_ZeroDelay_NoTimeProviderCalls()
    {
        int retries = 0;
        int generatedValues = 0;

        var delay = TimeSpan.Zero;
        var provider = new ThrowingFakeTimeProvider();

        _options.ShouldHandle = _ => PredicateResult.True();
        _options.MaxRetryAttempts = 3;
        _options.BackoffType = DelayBackoffType.Constant;

        _options.OnRetry = _ =>
        {
            retries++;
            return default;
        };
        _options.DelayGenerator = _ =>
        {
            generatedValues++;
            return new ValueTask<TimeSpan?>(delay);
        };

        var sut = CreateSut(provider);
        await sut.ExecuteAsync(_ => new ValueTask<string>("dummy"));

        retries.ShouldBe(3);
        generatedValues.ShouldBe(3);
    }

    [Fact]
    public void IsLastAttempt_Ok()
    {
        var sut = (RetryResilienceStrategy<object>)CreateSut().GetPipelineDescriptor().FirstStrategy.StrategyInstance;

        sut.IsLastAttempt(int.MaxValue, out var increment).ShouldBeFalse();
        increment.ShouldBeFalse();
    }

    private sealed class ThrowingFakeTimeProvider : FakeTimeProvider
    {
        public override DateTimeOffset GetUtcNow() => throw new XunitException("TimeProvider should not be used.");

        public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
            => throw new XunitException("TimeProvider should not be used.");
    }

    [Fact]
    public async Task OnRetry_EnsureCorrectArguments()
    {
        var attempts = new List<int>();
        var delays = new List<TimeSpan>();
        _options.OnRetry = args =>
        {
            attempts.Add(args.AttemptNumber);
            delays.Add(args.RetryDelay);

            args.Outcome.Exception.ShouldBeNull();
            args.Outcome.Result.ShouldBe(0);
            return default;
        };

        _options.ShouldHandle = args => PredicateResult.True();
        _options.MaxRetryAttempts = 3;
        _options.BackoffType = DelayBackoffType.Linear;

        var sut = CreateSut();

        var executing = ExecuteAndAdvance(sut);

        await executing;

        attempts.Count.ShouldBe(3);
        attempts[0].ShouldBe(0);
        attempts[1].ShouldBe(1);
        attempts[2].ShouldBe(2);

        delays[0].ShouldBe(TimeSpan.FromSeconds(2));
        delays[1].ShouldBe(TimeSpan.FromSeconds(4));
        delays[2].ShouldBe(TimeSpan.FromSeconds(6));
    }

    [Fact]
    public async Task MaxDelay_EnsureRespected()
    {
        var delays = new List<TimeSpan>();
        _options.OnRetry = args =>
        {
            delays.Add(args.RetryDelay);
            return default;
        };

        _options.ShouldHandle = args => PredicateResult.True();
        _options.MaxRetryAttempts = 3;
        _options.BackoffType = DelayBackoffType.Linear;
        _options.MaxDelay = TimeSpan.FromMilliseconds(123);

        var sut = CreateSut();

        await ExecuteAndAdvance(sut);

        delays[0].ShouldBe(TimeSpan.FromMilliseconds(123));
        delays[1].ShouldBe(TimeSpan.FromMilliseconds(123));
        delays[2].ShouldBe(TimeSpan.FromMilliseconds(123));
    }

    [Fact]
    public async Task OnRetry_EnsureExecutionTime()
    {
        _options.OnRetry = args =>
        {
            args.Duration.ShouldBe(TimeSpan.FromMinutes(1));

            return default;
        };

        _options.ShouldHandle = _ => PredicateResult.True();
        _options.MaxRetryAttempts = 1;
        _options.BackoffType = DelayBackoffType.Constant;
        _options.Delay = TimeSpan.Zero;

        var sut = CreateSut();

        await sut.ExecuteAsync(_ =>
        {
            _timeProvider.Advance(TimeSpan.FromMinutes(1));
            return new ValueTask<int>(0);
        }).AsTask();
    }

    [Fact]
    public void Execute_NotHandledOriginalAttempt_EnsureAttemptReported()
    {
        var called = false;
        _telemetry = TestUtilities.CreateResilienceTelemetry(args =>
        {
            var attempt = args.Arguments.ShouldBeOfType<ExecutionAttemptArguments>();
            args.Event.Severity.ShouldBe(ResilienceEventSeverity.Information);
            attempt.Handled.ShouldBeFalse();
            attempt.AttemptNumber.ShouldBe(0);
            attempt.Duration.ShouldBe(TimeSpan.FromSeconds(1));
            called = true;
        });

        var sut = CreateSut();

        sut.Execute(() =>
        {
            _timeProvider.Advance(TimeSpan.FromSeconds(1));
            return 0;
        });

        called.ShouldBeTrue();
    }

    [Fact]
    public void Execute_NotHandledFinalAttempt_EnsureAttemptReported()
    {
        _options.MaxRetryAttempts = 1;
        _options.Delay = TimeSpan.Zero;

        // original attempt is handled, retried attempt is not handled
        _options.ShouldHandle = args => new ValueTask<bool>(args.AttemptNumber == 0);
        var called = false;
        _telemetry = TestUtilities.CreateResilienceTelemetry(args =>
        {
            // ignore OnRetry event
            if (args.Arguments is OnRetryArguments<object>)
            {
                return;
            }

            var attempt = args.Arguments.ShouldBeOfType<ExecutionAttemptArguments>();
            if (attempt.AttemptNumber == 0)
            {
                args.Event.Severity.ShouldBe(ResilienceEventSeverity.Warning);
            }
            else
            {
                args.Event.Severity.ShouldBe(ResilienceEventSeverity.Information);
            }

            called = true;
        });

        var sut = CreateSut();

        sut.Execute(() =>
        {
            _timeProvider.Advance(TimeSpan.FromSeconds(1));
            return 0;
        });

        called.ShouldBeTrue();
    }

    [Fact]
    public void Execute_HandledFinalAttempt_EnsureAttemptReported()
    {
        _options.MaxRetryAttempts = 1;
        _options.Delay = TimeSpan.Zero;
        _options.ShouldHandle = _ => new ValueTask<bool>(true);
        var called = false;
        _telemetry = TestUtilities.CreateResilienceTelemetry(args =>
        {
            // ignore OnRetry event
            if (args.Arguments is OnRetryArguments<object>)
            {
                return;
            }

            var attempt = args.Arguments.ShouldBeOfType<ExecutionAttemptArguments>();
            if (attempt.AttemptNumber == 0)
            {
                args.Event.Severity.ShouldBe(ResilienceEventSeverity.Warning);
            }
            else
            {
                args.Event.Severity.ShouldBe(ResilienceEventSeverity.Error);
            }

            called = true;
        });

        var sut = CreateSut();

        sut.Execute(() =>
        {
            _timeProvider.Advance(TimeSpan.FromSeconds(1));
            return 0;
        });

        called.ShouldBeTrue();
    }

    [Fact]
    public async Task OnRetry_EnsureTelemetry()
    {
        var attempts = new List<int>();
        var delays = new List<TimeSpan>();

        _options.ShouldHandle = args => args.Outcome.ResultPredicateAsync(0);
        _options.MaxRetryAttempts = 3;
        _options.BackoffType = DelayBackoffType.Linear;

        var sut = CreateSut();

        await ExecuteAndAdvance(sut);

        _args.Select(a => a.Arguments).OfType<OnRetryArguments<object>>().Count().ShouldBe(3);
    }

    [Fact]
    public void RetryDelayGenerator_EnsureCorrectArguments()
    {
        var attempts = new List<int>();
        var hints = new List<TimeSpan>();
        _options.DelayGenerator = args =>
        {
            attempts.Add(args.AttemptNumber);

            args.Outcome.Exception.ShouldBeNull();
            args.Outcome.Result.ShouldBe(0);

            return new ValueTask<TimeSpan?>(TimeSpan.Zero);
        };

        _options.ShouldHandle = args => args.Outcome.ResultPredicateAsync(0);
        _options.MaxRetryAttempts = 3;
        _options.BackoffType = DelayBackoffType.Linear;

        var sut = CreateSut();

        sut.Execute(() => 0);

        attempts.Count.ShouldBe(3);
        attempts[0].ShouldBe(0);
        attempts[1].ShouldBe(1);
        attempts[2].ShouldBe(2);
    }

    [Fact]
    public void RetryDelayGenerator_ReturnsNull_EnsureDefaultRetry()
    {
        var delays = new List<TimeSpan>();
        _options.DelayGenerator = args => new ValueTask<TimeSpan?>((TimeSpan?)null);
        _options.OnRetry = args =>
        {
            delays.Add(args.RetryDelay);
            return default;
        };
        _options.ShouldHandle = args => args.Outcome.ResultPredicateAsync(0);
        _options.MaxRetryAttempts = 2;
        _options.BackoffType = DelayBackoffType.Constant;
        _options.Delay = TimeSpan.FromMilliseconds(2);

        var sut = CreateSut(TimeProvider.System);

        sut.Execute(() => 0);

        delays.Count.ShouldBe(2);
        delays[0].ShouldBe(TimeSpan.FromMilliseconds(2));
        delays[1].ShouldBe(TimeSpan.FromMilliseconds(2));
    }

    private void SetupNoDelay() => _options.DelayGenerator = _ => new ValueTask<TimeSpan?>(TimeSpan.Zero);

    private async ValueTask<int> ExecuteAndAdvance(ResiliencePipeline<object> sut)
    {
        var executing = sut.ExecuteAsync(_ => new ValueTask<int>(0)).AsTask();

        while (!executing.IsCompleted)
        {
            _timeProvider.Advance(TimeSpan.FromMinutes(1));
        }

        return await executing;
    }

    private ResiliencePipeline<object> CreateSut(TimeProvider? timeProvider = null) =>
        new RetryResilienceStrategy<object>(_options, timeProvider ?? _timeProvider, _telemetry).AsPipeline();
}
