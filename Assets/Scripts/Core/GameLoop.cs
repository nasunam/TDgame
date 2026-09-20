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
        [SerializeField] private TowerView towerPrefab;
        [SerializeField] private ProjectileView projectilePrefab;
        [Header("Roots")]
        [SerializeField] private Transform enemyRoot;
        [SerializeField] private Transform towerRoot;
        [SerializeField] private Transform projectileRoot;
        [Header("Configs")]
        [SerializeField] private WaveConfigSO waveConfig;
        private GameContext _ctx;
        public GameContext Context => _ctx;
        private void Awake()
        {
            _ctx = new GameContext();
            _ctx.Init(enemyPrefab, enemyRoot, towerPrefab, towerRoot, projectilePrefab, projectileRoot, waveConfig);
        }
        public void Update()
        {
            _ctx.Tick(Time.deltaTime);
        }
        private void OnDestroy()
        {
            _ctx?.Dispose();
        }
    }
}
