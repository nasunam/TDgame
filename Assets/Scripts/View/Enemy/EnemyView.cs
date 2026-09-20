using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

namespace Game.Views
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Transform hpBar;
        private EnemyData _data;
        public void Bind(EnemyData data)
        {
            _data = data;
            transform.position = data.Position;
            UpdateHpBar();
        }
        private void LateUpdate()
        {
            if (_data == null) return;
            transform.position = _data.Position;
            if (_data.Forward.sqrMagnitude > 0.0001f)
            {
                // 让模型朝向路径方向（Y 轴固定，避免抖动）
                var dir = _data.Forward;
                dir.y = 0f;
                if (dir.sqrMagnitude > 0.0001f)
                {
                    transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
                }
            }
            UpdateHpBar();
        }
        private void UpdateHpBar()
        {
            if (hpBar == null || _data == null) return;
            float pct = _data.MaxHp <= 0f ? 0f : Mathf.Clamp01(_data.Hp / _data.MaxHp);
            var s = hpBar.localScale;
            s.x = pct;
            hpBar.localScale = s;
        }
    }
}
