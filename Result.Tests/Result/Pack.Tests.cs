namespace System1Group.Lib.Result.Tests
{
    using NUnit.Framework;
    using System1Group.Lib.Result;

    [TestFixture]
    public class Result_Pack_Tests
    {
        private enum FailureType
        {
            Var
        }

        [Test]
        public void Ok2_Success()
        {
            var result1 = Result.Success<string, FailureType>("some_string");
            var result2 = Result.Success<int, FailureType>(2);

            Assert.That(result1.Pack(result2).Unwrap(), Is.EqualTo(("some_string", 2)));
        }
    }
}
