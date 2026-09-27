using Game.Config;
using Game.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class GhostFactory
    {
        private readonly Material _validMat;
        private readonly Material _invalidMat;
        private readonly Transform _root;
        public GhostFactory(Material validMat, Material invalidMat, Transform root)
        {
            _validMat = validMat;
            _invalidMat = invalidMat;
            _root = root;
        }
        public TowerGhostView Spawn(TowerConfigSO cfg, Vector3 pos)
        {
            // 幽灵用真模型 prefab，但剥掉所有 Collider，替换为幽灵材质
            var go = Object.Instantiate(cfg.ViewPrefab, _root);
            var view = go.GetComponent<TowerGhostView>();
            if (view == null) view = go.AddComponent<TowerGhostView>();
            // 剥掉Collider
            foreach (var c in go.GetComponentsInChildren<Collider>(true))
                Object.Destroy(c);
            view.Initialize(_validMat, _invalidMat);
            view.SetPosition(pos);
            return view;
        }
        public void Despawn(TowerGhostView ghost)
        {
            if (ghost != null) Object.Destroy(ghost.gameObject);
        }
    }
}