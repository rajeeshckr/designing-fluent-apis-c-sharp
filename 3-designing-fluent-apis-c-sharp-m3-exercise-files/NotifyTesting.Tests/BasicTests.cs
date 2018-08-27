using NUnit.Framework;
using System;

namespace NotifyTesting.Tests
{
    [TestFixture]
    public sealed class BasicTests
    {
        [Test]
        public void SingleProperty_AssertFail()
        {
            var person = new Person();

            var ex = Assert.Throws<AssertionException>(() =>

                person.ShouldNotifyFor(x => x.FirstName)
                    .When(() => { /* do nothing */ })
            
            );

            Assert.That(
                ex.Message,
                Is.EqualTo("Received notifications: (none). Expected FirstName."));
        }

        [Test]
        public void SingleProperty_AssertPass()
        {
            var person = new Person();

            person.ShouldNotifyFor(x => x.FirstName)
                .When(() => person.FirstName = "John");
        }

        [Test]
        public void MultipleProperties_SingleNotification_AssertFail()
        {
            var person = new Person();

            var ex = Assert.Throws<AssertionException>(() =>
                person.ShouldNotifyFor(x => x.FirstName)
                    .And(x => x.LastName)
                    .When(() => person.FirstName = "John")
            );

            var expectedMessage = "Received notifications: FirstName, FullName." +
                " Expected FirstName, LastName.";

            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void MultipleProperties_NoNotifications_AssertFail()
        {
            var person = new Person();

            var ex = Assert.Throws<AssertionException>(() =>
                person.ShouldNotifyFor(x => x.FirstName)
                    .And(x => x.LastName)
                    .When(() => { /* nothing happens */ })
            );

            var expectedMessage = "Received notifications: (none)." +
                " Expected FirstName, LastName.";

            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void MultipleProperties_AssertPass()
        {
            var person = new Person();

            person.ShouldNotifyFor(x => x.FirstName)
                .And(x => x.FullName)
                .When(() => person.FirstName = "John");
        }

        [Test]
        public void NegativeTest_AssertFail()
        {
            var person = new Person();

            var ex = Assert.Throws<AssertionException>(() =>
                person.ShouldNotNotifyFor(x => x.FirstName)
                    .When(() => person.FirstName = "John")
            );
        }

        [Test]
        public void NegativeTest_AssertPass()
        {
            var person = new Person();

            person.ShouldNotNotifyFor(x => x.LastName)
                .When(() => person.FirstName = "John");
        }
    }
}
