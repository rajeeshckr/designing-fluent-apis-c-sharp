using NUnit.Framework;
using System;

namespace NotifyTesting.Tests
{
    [TestFixture]
    public sealed class PositiveNegativeTests
    {
        [Test]
        public void PositivePlusNegative_AssertFailed()
        {
            var person = new Person();

            var ex = Assert.Throws<AssertionException>(() =>
                person.ShouldNotifyFor(x => x.FirstName)
                    .ButNot(x => x.FullName)
                    .When(() => person.FirstName = "John")
            );

            var expectedMessage = "Received notifications: FirstName, FullName. " +
                "Expected FirstName but not FullName.";
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void PositivePlusNegative_AssertPass()
        {
            var person = new Person();

            person.ShouldNotifyFor(x => x.FirstName)
                .ButNot(x => x.LastName)
                .When(() => person.FirstName = "John");
        }

        [Test]
        public void PositivePlusMultipleNegative_AssertFail()
        {
            var person = new Person();

            var ex = Assert.Throws<AssertionException>(() =>
                person.ShouldNotifyFor(x => x.FirstName)
                    .ButNot(x => x.LastName)
                        .Nor(x => x.FullName)
                        .When(() => {
                            person.LastName = "Smith";
                            person.FirstName = "Joe";
                        })
            );

            var expected = "Received notifications: LastName, FullName, FirstName, FullName. " +
                "Expected FirstName but not LastName, FullName.";

            Assert.That(ex.Message, Is.EqualTo(expected));
        }

        [Test]
        public void Nor_AssertPass()
        {
            var person = new Person();

            person.ShouldNotifyFor(x => x.BirthDate)
                .ButNot(x => x.FirstName)
                    .Nor(x => x.LastName)
                .When(() => person.BirthDate = new DateTime(2008, 8, 15));
        }
    }
}
