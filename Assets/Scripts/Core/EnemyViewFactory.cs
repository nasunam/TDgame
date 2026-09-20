using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using Game.Views;

namespace Game.Core
{
    public class EnemyViewFactory
    {
        private readonly EnemyView _prefab;
        private readonly Transform _root;
        private readonly Dictionary<int, EnemyView> _views = new();
        public EnemyViewFactory(EnemyView prefab, Transform root)
        {
            _prefab = prefab;
            _root = root;
        }
        public void Spawn(EnemyData data)
        {
            var view = Object.Instantiate(_prefab, _root);
            view.Bind(data);
            _views[data.id] = view;
        }
        public void Remove(int enemyId)
        {
            if (_views.TryGetValue(enemyId, out var view))
            {
                Object.Destroy(view.gameObject);
                _views.Remove(enemyId);
            }
        }
        public EnemyView Get(int id) => _views.TryGetValue(id, out var v) ? v : null;
    }
}
