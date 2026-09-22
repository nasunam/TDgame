using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using Game.Views;

namespace Game.Core
{
    public class TowerViewFactory
    {
        private readonly Transform _root;
        private readonly Dictionary<int, TowerView> _views = new();
        public TowerViewFactory(Transform root)
        {
            _root = root;
        }
        public TowerView Spawn(TowerData data)
        {
            var prefab = data.Config.ViewPrefab;
            if (prefab == null)
            {
                Debug.LogError($"[TowerViewFactory] {data.Config.Id} 没有配置 ViewPrefab");
                return null;
            }
            var go = Object.Instantiate(prefab, _root);
            var view = go.GetComponent<TowerView>();
            if (view == null)
            {
                Debug.LogError($"[TowerViewFactory] {prefab.name} 缺少 TowerView 组件");
                Object.Destroy(go);
                return null;
            }
            view.Bind(data);
            _views[data.Id] = view;
            return view;
        }
        public TowerView Get(int id) => _views.TryGetValue(id, out var v) ? v : null;
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
