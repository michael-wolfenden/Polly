using System.Threading.Tasks;

namespace Polly.Strategy;

internal abstract class PredicateInvoker<TArgs>
    where TArgs : IResilienceArguments
{
    public static PredicateInvoker<TArgs>? Create<TResult>(Func<Outcome<TResult>, TArgs, ValueTask<bool>>? predicate, bool isGeneric) => predicate switch
    {
        null => null,
        Func<Outcome<object>, TArgs, ValueTask<bool>> objectPredicate when !isGeneric => new NonGenericPredicateInvoker(objectPredicate),
        _ => new GenericPredicateInvoker<TResult>(predicate)
    };

    public abstract ValueTask<bool> HandleAsync<TResult>(Outcome<TResult> outcome, TArgs args);

    private sealed class NonGenericPredicateInvoker : PredicateInvoker<TArgs>
    {
        private readonly Func<Outcome<object>, TArgs, ValueTask<bool>> _predicate;

        public NonGenericPredicateInvoker(Func<Outcome<object>, TArgs, ValueTask<bool>> predicate) => _predicate = predicate;

        public override ValueTask<bool> HandleAsync<TResult>(Outcome<TResult> outcome, TArgs args) => _predicate(outcome.AsOutcome(), args);
    }

    private sealed class GenericPredicateInvoker<T> : PredicateInvoker<TArgs>
    {
        private readonly object _predicate;

        public GenericPredicateInvoker(Func<Outcome<T>, TArgs, ValueTask<bool>> predicate) => _predicate = predicate;

        public override ValueTask<bool> HandleAsync<TResult>(Outcome<TResult> outcome, TArgs args)
        {
            if (typeof(TResult) == typeof(T))
            {
                return ((Func<Outcome<TResult>, TArgs, ValueTask<bool>>)_predicate)(outcome, args);
            }

            return PredicateResult.False;
        }
    }
}
