namespace System1Group.Lib.Result.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using System1Group.Lib.Result;

    [TestFixture]
    public class Result_Pack_Tests
    {
        private enum FailureType
        {
            Failed
        }

        [Test]
        public void Ok2_Success()
        {
            var results = this.GetResults().Take(2).ToList();

            Assert.That(results[0].Pack(results[1]).Unwrap(), Is.EqualTo((results[0].Unwrap(), results[1].Unwrap())));
        }

        [Test]
        public void Ok3_Success()
        {
            var results = this.GetResults().Take(3).ToList();

            Assert.That(results[0].Pack(results[1], results[2]).Unwrap(), Is.EqualTo((results[0].Unwrap(), results[1].Unwrap(), results[2].Unwrap())));
        }

        [Test]
        public void Ok4_Success()
        {
            var results = this.GetResults().Take(4).ToList();

            Assert.That(
                results[0].Pack(results[1], results[2], results[3]).Unwrap(),
                Is.EqualTo((results[0].Unwrap(), results[1].Unwrap(), results[2].Unwrap(), results[3].Unwrap())));
        }

        [Test]
        public void Ok5_Success()
        {
            var results = this.GetResults().Take(5).ToList();

            Assert.That(
                results[0].Pack(results[1], results[2], results[3], results[4]).Unwrap(),
                Is.EqualTo((results[0].Unwrap(), results[1].Unwrap(), results[2].Unwrap(), results[3].Unwrap(), results[4].Unwrap())));
        }

        [Test]
        public void Ok6_Success()
        {
            var results = this.GetResults().Take(6).ToList();

            Assert.That(
                results[0].Pack(results[1], results[2], results[3], results[4], results[5]).Unwrap(),
                Is.EqualTo((results[0].Unwrap(), results[1].Unwrap(), results[2].Unwrap(), results[3].Unwrap(), results[4].Unwrap(), results[5].Unwrap())));
        }

        [Test]
        public void Ok7_Success()
        {
            var results = this.GetResults().Take(7).ToList();

            Assert.That(
                results[0].Pack(results[1], results[2], results[3], results[4], results[5], results[6]).Unwrap(),
                Is.EqualTo(
                    (results[0].Unwrap(), results[1].Unwrap(), results[2].Unwrap(), results[3].Unwrap(), results[4].Unwrap(), results[5].Unwrap(), results[6]
                        .Unwrap())));
        }

        [Test]
        public void Ok8_Success()
        {
            var results = this.GetResults().Take(8).ToList();

            Assert.That(
                results[0].Pack(results[1], results[2], results[3], results[4], results[5], results[6], results[7]).Unwrap(),
                Is.EqualTo(
                    (results[0].Unwrap(), results[1].Unwrap(), results[2].Unwrap(), results[3].Unwrap(), results[4].Unwrap(), results[5].Unwrap(),
                    results[6].Unwrap(), results[7].Unwrap())));
        }

        [Test]
        public void Ok9_Success()
        {
            var results = this.GetResults().Take(9).ToList();

            Assert.That(
                results[0].Pack(results[1], results[2], results[3], results[4], results[5], results[6], results[7], results[8]).Unwrap(),
                Is.EqualTo(
                    (results[0].Unwrap(), results[1].Unwrap(), results[2].Unwrap(), results[3].Unwrap(), results[4].Unwrap(), results[5].Unwrap(),
                    results[6].Unwrap(), results[7].Unwrap(), results[8].Unwrap())));
        }

        [Test]
        public void Ok10_Success()
        {
            var results = this.GetResults().Take(10).ToList();

            Assert.That(
                results[0].Pack(results[1], results[2], results[3], results[4], results[5], results[6], results[7], results[8], results[9]).Unwrap(),
                Is.EqualTo(
                    (results[0].Unwrap(), results[1].Unwrap(), results[2].Unwrap(), results[3].Unwrap(), results[4].Unwrap(), results[5].Unwrap(),
                    results[6].Unwrap(), results[7].Unwrap(), results[8].Unwrap(), results[9].Unwrap())));
        }

        [Test]
        public void Ok2_Failure()
        {
            var results = this.GetResults().Take(1).ToList();
            var failure = Result.Failure<int, FailureType>(FailureType.Failed);

            Assert.That(results[0].Pack(failure).UnwrapError(), Is.EqualTo(FailureType.Failed));
        }

        private IEnumerable<Result<int, FailureType>> GetResults()
        {
            var rng = new Random();
            while (true)
            {
                yield return Result.Success<int, FailureType>(rng.Next());
            }
        }
    }
}
