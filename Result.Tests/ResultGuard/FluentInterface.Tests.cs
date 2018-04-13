namespace System1Group.Lib.Result.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ResultGuard_FluentInterface_Tests
    {
        [Test]
        public void Ok_Full()
        {
            Result<int, string> wrappedResult = new Success<int, string>(10);
            IGuardEntryPoint<int, string, string> guard = new ResultGuard<int, string, string>(wrappedResult);

            var result = guard.Success(
                    when =>
                    {
                        when(i => i == 5).Then("It's five!");
                        when(i => i % 2 == 0).Then(i => i + " is even!");
                        when(i => i < 0).Then(i => i + " is negative!");
                    })
                .Failure(
                    (when, @default) =>
                    {
                        when(i => i.Length > 100).Then("Error message is too long!");
                        when(i => i.StartsWith("Fatal error")).Then(i => "A fatal error occurred! " + i);
                        @default(i => "Error: " + i);
                    })
                .Default("Who knows what happened???");

            Assert.AreEqual(result, "10 is even!");
        }
    }
}
