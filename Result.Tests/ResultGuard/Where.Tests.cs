namespace System1Group.Lib.Result.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ResultGuard_Where_Tests
    {
        [Test]
        public void Ok_Success()
        {
            Result<int, string> wrappedResult = new Success<int, string>(10);
            var guard = new ResultGuard<int, string, int>(wrappedResult);
            guard.Success(when =>
            {
                when(i => i == 5).Then(0);
                when(i => i == 10).Then(i => i * 2);
            });
            var result = guard.Default(100);
            Assert.AreEqual(result, 20);
        }

        [Test]
        public void Ok_Failure()
        {
            Result<int, string> wrappedResult = new Failure<int, string>("Error");
            var guard = new ResultGuard<int, string, string>(wrappedResult);
            guard.Failure(when =>
            {
                when(i => i == "something").Then(string.Empty);
                when(i => i == "Error").Then(i => i + i);
            });
            var result = guard.Default(() => null);
            Assert.AreEqual(result, "ErrorError");
        }

        [Test]
        public void Ok_Success_MultipleMatches()
        {
            Result<int, string> wrappedResult = new Success<int, string>(10);
            var guard = new ResultGuard<int, string, string>(wrappedResult);
            guard.Success(when =>
            {
                when(i => i == 10).Then("first match");
                when(i => i == 10).Then("second match");
            });
            var result = guard.Default(() => null);
            Assert.AreEqual(result, "first match");
        }

        [Test]
        public void Ok_Failure_MultipleMatches()
        {
            Result<int, string> wrappedResult = new Failure<int, string>("Error");
            var guard = new ResultGuard<int, string, string>(wrappedResult);
            guard.Failure(when =>
            {
                when(i => i == "Error").Then("first match");
                when(i => i == "Error").Then("second match");
            });
            var result = guard.Default(() => null);
            Assert.AreEqual(result, "first match");
        }

        [Test]
        public void Ok_Success_FallsThroughToDefaultFunc()
        {
            Result<int, string> wrappedResult = new Success<int, string>(10);
            var guard = new ResultGuard<int, string, string>(wrappedResult);

            guard.Success((when, @default) =>
            {
                when(i => i == 11).Then("no match");
                when(i => i == 11).Then("no match");
                @default(i => "match " + i);
            });

            var result = guard.Default(() => null);
            Assert.AreEqual(result, "match 10");
        }

        [Test]
        public void Ok_Success_FallsThroughToDefaultValue()
        {
            Result<int, string> wrappedResult = new Success<int, string>(10);
            var guard = new ResultGuard<int, string, string>(wrappedResult);

            guard.Success((when, @default) =>
            {
                when(i => i == 11).Then("no match");
                when(i => i == 11).Then("no match");
                @default("match");
            });

            var result = guard.Default(() => null);
            Assert.AreEqual(result, "match");
        }

        [Test]
        public void Ok_Failure_FallsThroughToDefaultFunc()
        {
            Result<int, string> wrappedResult = new Failure<int, string>("Error");
            var guard = new ResultGuard<int, string, string>(wrappedResult);
            guard.Failure((when, @default) =>
            {
                when(i => i == string.Empty).Then("no match");
                when(i => i == string.Empty).Then("no match");
                @default(i => "match " + i);
            });
            var result = guard.Default(() => null);
            Assert.AreEqual(result, "match Error");
        }

        [Test]
        public void Ok_Failure_FallsThroughToDefaultValue()
        {
            Result<int, string> wrappedResult = new Failure<int, string>("Error");
            var guard = new ResultGuard<int, string, string>(wrappedResult);
            guard.Failure((when, @default) =>
            {
                when(i => i == string.Empty).Then("no match");
                when(i => i == string.Empty).Then("no match");
                @default("match");
            });
            var result = guard.Default(() => null);
            Assert.AreEqual(result, "match");
        }

        [Test]
        public void Ok_DoNotHandleSuccess()
        {
            Result<int, string> wrappedResult = new Success<int, string>(10);
            var guard = new ResultGuard<int, string, string>(wrappedResult);
            guard.DoNotHandleSuccess();
            var result = guard.Default(() => "expected");
            Assert.AreEqual(result, "expected");
        }

        [Test]
        public void Ok_DoNotHandleFailure()
        {
            Result<int, string> wrappedResult = new Failure<int, string>("Error");
            var guard = new ResultGuard<int, string, string>(wrappedResult);
            guard.DoNotHandleFailure();
            var result = guard.Default(() => "expected");
            Assert.AreEqual(result, "expected");
        }
    }
}
