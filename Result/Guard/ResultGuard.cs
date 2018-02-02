namespace System1Group.Lib.Result
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices.ComTypes;
    using CoreUtils;

    public sealed class ResultGuard<TSuccess, TFailure, TOut> : IGuardEntryPoint<TSuccess, TFailure, TOut>,
                                                                IGuardSuccessClosing<TFailure, TOut>,
                                                                IGuardFailureClosing<TOut>
    {
        private readonly Result<TSuccess, TFailure> innerResult;

        private readonly List<(Func<TSuccess, bool> predicate, Func<TSuccess, TOut> calculator)> successCalls =
            new List<(Func<TSuccess, bool>, Func<TSuccess, TOut>)>();

        private readonly List<(Func<TFailure, bool> predicate, Func<TFailure, TOut> calculator)> failureCalls =
            new List<(Func<TFailure, bool>, Func<TFailure, TOut>)>();

        private Func<TOut> defaultCall;

        public ResultGuard(Result<TSuccess, TFailure> innerResult)
        {
            Throw.IfNull(innerResult, nameof(innerResult));
            this.innerResult = innerResult;
        }

        public IGuardSuccessClosing<TFailure, TOut> Success(Action<Func<Func<TSuccess, bool>, GuardResultCalculator<TSuccess, TOut>>> configurator)
        {
            Throw.IfNull(configurator, nameof(configurator));

            configurator(predicate => new GuardResultCalculator<TSuccess, TOut>(calculate => { this.successCalls.Add((predicate, calculate)); }));

            return this;
        }

        public IGuardSuccessClosing<TFailure, TOut> Success(Action<Func<Func<TSuccess, bool>, GuardResultCalculator<TSuccess, TOut>>, Action<TOut>> configurator)
        {
            configurator(
                predicate => new GuardResultCalculator<TSuccess, TOut>(calculate => { this.successCalls.Add((predicate, calculate)); }),
                @default => { this.successCalls.Add((_ => true, _ => @default)); });

            return this;
        }

        public IGuardSuccessClosing<TFailure, TOut> Success(Action<Func<Func<TSuccess, bool>, GuardResultCalculator<TSuccess, TOut>>, Action<Func<TSuccess, TOut>>> configurator)
        {
            configurator(
                predicate => new GuardResultCalculator<TSuccess, TOut>(calculate => { this.successCalls.Add((predicate, calculate)); }),
                @default => { this.successCalls.Add((_ => true, @default)); });

            return this;
        }

        public IGuardSuccessClosing<TFailure, TOut> DoNotHandleSuccess()
        {
            return this;
        }

        public IGuardFailureClosing<TOut> Failure(Action<Func<Func<TFailure, bool>, GuardResultCalculator<TFailure, TOut>>> configurator)
        {
            Throw.IfNull(configurator, nameof(configurator));

            configurator(predicate => new GuardResultCalculator<TFailure, TOut>(calculate => { this.failureCalls.Add((predicate, calculate)); }));

            return this;
        }

        public IGuardFailureClosing<TOut> Failure(Action<Func<Func<TFailure, bool>, GuardResultCalculator<TFailure, TOut>>, Action<TOut>> configurator)
        {
            Throw.IfNull(configurator, nameof(configurator));

            configurator(
                predicate => new GuardResultCalculator<TFailure, TOut>(calculate => { this.failureCalls.Add((predicate, calculate)); }),
                @default => { this.failureCalls.Add((_ => true, _ => @default)); });

            return this;
        }

        public IGuardFailureClosing<TOut> Failure(Action<Func<Func<TFailure, bool>, GuardResultCalculator<TFailure, TOut>>, Action<Func<TFailure, TOut>>> configurator)
        {
            Throw.IfNull(configurator, nameof(configurator));

            configurator(
                predicate => new GuardResultCalculator<TFailure, TOut>(calculate => { this.failureCalls.Add((predicate, calculate)); }),
                @default => { this.failureCalls.Add((_ => true, @default)); });

            return this;
        }

        public IGuardFailureClosing<TOut> DoNotHandleFailure()
        {
            return this;
        }

        public TOut Default(TOut outValue)
        {
            this.defaultCall = () => outValue;
            return this.Do();
        }

        public TOut Default(Func<TOut> outFunc)
        {
            this.defaultCall = outFunc;
            return this.Do();
        }

        private static TOut CallFunctionOrDefault<T>(T obj, IEnumerable<(Func<T, bool> predicate, Func<T, TOut> calculator)> calls, Func<TOut> @default)
        {
            var matchingFunc = calls.FirstOrDefault(c => c.predicate(obj));
            return matchingFunc.calculator != null ? matchingFunc.calculator(obj) : @default();
        }

        private TOut Do()
        {
            return this.innerResult.Do(
                success => CallFunctionOrDefault(success, this.successCalls, this.defaultCall),
                failure => CallFunctionOrDefault(failure, this.failureCalls, this.defaultCall));
        }
    }
}