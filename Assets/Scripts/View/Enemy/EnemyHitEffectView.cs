using Game.Core;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Views
{
    public class EnemyHitEffectView : MonoBehaviour
    {
        [SerializeField] private GameLoop gameLoop;
        [SerializeField] private ParticleSystem prefab;
        [SerializeField] private Transform effectRoot;
        private void Awake()
        {
            if (gameLoop == null) gameLoop = FindFirstObjectByType<GameLoop>();
        }
        private void OnEnable()
        {
            EventBus.Subscribe<EnemyHitEvent>(OnHit);
        }
        private void OnDisable()
        {
            EventBus.Unsubscribe<EnemyHitEvent>(OnHit);
        }
        private void OnHit(EnemyHitEvent e)
        {
            var ps = Instantiate(prefab, effectRoot);
            ps.transform.position = e.Position;
            if (e.ImpactDir.sqrMagnitude > 0.001f)
                ps.transform.rotation = Quaternion.LookRotation(-e.ImpactDir, Vector3.up);
            ps.Play();
            Destroy(ps.gameObject, 1f);
        }
    }
}
