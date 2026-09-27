using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Views
{
    /// <summary>
    /// 幽灵预览：位置跟随 + 材质切换（绿/红）。
    /// </summary>
    public class TowerGhostView : MonoBehaviour
    {
        private Material _validMat;
        private Material _invalidMat;
        private Renderer[] _renderers;
        private bool _isValid;
        public void Initialize(Material validMat, Material invalidMat)
        {
            _validMat = validMat;
            _invalidMat = invalidMat;
            // 用SharedMaterial避免每次实例化
            _renderers = GetComponentsInChildren<Renderer>(true);
            _isValid = false;
            ApplyMaterial(false);
        }
        public void SetPosition(Vector3 pos) => transform.position = pos;
        public void SetValid(bool valid)
        {
            if (_isValid == valid) return;
            _isValid = valid;
            ApplyMaterial(true);
        }
        private void ApplyMaterial(bool valid)
        {
            if (_renderers == null) return;
            var mat = _isValid ? _validMat : _invalidMat;
            foreach (var r in _renderers)
            {
                if (r == null) continue;
                var mats = new Material[r.sharedMaterials.Length];
                for (int i = 0; i < mats.Length; i++) mats[i] = mat;
                r.sharedMaterials = mats;
            }
        }
    }
}
