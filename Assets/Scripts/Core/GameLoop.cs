using Game.Config;
using Game.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    [DefaultExecutionOrder(-1000)]
    public class GameLoop : MonoBehaviour
    {

        [Header("Prefabs")]
        [SerializeField] private EnemyView enemyPrefab;
        [SerializeField] private ProjectileView projectilePrefab;
        [Header("Roots")]
        [SerializeField] private Transform enemyRoot;
        [SerializeField] private Transform towerRoot;
        [SerializeField] private Transform projectileRoot;
        [Header("Configs")]
        [SerializeField] private WaveConfigSO waveConfig;
        [SerializeField] private TowerCatalogSO towerCatalog;
        private GameContext _ctx;
        public GameContext Context => _ctx;
        private void Awake()
        {
            _ctx = new GameContext();
            _ctx.Init(enemyPrefab, enemyRoot,
                towerRoot,
                projectilePrefab, projectileRoot,
                waveConfig,
                towerCatalog);
        }
        private void Update() => _ctx?.Tick(Time.deltaTime);
        private void OnDestroy() => _ctx?.Dispose();
    }
}
