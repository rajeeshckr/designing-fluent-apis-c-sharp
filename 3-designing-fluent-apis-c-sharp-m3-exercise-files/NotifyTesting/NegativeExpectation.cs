using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace NotifyTesting
{
    public sealed class NegativeExpectation<T>
        : PropertyChangedExpectation<T>
        where T : INotifyPropertyChanged
    {
        public NegativeExpectation(
                T subject,
                IEnumerable<string> expected,
                IEnumerable<string> notExpected
            )
            : base(subject, expected, notExpected)
        {

        }

        public NegativeExpectation<T> 
            Nor<TProp>(
                Expression<Func<T, TProp>> propertyExpression
            )
        {
            return new NegativeExpectation<T>(
                _subject,
                _expectedProps,
                _notExpected.Concat(new[] {
                    ExpressionUtilities.GetPropertyName(propertyExpression)
                    })
                );
        }
    }
}