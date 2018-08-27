using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NotifyTesting
{
    public static class NotifyPropertyChangedExtensions
    {
        public static 
            PositiveExpectation<T> 
            ShouldNotifyFor<T, TProp>(
                this T subject,
                Expression<Func<T, TProp>> propertyExpression)
            where T : INotifyPropertyChanged
        {
            return new PositiveExpectation<T>(
                subject,
                ExpressionUtilities.GetPropertyName(propertyExpression)
                );
        }

        public static 
            NegativeExpectation<T>
            ShouldNotNotifyFor<T, TProp>(
                this T subject,
                Expression<Func<T, TProp>> propertyExpression)
            where T : INotifyPropertyChanged
        {
            return new NegativeExpectation<T>(
                subject, 
                new string[0], 
                new[] {
                    ExpressionUtilities.GetPropertyName(propertyExpression)
                });
        }
    }
}
