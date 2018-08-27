using NUnit.Framework;

namespace NotifyTesting.Tests
{
    [TestFixture]
    public sealed class ExclusiveTests
    {
        [Test]
        public void SingleProperty_AssertFail()
        {
            var person = new Person();

            var ex = Assert.Throws<AssertionException>(() =>
                person.ShouldNotifyFor(x => x.FirstName)
                    .AndNothingElse()
                    .When(() => person.FirstName = "John")
            );

            var expectedMessage = "Received notifications: FirstName, FullName. " +
                "Expected FirstName and nothing else.";

            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void SingleProperty_AssertPass()
        {
            var person = new Person();

            person.ShouldNotifyFor(x => x.BirthDate)
                .AndNothingElse()
                .When(() => person.BirthDate = new System.DateTime(2011, 11, 8));
        }

    }
}
