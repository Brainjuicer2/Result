namespace System1Group.Lib.Result
{
    using System;
    using CoreUtils;

    public class GuardResultCalculator<TIn, TOut>
    {
        private readonly Action<Func<TIn, TOut>> callback;

        public GuardResultCalculator(Action<Func<TIn, TOut>> callback)
        {
            Throw.IfNull(callback, nameof(callback));
            this.callback = callback;
        }

        public void Then(Func<TIn, TOut> thenPredicate)
        {
            this.callback(thenPredicate);
        }

        public void Then(TOut thenValue)
        {
            this.Then(_ => thenValue);
        }
    }
}