using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace NotifyTesting.Tests
{
    public sealed class Person : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _firstName;
        private string _lastName;
        private DateTime _birthDate; 

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(() => FirstName);
                OnPropertyChanged(() => FullName);
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(() => LastName);
                OnPropertyChanged(() => FullName);
            }
        }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
        
        public DateTime BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                OnPropertyChanged(() => BirthDate);
            }
        }

        private void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (PropertyChanged == null)
                return;

            var propertyName = ((MemberExpression)propertyExpression.Body)
                .Member.Name;

            var args = new PropertyChangedEventArgs(propertyName);

            PropertyChanged(this, args);
        }
    }
}
