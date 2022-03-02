#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices; // .NET 4.5 required.
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace INPC.Sample
{
    // source: https://stackoverflow.com/questions/1315621/implementing-inotifypropertychanged-does-a-better-way-exist

    #region NET40

    public class INPCNet40 : INotifyPropertyChanged
    {
        // boiler-plate
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        /*
        // props
        private string name;
        public string Name
        {
            get { return name; }
            set { SetField(ref name, value, "Name"); }
        }
        */
    }

    #endregion

    #region NET45

    public class INPCNet45 : INotifyPropertyChanged
    {
        // boiler-plate
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        /*
        // props
        private string name;
        public string Name
        {
            get { return name; }
            set { SetField(ref name, value); }
        }
        */
    }

    #endregion

    #region NET40 - LINQ

    public class INPCNet40LINQ : INotifyPropertyChanged
    {
        // boiler-plate
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        /*
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        */
        protected void OnPropertyChanged<T>(Expression<Func<T>> selectorExpression)
        {
            if (selectorExpression == null)
                throw new ArgumentNullException("selectorExpression");
            var me = selectorExpression.Body as MemberExpression;

            // Nullable properties can be nested inside of a convert function
            if (me == null)
            {
                var ue = selectorExpression.Body as UnaryExpression;
                if (ue != null)
                    me = ue.Operand as MemberExpression;
            }

            if (me == null)
                throw new ArgumentException("The body must be a member expression");

            OnPropertyChanged(me.Member.Name);
        }

        protected void SetField<T>(ref T field, T value, Expression<Func<T>> selectorExpression, 
            params Expression<Func<object>>[] additonal)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) 
                return;
            field = value;
            OnPropertyChanged(selectorExpression);
            foreach (var item in additonal)
                OnPropertyChanged(item);
        }
    }

    #endregion

    #region NET40 - LINQv2

    #endregion

    #region INotifyPropertyChanged Extensions v1
    /*
    public static class INotifyPropertyChangedExtensions
    {
        public static bool SetPropertyAndNotify<T>(this INotifyPropertyChanged sender,
                   PropertyChangedEventHandler handler, ref T field, T value,
                   [CallerMemberName] string propertyName = "",
                   EqualityComparer<T> equalityComparer = null)
        {
            var eqComp = equalityComparer ?? EqualityComparer<T>.Default;
            bool hasChanged = false;
            if (!eqComp.Equals(field, value))
            {
                field = value;
                hasChanged = true;
                if (handler != null)
                {
                    var args = new PropertyChangedEventArgs(propertyName);
                    handler(sender, args);
                }
            }
            return hasChanged;
        }
    }
    */
    #endregion

    #region INotifyPropertyChanged Extensions v2

    public static class INotifyPropertyChangedExtensions
    {
        public class PropertyChangedAction<T>
        {
            private PropertyChangedAction() : base() { }
            internal PropertyChangedAction(INotifyPropertyChanged sender, 
                string propertyName, 
                T oldValue, T newValue,
                bool hasChanged = false) : this() 
            {
                Sender = sender;
                PropertyName = propertyName;
                OldValue = oldValue;
                NewValue = newValue;
                HasChanged = hasChanged;
            }

            public INotifyPropertyChanged Sender { get; private set; }
            public string PropertyName { get; private set; }
            public bool HasChanged { get; internal set; }

            public T OldValue { get; private set; }
            public T NewValue { get; private set; }

            public PropertyChangedAction<T> Then(Action<PropertyChangedAction<T>> action)
            {
                if (null != Sender && HasChanged && !string.IsNullOrWhiteSpace(PropertyName))
                {
                    if (null != action) action(this);
                }
                return this;
            }

            public PropertyChangedAction<T> Raise(PropertyChangedEventHandler handler)
            {
                if (null != Sender && HasChanged && !string.IsNullOrWhiteSpace(PropertyName))
                {
                    if (null != handler)
                    {
                        handler(Sender, new PropertyChangedEventArgs(PropertyName));
                    }
                }
                return this;
            }
        }

        public static PropertyChangedAction<T> IfChanged<T>(this INotifyPropertyChanged sender,
            ref T field, T value,
            [CallerMemberName] string propertyName = "")
        {
            var eqComp = EqualityComparer<T>.Default;
            PropertyChangedAction<T> action = new PropertyChangedAction<T>(sender, propertyName, field, value, false);
            if (!eqComp.Equals(field, value))
            {
                field = value;
                action.HasChanged = true; // set flag.
            }
            return action;
        }
    }

    #endregion
}
