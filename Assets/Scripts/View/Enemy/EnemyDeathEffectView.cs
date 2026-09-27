using Game.Core;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Views
{
    /// <summary>
    /// 监听 EnemyKilledEvent，在死亡位置生成粒子特效。
    /// 方向：与子弹飞行反向（溅射向后）。
    /// 颜色：从敌人 Tint 拷。
    public class EnemyDeathEffectView : MonoBehaviour
    {
        [SerializeField] private GameLoop gameLoop;
        [SerializeField] private ParticleSystem prefab;
        [SerializeField] private Transform effectRoot;
        private GameContext Ctx => gameLoop != null ? gameLoop.Context : null;
        private void Awake()
        {
            if (gameLoop == null) gameLoop = FindFirstObjectByType<GameLoop>();
        }
        private void OnEnable()
        {
            EventBus.Subscribe<EnemyKilledEvent>(OnKilled);
        }
        private void OnDisable()
        {
            EventBus.Unsubscribe<EnemyKilledEvent>(OnKilled);
        }
        private void OnKilled(EnemyKilledEvent e)
        {
            var ps = Instantiate(prefab, effectRoot);
            ps.transform.position = e.Position;
            Debug.Log($"Death at {e.Position}, Tint={e.Tint}, Dir={e.ImpactDir}");
            // 溅射方向: 反向对齐子弹
            if (e.ImpactDir.sqrMagnitude > 0.001f)
                ps.transform.rotation = Quaternion.LookRotation(-e.ImpactDir, Vector3.up);
            var main = ps.main;
            main.startColor = new ParticleSystem.MinMaxGradient(e.Tint);
            ps.Play();
            Destroy(ps.gameObject, main.duration + main.startLifetime.constantMax);
        }
    }
}
