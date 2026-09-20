using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _handlers = new();
        public static void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            _handlers[type] = _handlers.TryGetValue(type, out var d)
                ? Delegate.Combine(d, handler)
                : handler;
        }
        public static void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!_handlers.TryGetValue(type, out var existing)) return;
            var newD = Delegate.Remove(existing, handler);
            if (newD == null) _handlers.Remove(type);
            else _handlers[type] = newD;
        }
        public static void Publish<T>(T evt)
        {
            if (_handlers.TryGetValue(typeof(T), out var d))
                (d as Action<T>)?.Invoke(evt);
        }
        public static void Clear() => _handlers.Clear();
    }
}
