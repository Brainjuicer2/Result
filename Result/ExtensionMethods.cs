namespace System1Group.Lib.Result
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Attributes.ParameterTesting;
    using CoreUtils;

    public static class ExtensionMethods
    {
        public static IEnumerable<T> SelectSuccess<T, TF>(this IEnumerable<Result<T, TF>> values)
        {
            Throw.IfNull(values, "values");
            return values.Where(a => a.IsSuccess).Select(a => a.Unwrap());
        }

        public static IEnumerable<TF> SelectFailure<T, TF>(this IEnumerable<Result<T, TF>> values)
        {
            Throw.IfNull(values, "values");
            return values.Where(a => a.IsFailure).Select(a => a.UnwrapError());
        }

        public static Result<IEnumerable<TSuccess>, TFailure> UnwrapAll<TSuccess, TFailure>(this IEnumerable<Result<TSuccess, TFailure>> values)
        {
            Throw.IfNull(values, "values");

            var outList = new List<TSuccess>();
            foreach (var val in values)
            {
                if (val.IsSuccess)
                {
                    outList.Add(val.Unwrap());
                }
                else
                {
                    return val.UnwrapError();
                }
            }

            return outList;
        }

        public static Result<TValue, string> TryGetValueAsResult<TKey, TValue>([AllowedToBeNull] this IDictionary<TKey, TValue> dict, TKey key)
        {
            if (dict == null)
            {
                return "Could not get value from null dictionary";
            }

            if (dict.TryGetValue(key, out var outValue))
            {
                return outValue;
            }

            return string.Format("Dictionary does not contain key: {0}", key);
        }

        public static Result<TOutput, TFailure> ResultAggregate<TSuccess, TFailure, TOutput>(
            this IEnumerable<Result<TSuccess, TFailure>> values,
            [AllowedToBeNull] TOutput aggregateBase,
            Func<TOutput, TSuccess, TOutput> func)
        {
            Throw.IfNull(values, "values");
            Throw.IfNull(func, "func");

            return values.Aggregate((Result<TOutput, TFailure>)aggregateBase, (output, item) => output.Combine(item, func));
        }

        public static Result<TOutput, TFailure> ResultAggregate<TSuccess, TFailure, TOutput>(
            this IEnumerable<Result<TSuccess, TFailure>> values,
            Func<TOutput, TSuccess, TOutput> func)
        {
            return ResultAggregate(values, default(TOutput), func);
        }

        [ExcludeFromCodeCoverage]
        public static Result<T, string> SingleAsResult<T>([AllowedToBeNull] this IQueryable<T> values)
        {
            if (values == null)
            {
                return "SingleAsResult called on null collection";
            }

            // To make SingleAsResult efficient when the enumerable underlying it is a database, we attempt to take 2 then see if there are two elements
            // Take(2) should send 'SELECT TOP(2) *' to the database, instead of 'SELECT *' if we use an underlying enumerable
            var list = values.Take(2).ToList();
            if (list.Count == 2)
            {
                return "SingleAsResult called on collection with more than one element";
            }

            if (list.Count == 0)
            {
                return "SingleAsResult called on collection with no elements";
            }

            return list[0];
        }

        public static Result<T, string> SingleAsResult<T>([AllowedToBeNull] this IEnumerable<T> values)
        {
            if (values == null)
            {
                return "SingleAsResult called on null collection";
            }

            using (var e = values.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    return "SingleAsResult called on collection with no elements";
                }

                var result = e.Current;
                if (!e.MoveNext())
                {
                    return result;
                }

                return "SingleAsResult called on collection with more than one element";
            }
        }

        [ExcludeFromAutoParameterTests("Can't initialise concrete class")]
        public static TSuccess UnwrapOrThrow<TSuccess, TFailure>(this Result<TSuccess, TFailure> result)
            where TFailure : Exception
        {
            Throw.IfNull(result, "result");
            return result.UnwrapOr(exc => { throw exc; });
        }

        [ExcludeFromAutoParameterTests("Can't initialise concrete class")]
        public static LazyResult<TSuccess, TFailure> MakeLazy<TSuccess, TFailure>(this Result<TSuccess, TFailure> result)
        {
            Throw.IfNull(result, nameof(result));
            return result as LazyResult<TSuccess, TFailure> ?? new LazyResult<TSuccess, TFailure>(() => result);
        }

        [ExcludeFromAutoParameterTests("Can't initialise concrete class")]
        public static Result<TSuccess, TFailure> Squash<TSuccess, TFailure>(this Result<Result<TSuccess, TFailure>, TFailure> result)
        {
            Throw.IfNull(result, nameof(result));
            return result.BindToResult(t => t);
        }

        [ExcludeFromAutoParameterTests("Can't initialise concrete class")]
        public static Result<TSuccess, TFailureNew> ChangeFailure<TSuccess, TFailure, TFailureNew>(this Result<TSuccess, TFailure> result, TFailureNew newValue)
        {
            return result.Bind(success => success, _ => newValue);
        }

        [ExcludeFromAutoParameterTests("Can't initialise concrete class")]
        public static Result<TSuccess, TFailureNew> ChangeFailure<TSuccess, TFailure, TFailureNew>(
            this Result<TSuccess, TFailure> result,
            Func<TFailureNew> newValue)
        {
            Throw.IfNull(result, nameof(result));
            return result.Bind(success => success, _ => newValue());
        }

        [ExcludeFromAutoParameterTests("Can't initialise concrete class")]
        public static Result<TSuccess, TFailureNew> ChangeFailure<TSuccess, TFailure, TFailureNew>(
            this Result<TSuccess, TFailure> result,
            Func<TFailure, TFailureNew> newValue)
        {
            Throw.IfNull(result, nameof(result));
            return result.Do<Result<TSuccess, TFailureNew>>(success => success, failure => newValue(failure));
        }

        [ExcludeFromAutoParameterTests("Can't initialise concrete class")]
        public static T Either<T>(this Result<T, T> result)
        {
            Throw.IfNull(result, nameof(result));
            return result.Do(t => t, t => t);
        }

        [ExcludeFromAutoParameterTests("Can't initialise concrete class")]
        public static object Either<TSuccess, TFailure>(this Result<TSuccess, TFailure> result)
        {
            Throw.IfNull(result, nameof(result));
            return result.Do<object>(t => t, t => t);
        }

        [ExcludeFromAutoParameterTests("Can't initialise concrete class")]
        public static Result<TSuccess, TFailure> RetainIf<TSuccess, TFailure>(this Result<TSuccess, TFailure> result, Func<TSuccess, bool> predicate, TFailure replaceWith)
        {
            Throw.IfNull(result, nameof(result));
            return result.BindToResult(s => predicate(s) ? Result.Success<TSuccess, TFailure>(s) : replaceWith);
        }

        [ExcludeFromAutoParameterTests("Can't initialise concrete class")]
        public static void OnSuccess<TSuccess, TFailure>(this Result<TSuccess, TFailure> result, Action<TSuccess> action)
        {
            Throw.IfNull(result, nameof(result));
            result.Do(action, _ => { });
        }

        [ExcludeFromAutoParameterTests("Can't initialise concrete class")]
        public static void OnFailure<TSuccess, TFailure>(this Result<TSuccess, TFailure> result, Action<TFailure> action)
        {
            Throw.IfNull(result, nameof(result));
            result.Do(_ => { }, action);
        }
    }
}
