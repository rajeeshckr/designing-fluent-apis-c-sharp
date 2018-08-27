using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NotifyTesting
{
    public sealed class ExclusiveExpectation<T>
        where T : INotifyPropertyChanged
    {
        private readonly T _subject;
        private readonly IEnumerable<string> _expectedProperties;

        public ExclusiveExpectation(T subject, IEnumerable<string> expectedProperties)
        {
            _subject = subject;
            _expectedProperties = expectedProperties;
        }

        public void When(Action action)
        {
            var notifications = new List<string>();

            _subject.PropertyChanged += (o, e) => notifications.Add(e.PropertyName);

            action();

            var unexpected = notifications.Except(_expectedProperties);

            if (!unexpected.Any())
                return;

            var message = string.Format(
                "Received notifications: {0}. " +
                "Expected {1} and nothing else.",
                string.Join(", ", notifications),
                string.Join(", ", _expectedProperties)
                );

            Assert.Fail(message);
        }
    }
}
