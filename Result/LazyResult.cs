namespace System1Group.Lib.Result
{
    using System;
    using Attributes.ParameterGeneration;
    using Attributes.ParameterTesting;
    using CoreUtils;

    public class LazyResult<TSuccess, TFailure> : Result<TSuccess, TFailure>
    {
        private readonly LazyValue<Result<TSuccess, TFailure>> lazyResult;

        public LazyResult(Func<Result<TSuccess, TFailure>> factory)
        {
            Throw.IfNull(factory, "factory");
            this.lazyResult = LazyValue.Create(factory);
        }

        public override bool IsSuccess => this.lazyResult.Value.IsSuccess;

        public override TReturn Do<TReturn>([AllowedToBeNull] Func<TSuccess, TReturn> onSuccess, [AllowedToBeNull] Func<TFailure, TReturn> onFailure)
        {
            return this.lazyResult.Value.Do(onSuccess, onFailure);
        }

        public override Result<TReturn, TFailure> Bind<TReturn>(Func<TSuccess, TReturn> bindingAction)
        {
            Throw.IfNull(bindingAction, nameof(bindingAction));
            return LazyResult.Create(() => base.Bind(bindingAction));
        }

        public override Result<TNewSuccess, TNewFailure> Bind<TNewSuccess, TNewFailure>(
            Func<TSuccess, TNewSuccess> successBindingAction,
            Func<TFailure, TNewFailure> failureBindingAction)
        {
            Throw.IfNull(successBindingAction, nameof(successBindingAction));
            Throw.IfNull(failureBindingAction, nameof(failureBindingAction));

            return LazyResult.Create(() => base.Bind(successBindingAction, failureBindingAction));
        }

        public override Result<TReturn, TFailure> BindToResult<TReturn>(Func<TSuccess, Result<TReturn, TFailure>> bindingAction)
        {
            Throw.IfNull(bindingAction, nameof(bindingAction));
            return LazyResult.Create(() => base.BindToResult(bindingAction));
        }
    }
}
