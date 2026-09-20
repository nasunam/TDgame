using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Models;
using Game.Services;
using Game.Systems;
using Game.Views;
using Game.Config;

namespace Game.Core
{
    public class GameContext
    {
        public static GameContext Current { get; private set; }
        // 1, Models
        public EnemyModel EnemyModel;
        public CoreModel CoreModel;
        public EconomyModel EconomyModel;
        public GameStateModel GameStateModel;
        public TowerModel TowerModel;
        public ProjectileModel ProjectileModel;
        // 2. Services
        public PathService PathService;
        public TowerConfigService TowerConfigService;
        // 3, View Factory
        public EnemyViewFactory EnemyViewFactory;
        public TowerViewFactory TowerViewFactory;
        public ProjectileViewFactory ProjectileViewFactory;
        // 4. Systems
        public EnemySystem EnemySystem;
        public WaveSystem WaveSystem;
        public CoreSystem CoreSystem;
        public TowerSystem TowerSystem;
        public ProjectileSystem ProjectileSystem;
        public EconomySystem EconomySystem;
        public BuildSystem BuildSystem;
        // 初始化
        public void Init(
            EnemyView enemyPrefab, Transform enemyRoot,
            TowerView towerPrefab, Transform towerRoot,
            ProjectileView projectilePrefab, Transform projectileRoot,
            WaveConfigSO waveConfig)
        {
            Current = this;
            // Models
            EnemyModel = new EnemyModel();
            CoreModel = new CoreModel(maxHp: 20);
            EconomyModel = new EconomyModel(initialGold: 200);
            GameStateModel = new GameStateModel();
            TowerModel = new TowerModel();
            ProjectileModel = new ProjectileModel();
            // Services
            PathService = new PathService();
            PathService.BuildFromScene();
            TowerConfigService = new TowerConfigService();
            TowerConfigService.RegisterDefaults();
            // View Factories
            EnemyViewFactory = new EnemyViewFactory(enemyPrefab, enemyRoot);
            TowerViewFactory = new TowerViewFactory(towerPrefab, towerRoot);
            ProjectileViewFactory = new ProjectileViewFactory(projectilePrefab, projectileRoot);
            // Systems
            EnemySystem = new EnemySystem(this);
            WaveSystem = new WaveSystem(this, waveConfig);
            CoreSystem = new CoreSystem(CoreModel, GameStateModel);
            TowerSystem = new TowerSystem(this);
            ProjectileSystem = new ProjectileSystem(this);
            EconomySystem = new EconomySystem(EconomyModel);
            BuildSystem = new BuildSystem(this);
        }
        public void Tick(float dt)
        {
            if (!GameStateModel.IsPlaying) return;
            WaveSystem.Tick(dt);
            EnemySystem.Tick(dt);
            TowerSystem.Tick(dt);
            ProjectileSystem.Tick(dt);
        }
        public void Dispose()
        {
            CoreSystem?.Dispose();
            EconomySystem?.Dispose();
            EventBus.Clear();
            Current = null;
        }
    }
}
