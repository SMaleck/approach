using System;
using System.Linq;
using UniRx;

namespace _Source.Util
{
    public static class UniRxExtensions
    {
        public static IObservable<bool> IfTrue(this IObservable<bool> observable)
        {
            return observable
                .Where(value => value);
        }

        public static IObservable<bool> IfFalse(this IObservable<bool> observable)
        {
            return observable
                .Where(value => !value);
        }

        public static IObservable<Pair<bool>> IfSwitchedToTrue(this IObservable<bool> observable)
        {
            return observable.Pairwise()
                .Where(pair => !pair.Previous && pair.Current);
        }

        public static IObservable<Pair<bool>> IfSwitchedToFalse(this IObservable<bool> observable)
        {
            return observable.Pairwise()
                .Where(pair => pair.Previous && !pair.Current);
        }

        public static IObservable<Unit> AnyCollectionChangeAsObservable<T>(this IReadOnlyReactiveCollection<T> reactiveCollection)
        {
            return Observable.Merge(
                reactiveCollection.ObserveReset().AsUnitObservable(),
                reactiveCollection.ObserveAdd().AsUnitObservable(),
                reactiveCollection.ObserveMove().AsUnitObservable(),
                reactiveCollection.ObserveRemove().AsUnitObservable(),
                reactiveCollection.ObserveReplace().AsUnitObservable());
        }

        public static IObservable<Unit> AnyDictionaryChangeAsObservable<TKey, TValue>(this IReadOnlyReactiveDictionary<TKey, TValue> reactiveDictionary)
        {
            return Observable.Merge(
                reactiveDictionary.ObserveReset().AsUnitObservable(),
                reactiveDictionary.ObserveAdd().AsUnitObservable(),
                reactiveDictionary.ObserveRemove().AsUnitObservable(),
                reactiveDictionary.ObserveReplace().AsUnitObservable(),
                reactiveDictionary.ObserveCountChanged().AsUnitObservable());
        }

        public static IObservable<T> OncePerFrame<T>(this IObservable<T> observable)
        {
            return observable.BatchFrame().Select(batch => batch.Last());
        }
    }
}
