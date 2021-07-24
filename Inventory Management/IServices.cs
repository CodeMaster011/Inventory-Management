using System;
using System.Collections.Generic;

namespace Inventory_Management
{
    public interface IServices
    {
        void Register<T>(T service) where T : class;

        T GetService<T>() where T : class;

        T GetServiceHard<T>() where T : class;
    }

    public class DefaultServices : IServices
    {
        protected Dictionary<string, object> Dictionary = new Dictionary<string, object>();

        public virtual T GetService<T>() where T : class
        {
            var key = GetKey(typeof(T));
            if (!Dictionary.ContainsKey(key)) return null;
            return Dictionary[key] as T;
        }

        public virtual T GetServiceHard<T>() where T : class
        {
            var service = GetService<T>();
            return service ?? throw new InvalidOperationException(nameof(T));
        }

        public virtual void Register<T>(T service) where T : class
        {
            var key = GetKey(typeof(T));
            if (Dictionary.ContainsKey(key))
                throw new InvalidProgramException("Service is already registered");

            Dictionary.Add(key, service);
        }

        protected virtual string GetKey(Type type) => type.FullName;
    }
}
