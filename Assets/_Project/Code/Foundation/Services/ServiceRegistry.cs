using System;
using System.Collections.Generic;

namespace MaintGames.Bush.Foundation.Services
{
    /// <summary>
    /// Explicit, instance-based service container. Services are registered under their interface type while
    /// the composition root builds the game, then the registry is sealed. There is no static access.
    /// Violations (null, duplicate, late registration, missing service) are programming errors and throw.
    /// </summary>
    public sealed class ServiceRegistry : IServices
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        /// <summary>True after <see cref="Seal"/>. A sealed registry rejects further registrations.</summary>
        public bool IsSealed { get; private set; }

        /// <summary>Number of registered services.</summary>
        public int Count => _services.Count;

        /// <summary>Registers <paramref name="service"/> under <typeparamref name="T"/> (use the interface type).</summary>
        public void Register<T>(T service) where T : class
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (IsSealed)
            {
                throw new InvalidOperationException(
                    "Cannot register " + typeof(T).Name + ": the registry is sealed.");
            }

            if (_services.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException(typeof(T).Name + " is already registered.");
            }

            _services.Add(typeof(T), service);
        }

        /// <summary>Ends the composition phase. Services can still be read.</summary>
        public void Seal()
        {
            IsSealed = true;
        }

        public T Get<T>() where T : class
        {
            if (TryGet(out T service))
            {
                return service;
            }

            throw new InvalidOperationException(typeof(T).Name + " is not registered.");
        }

        public bool TryGet<T>(out T service) where T : class
        {
            if (_services.TryGetValue(typeof(T), out object found))
            {
                service = (T)found;
                return true;
            }

            service = null;
            return false;
        }
    }
}
