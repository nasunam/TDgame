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
        [SerializeField] private Transform ghostRoot;
        [Header("Configs")]
        [SerializeField] private WaveConfigSO waveConfig;
        [SerializeField] private TowerCatalogSO towerCatalog;
        [Header("Camera")]
        [SerializeField] private Camera gameCamera;
        [Header("LayerMask")]
        [SerializeField] private LayerMask groundLayer;
        [Header("Material")]
        [SerializeField] private Material ghostValidMat;
        [SerializeField] private Material ghostInvalidMat;
        private GameContext _ctx;
        public GameContext Context => _ctx;
        private void Awake()
        {
            _ctx = new GameContext();
            _ctx.Init(
                enemyPrefab, enemyRoot,
                projectilePrefab, projectileRoot,
                towerRoot, ghostRoot,
                waveConfig, towerCatalog,
                ghostValidMat, ghostInvalidMat,
                gameCamera, groundLayer);
        }
        private void Update() => _ctx?.Tick(Time.deltaTime);
        private void OnDestroy() => _ctx?.Dispose();
    }
}
