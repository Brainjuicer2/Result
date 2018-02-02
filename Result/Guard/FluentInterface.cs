namespace System1Group.Lib.Result
{
    using System;

#pragma warning disable SA1649 // File name must match first type name
    public interface IGuardEntryPoint<TSuccess, TFailure, TOut>
#pragma warning restore SA1649 // File name must match first type name
    {
        IGuardSuccessClosing<TFailure, TOut> Success(Action<Func<Func<TSuccess, bool>, GuardResultCalculator<TSuccess, TOut>>> configurator);

        IGuardSuccessClosing<TFailure, TOut> Success(Action<Func<Func<TSuccess, bool>, GuardResultCalculator<TSuccess, TOut>>, Action<TOut>> configurator);

        IGuardSuccessClosing<TFailure, TOut> Success(Action<Func<Func<TSuccess, bool>, GuardResultCalculator<TSuccess, TOut>>, Action<Func<TSuccess, TOut>>> configurator);

        IGuardSuccessClosing<TFailure, TOut> DoNotHandleSuccess();
    }

    public interface IGuardSuccessClosing<TFailure, TOut>
    {
        IGuardFailureClosing<TOut> Failure(Action<Func<Func<TFailure, bool>, GuardResultCalculator<TFailure, TOut>>> configurator);

        IGuardFailureClosing<TOut> Failure(Action<Func<Func<TFailure, bool>, GuardResultCalculator<TFailure, TOut>>, Action<TOut>> configurator);

        IGuardFailureClosing<TOut> Failure(Action<Func<Func<TFailure, bool>, GuardResultCalculator<TFailure, TOut>>, Action<Func<TFailure, TOut>>> configurator);

        IGuardFailureClosing<TOut> DoNotHandleFailure();
    }

    public interface IGuardFailureClosing<TOut>
    {
        TOut Default(TOut outValue);

        TOut Default(Func<TOut> outFunc);
    }
}