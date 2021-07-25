using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Inventory_Management
{
    public abstract class BindableBaseFody : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName, object before, object after)
        {
            SentOnPropertyChangedCallbacks(propertyName, before, after);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void NotifyPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region OnPropertyChangedCallback
        protected List<PropertyChangedCallbacksData> propertyChangedCallbackCollection = new List<PropertyChangedCallbacksData>();

        public void AddOnPropertyChangedCallback<T>(string propertyName, Action<T> callback)
            where T : class =>
            AddOnPropertyChangedCallback<T>(propertyName, (_, __, n) => callback(n));

        public void AddOnPropertyChangedCallback(string propertyName, Action callback) =>
            AddOnPropertyChangedCallback<object>(propertyName, (_, __, ___) => callback());


        public void AddOnPropertyChangedCallback<T>(string propertyName, Action<string, T, T> callback)
            where T : class
        {
            propertyChangedCallbackCollection.Add(new PropertyChangedCallbacksData(propertyName, (s, o, n) => callback(s, o as T, n as T)));
        }

        protected void SentOnPropertyChangedCallbacks(string propertyName, object before, object after)
        {
            if (propertyChangedCallbackCollection.Count == 0) return;
            var itemsToSentCallbacks = propertyChangedCallbackCollection.Where(d => d.PropertyName == propertyName);
            if (itemsToSentCallbacks == null) return;
            foreach (var itemToSentCallback in itemsToSentCallbacks)
                itemToSentCallback.Callback(propertyName, before, after);
        }

        protected class PropertyChangedCallbacksData
        {
            public PropertyChangedCallbacksData(string propertyName, Action<string, object, object> callback)
            {
                PropertyName = propertyName;
                Callback = callback;
            }

            public string PropertyName { get; }
            public Action<string, object, object> Callback { get; }
        }
        #endregion
    }

    public class StandardCommand : ICommand
    {
        public virtual event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter) => canExceute != null ? canExceute(parameter) : true;

        public virtual void Execute(object parameter) => execute?.Invoke(parameter);

        protected Action<object> execute = null;
        protected Func<object, bool> canExceute = null;

        public StandardCommand(Action<object> execute, Func<object, bool> canExceute)
        {
            this.execute = execute;
            this.canExceute = canExceute;
        }

        public StandardCommand(Action<object> execute) : this(execute, null)
        {
        }
    }

    public class RelayCommand : StandardCommand
    {
        public override event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute) : base(execute)
        {
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExceute) : base(execute, canExceute)
        {
        }
    }
}
