using System;
using System.Collections.Generic;

using UnityEngine;

namespace GGJ2022 {
    public interface IDependencyResolver {
        void Register<T>(object obj);
        T Resolve<T>();
        void Dispose<T>(object obj);
    }

    public sealed class DependencyManager : IDependencyResolver {
        private readonly Dictionary<Type, object> _types = new Dictionary<Type, object>();

        public T Resolve<T>() => (T) _types[typeof(T)];

        public void Register<T>(object obj) {
            object targetObject = null;

            
            
            if (obj is GameObject) {
                var sourceGameObject = (GameObject) obj;
                if (sourceGameObject.gameObject.TryGetComponent(typeof(T), out var sourceComponent)) {
                    targetObject = sourceComponent;
                }
            }
            else {

                // validates implementation type
                if (obj is T == false)
                    throw new InvalidOperationException(
                        $"The supplied instance does not implement {typeof(T).FullName}");
                targetObject = obj;
            }

            // validates unique implementations
            if (_types.ContainsKey(typeof(T)))
                //throw new DuplicateNameException($"{typeof(T).FullName} has already been implemented.");
                Debug.LogWarning($"{typeof(T).FullName} has already been implemented.");

            if (targetObject == null)
                throw new InvalidOperationException(
                    $"{typeof(T).FullName} can not be identified from obj: {obj}.");

            _types.Add(typeof(T), (T) targetObject);
        }

        public void Dispose<T>(object obj) {
            if (_types.ContainsKey(typeof(T))) {
                _types.Remove(typeof(T));
            }
        }
    }
}