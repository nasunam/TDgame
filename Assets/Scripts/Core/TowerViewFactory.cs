using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using Game.Views;

namespace Game.Core
{
    public class TowerViewFactory
    {
        private readonly TowerView _prefab;
        private readonly Transform _root;
        private readonly Dictionary<int, TowerView> _views = new();
        public TowerViewFactory(TowerView prefab, Transform root)
        {
            _prefab = prefab;
            _root = root;
        }
        public void Spawn(TowerData data)
        {
            var v = Object.Instantiate(_prefab, _root);
            v.Bind(data);
            _views[data.Id] = v;
        }
        public void Remove(int id)
        {
            if (_views.TryGetValue(id, out var v))
            {
                Object.Destroy(v.gameObject);
                _views.Remove(id);
            }
        }
    }
}
