using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace NotifyTesting
{
    public class PositiveExpectation<T>
        : PropertyChangedExpectation<T>
        where T : INotifyPropertyChanged
    {

        public PositiveExpectation(T subject, params string[] propertyNames)
            : base(subject, propertyNames, new string[0])
        {
        }

        public PositiveExpectation<T> And<TProp>(Expression<Func<T, TProp>> propertyExpression)
        {
            var newPropertyNames = _expectedProps
                .Concat(new[] {
                    ExpressionUtilities.GetPropertyName(propertyExpression)
                })
                .ToArray();

            return new PositiveExpectation<T>(
                _subject,
                newPropertyNames
                );
        }

        public NegativeExpectation<T> ButNot<TProp>(Expression<Func<T, TProp>> propertyExpression)
        {
            return new NegativeExpectation<T>(
                _subject,
                _expectedProps,
                new[] { ExpressionUtilities.GetPropertyName(propertyExpression) }
                );
        }

        public ExclusiveExpectation<T> AndNothingElse()
        {
            return new ExclusiveExpectation<T>(_subject, _expectedProps);
        }
    }
}
