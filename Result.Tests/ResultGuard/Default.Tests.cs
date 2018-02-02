namespace System1Group.Lib.Result.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ResultGuard_Default_Tests
    {
        [Test]
        public void Ok_Func()
        {
            Result<int, string> wrappedResult = new Success<int, string>(10);
            var guard = new ResultGuard<int, string, int>(wrappedResult);
            var result = guard.Default(() => 20);
            Assert.AreEqual(result, 20);
        }

        [Test]
        public void Ok_Value()
        {
            Result<int, string> wrappedResult = new Failure<int, string>("Error");
            var guard = new ResultGuard<int, string, int>(wrappedResult);
            var result = guard.Default(20);
            Assert.AreEqual(result, 20);
        }
    }
}
