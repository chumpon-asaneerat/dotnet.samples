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

    #region NET40v2

    public class INPCNet40v2 : INotifyPropertyChanged
    {
        public class PropertyChangedAction<T>
        {
            private PropertyChangedAction() : base() { }
            internal PropertyChangedAction(INPCNet40v2 sender,
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

            public INPCNet40v2 Sender { get; private set; }
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

            public PropertyChangedAction<T> Raise()
            {
                if (null != Sender && HasChanged && !string.IsNullOrWhiteSpace(PropertyName))
                {
                    Sender.OnPropertyChanged(PropertyName);
                }
                return this;
            }

            public PropertyChangedAction<T> Raise(string propertyName)
            {
                if (null != Sender && HasChanged && !string.IsNullOrWhiteSpace(propertyName))
                {
                    Sender.OnPropertyChanged(propertyName);
                }
                return this;
            }
            public void Raise(params Expression<Func<object>>[] actions)
            {
                if (null != Sender && null != actions && actions.Length > 0)
                {
                    Sender.Raise(actions);
                }
            }
        }


        // boiler-plate
        public event PropertyChangedEventHandler PropertyChanged;

        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected internal void OnPropertyChanged<T>(Expression<Func<T>> selectorExpression)
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

        public void Raise(params Expression<Func<object>>[] actions)
        {
            if (null != actions && actions.Length > 0)
            {
                foreach (var item in actions)
                    OnPropertyChanged(item);
            }
        }

        public PropertyChangedAction<T> IfChanged<T>(ref T field, T value,
            [CallerMemberName] string propertyName = "")
        {
            var eqComp = EqualityComparer<T>.Default;
            PropertyChangedAction<T> action = new PropertyChangedAction<T>(this, propertyName, field, value, false);
            if (!eqComp.Equals(field, value))
            {
                field = value;
                action.HasChanged = true; // set flag.
            }
            return action;
        }
    }


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

    #region DMT production v5

    #region NInpc

    /// <summary>
    /// The NInpc abstract class.
    /// Provide basic implementation of INotifyPropertyChanged interface.
    /// </summary>
    public abstract class NInpc : INotifyPropertyChanged
    {
        #region Internal Variables

        private bool _lock = false;

        #endregion

        #region Private Methods

        /// <summary>
        /// Internal Raise Property Changed event (Lamda function).
        /// </summary>
        /// <param name="selectorExpression">The Expression function.</param>

        private void InternalRaise<T>(Expression<Func<T>> selectorExpression)
        {
            if (_lock) return; // if lock do nothing
            if (null == selectorExpression)
            {
                throw new ArgumentNullException("selectorExpression");
                // return;
            }
            var me = selectorExpression.Body as MemberExpression;

            // Nullable properties can be nested inside of a convert function
            if (null == me)
            {
                var ue = selectorExpression.Body as UnaryExpression;
                if (null != ue)
                {
                    me = ue.Operand as MemberExpression;
                }
            }

            if (null == me)
            {
                throw new ArgumentException("The body must be a member expression");
                // return;
            }
            Raise(me.Member.Name);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Enable Notify Change Event.
        /// </summary>
        public void EnableNotify()
        {
            _lock = true;
        }
        /// <summary>
        /// Disable Notify Change Event.
        /// </summary>
        public void DisableNotify()
        {
            _lock = false;
        }
        /// <summary>
        /// Raise Property Changed event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        public void Raise(string propertyName)
        {
            if (_lock) return; // if lock do nothing
            // raise event.
            //PropertyChanged.Call(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Raise Property Changed event (Lamda function).
        /// </summary>
        /// <param name="actions">The array of lamda expression's functions.</param>
        public void Raise(params Expression<Func<object>>[] actions)
        {
            if (_lock) return; // if lock do nothing
            if (null != actions && actions.Length > 0)
            {
                foreach (var item in actions)
                {
                    if (null != item) InternalRaise(item);
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Checks is notifiy enabled or disable.
        /// </summary>
        /// <returns>Returns true if enabled.</returns>
        public bool IsLocked { get { return _lock; } set { } }

        #endregion

        #region Public Events

        /// <summary>
        /// The PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    #endregion


    #endregion
}
