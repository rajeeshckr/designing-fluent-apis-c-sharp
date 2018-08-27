using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace NotifyTesting
{
    public abstract class PropertyChangedExpectation<T> 
        where T : INotifyPropertyChanged
    {
        protected readonly IEnumerable<string> _expectedProps;
        protected readonly IEnumerable<string> _notExpected;
        protected readonly T _subject;

        protected PropertyChangedExpectation(
            T subject,
            IEnumerable<string> expected,
            IEnumerable<string> notExpected)
        {
            _subject = subject;
            _expectedProps = expected;
            _notExpected = notExpected;

            var conflicts = _expectedProps.Intersect(_notExpected);

            if (conflicts.Any())
                throw new ArgumentException("Cannot specify properties for both "
                + "positive and negative verification. Conflicting properties: " +
                FormatNames(conflicts)
                );
        }

        public void When(Action action)
        {
            var notifications = new List<string>();
            _subject.PropertyChanged += (o, e) => notifications.Add(e.PropertyName);

            action();

            var metExpectations = _expectedProps.Intersect(notifications)
                .ToArray();

            var unmetExpectations = _expectedProps.Where(x => !notifications.Contains(x));
            var unexpectedNotifications = _notExpected.Intersect(notifications);

            if (!unmetExpectations.Any() && !unexpectedNotifications.Any())
                return;

            var receivedText = notifications.Any()
                ? FormatNames(notifications) 
                : "(none)";

            var msg = string.Format("Received notifications: {0}." +
                " Expected {1}", // + " but not {2}.",
                receivedText,
                FormatNames(_expectedProps),
                FormatNames(_notExpected)
                );

            if (_notExpected.Any())
                msg += " but not " + FormatNames(_notExpected) + ".";
            else
                msg += ".";

            Assert.Fail(msg);
        }

        private string FormatNames(IEnumerable<string> propertyNames)
        {
            return string.Join(
                ", ",
                propertyNames
                );
        }
    }
}