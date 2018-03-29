namespace System1Group.Lib.Result
{
    using Attributes.ParameterTesting;
    using CoreUtils;

    public static class Result
    {
        public static Result<TSuccess, TFailure> Success<TSuccess, TFailure>([AllowedToBeNull] TSuccess success)
        {
            return new Success<TSuccess, TFailure>(success);
        }

        public static Result<TSuccess, TFailure> Failure<TSuccess, TFailure>([AllowedToBeNull] TFailure failure)
        {
            return new Failure<TSuccess, TFailure>(failure);
        }

        public static Result<(T1, T2), TFailure> Pack<T1, T2, TFailure>(this Result<T1, TFailure> result1, Result<T2, TFailure> result2)
        {
            Throw.IfNull(result1, nameof(result1));
            Throw.IfNull(result2, nameof(result2));

            return result1.BindToResult(r1 => result2.Bind(r2 => (r1, r2)));
        }

        public static Result<(T1, T2, T3), TFailure> Pack<T1, T2, T3, TFailure>(
            this Result<T1, TFailure> result1,
            Result<T2, TFailure> result2,
            Result<T3, TFailure> result3)
        {
            Throw.IfNull(result1, nameof(result1));
            Throw.IfNull(result2, nameof(result2));
            Throw.IfNull(result3, nameof(result3));

            return result1.BindToResult(r1 => result2.BindToResult(r2 => result3.Bind(r3 => (r1, r2, r3))));
        }

        public static Result<(T1, T2, T3, T4), TFailure> Pack<T1, T2, T3, T4, TFailure>(
            this Result<T1, TFailure> result1,
            Result<T2, TFailure> result2,
            Result<T3, TFailure> result3,
            Result<T4, TFailure> result4)
        {
            Throw.IfNull(result1, nameof(result1));
            Throw.IfNull(result2, nameof(result2));
            Throw.IfNull(result3, nameof(result3));
            Throw.IfNull(result4, nameof(result4));

            return result1.BindToResult(r1 => result2.BindToResult(r2 => result3.BindToResult(r3 => result4.Bind(r4 => (r1, r2, r3, r4)))));
        }

        public static Result<(T1, T2, T3, T4, T5), TFailure> Pack<T1, T2, T3, T4, T5, TFailure>(
            this Result<T1, TFailure> result1,
            Result<T2, TFailure> result2,
            Result<T3, TFailure> result3,
            Result<T4, TFailure> result4,
            Result<T5, TFailure> result5)
        {
            Throw.IfNull(result1, nameof(result1));
            Throw.IfNull(result2, nameof(result2));
            Throw.IfNull(result3, nameof(result3));
            Throw.IfNull(result4, nameof(result4));
            Throw.IfNull(result5, nameof(result5));

            return result1.BindToResult(
                r1 => result2.BindToResult(r2 => result3.BindToResult(r3 => result4.BindToResult(r4 => result5.Bind(r5 => (r1, r2, r3, r4, r5))))));
        }

        public static Result<(T1, T2, T3, T4, T5, T6), TFailure> Pack<T1, T2, T3, T4, T5, T6, TFailure>(
            this Result<T1, TFailure> result1,
            Result<T2, TFailure> result2,
            Result<T3, TFailure> result3,
            Result<T4, TFailure> result4,
            Result<T5, TFailure> result5,
            Result<T6, TFailure> result6)
        {
            Throw.IfNull(result1, nameof(result1));
            Throw.IfNull(result2, nameof(result2));
            Throw.IfNull(result3, nameof(result3));
            Throw.IfNull(result4, nameof(result4));
            Throw.IfNull(result5, nameof(result5));
            Throw.IfNull(result6, nameof(result6));

            return result1.BindToResult(
                r1 => result2.BindToResult(
                    r2 => result3.BindToResult(r3 => result4.BindToResult(r4 => result5.BindToResult(r5 => result6.Bind(r6 => (r1, r2, r3, r4, r5, r6)))))));
        }

        public static Result<(T1, T2, T3, T4, T5, T6, T7), TFailure> Pack<T1, T2, T3, T4, T5, T6, T7, TFailure>(
            this Result<T1, TFailure> result1,
            Result<T2, TFailure> result2,
            Result<T3, TFailure> result3,
            Result<T4, TFailure> result4,
            Result<T5, TFailure> result5,
            Result<T6, TFailure> result6,
            Result<T7, TFailure> result7)
        {
            Throw.IfNull(result1, nameof(result1));
            Throw.IfNull(result2, nameof(result2));
            Throw.IfNull(result3, nameof(result3));
            Throw.IfNull(result4, nameof(result4));
            Throw.IfNull(result5, nameof(result5));
            Throw.IfNull(result6, nameof(result6));
            Throw.IfNull(result7, nameof(result7));

            return result1.BindToResult(
                r1 => result2.BindToResult(
                    r2 => result3.BindToResult(
                        r3 => result4.BindToResult(
                            r4 => result5.BindToResult(r5 => result6.BindToResult(r6 => result7.Bind(r7 => (r1, r2, r3, r4, r5, r6, r7))))))));
        }

        public static Result<(T1, T2, T3, T4, T5, T6, T7, T8), TFailure> Pack<T1, T2, T3, T4, T5, T6, T7, T8, TFailure>(
            this Result<T1, TFailure> result1,
            Result<T2, TFailure> result2,
            Result<T3, TFailure> result3,
            Result<T4, TFailure> result4,
            Result<T5, TFailure> result5,
            Result<T6, TFailure> result6,
            Result<T7, TFailure> result7,
            Result<T8, TFailure> result8)
        {
            Throw.IfNull(result1, nameof(result1));
            Throw.IfNull(result2, nameof(result2));
            Throw.IfNull(result3, nameof(result3));
            Throw.IfNull(result4, nameof(result4));
            Throw.IfNull(result5, nameof(result5));
            Throw.IfNull(result6, nameof(result6));
            Throw.IfNull(result7, nameof(result7));
            Throw.IfNull(result8, nameof(result8));

            return result1.BindToResult(
                r1 => result2.BindToResult(
                    r2 => result3.BindToResult(
                        r3 => result4.BindToResult(
                            r4 => result5.BindToResult(
                                r5 => result6.BindToResult(r6 => result7.BindToResult(r7 => result8.Bind(r8 => (r1, r2, r3, r4, r5, r6, r7, r8)))))))));
        }

        public static Result<(T1, T2, T3, T4, T5, T6, T7, T8, T9), TFailure> Pack<T1, T2, T3, T4, T5, T6, T7, T8, T9, TFailure>(
            this Result<T1, TFailure> result1,
            Result<T2, TFailure> result2,
            Result<T3, TFailure> result3,
            Result<T4, TFailure> result4,
            Result<T5, TFailure> result5,
            Result<T6, TFailure> result6,
            Result<T7, TFailure> result7,
            Result<T8, TFailure> result8,
            Result<T9, TFailure> result9)
        {
            Throw.IfNull(result1, nameof(result1));
            Throw.IfNull(result2, nameof(result2));
            Throw.IfNull(result3, nameof(result3));
            Throw.IfNull(result4, nameof(result4));
            Throw.IfNull(result5, nameof(result5));
            Throw.IfNull(result6, nameof(result6));
            Throw.IfNull(result7, nameof(result7));
            Throw.IfNull(result8, nameof(result8));
            Throw.IfNull(result9, nameof(result9));

            return result1.BindToResult(
                r1 => result2.BindToResult(
                    r2 => result3.BindToResult(
                        r3 => result4.BindToResult(
                            r4 => result5.BindToResult(
                                r5 => result6.BindToResult(
                                    r6 => result7.BindToResult(
                                        r7 => result8.BindToResult(r8 => result9.Bind(r9 => (r1, r2, r3, r4, r5, r6, r7, r8, r9))))))))));
        }

        public static Result<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10), TFailure> Pack<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TFailure>(
            this Result<T1, TFailure> result1,
            Result<T2, TFailure> result2,
            Result<T3, TFailure> result3,
            Result<T4, TFailure> result4,
            Result<T5, TFailure> result5,
            Result<T6, TFailure> result6,
            Result<T7, TFailure> result7,
            Result<T8, TFailure> result8,
            Result<T9, TFailure> result9,
            Result<T10, TFailure> result10)
        {
            Throw.IfNull(result1, nameof(result1));
            Throw.IfNull(result2, nameof(result2));
            Throw.IfNull(result3, nameof(result3));
            Throw.IfNull(result4, nameof(result4));
            Throw.IfNull(result5, nameof(result5));
            Throw.IfNull(result6, nameof(result6));
            Throw.IfNull(result7, nameof(result7));
            Throw.IfNull(result8, nameof(result8));
            Throw.IfNull(result9, nameof(result9));
            Throw.IfNull(result10, nameof(result10));

            return result1.BindToResult(
                r1 => result2.BindToResult(
                    r2 => result3.BindToResult(
                        r3 => result4.BindToResult(
                            r4 => result5.BindToResult(
                                r5 => result6.BindToResult(
                                    r6 => result7.BindToResult(
                                        r7 => result8.BindToResult(
                                            r8 => result9.BindToResult(r9 => result10.Bind(r10 => (r1, r2, r3, r4, r5, r6, r7, r8, r9, r10)))))))))));
        }
    }
}
