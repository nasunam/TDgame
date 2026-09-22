using Game.Config;
using Game.Core;
using Game.Data;
using Game.Events;
using UnityEngine;

namespace Game.Systems
{
    public class BuildSystem
    {
        private readonly GameContext _ctx;
        private readonly TowerCatalogSO _catalog;

        private const float MinTowerSpacingSq = 1.5f * 1.5f;

        public BuildSystem(GameContext ctx, TowerCatalogSO catalog) { _ctx = ctx; _catalog = catalog; }

        public bool TryBuild(Vector3 position, string configId)
        {
            if (!_ctx.GameStateModel.IsPlaying) return false;
            var cfg = _catalog.Get(configId);
            if (cfg == null)
            {
                Debug.LogWarning($"[BuildSystem] 未知塔配置: {configId}");
                return false;
            }
            // 距离检测：与已有塔太近
            var towers = _ctx.TowerModel.All;
            for (int i = 0; i < towers.Count; i++)
            {
                if ((towers[i].Position - position).sqrMagnitude < MinTowerSpacingSq)
                {
                    EventBus.Publish(new UiToastEvent { Message = "位置已被占用" });
                    EventBus.Publish(new BuildFailedEvent { Reason = "occupied" });
                    return false;
                }
            }
            if (!_ctx.EconomySystem.SpendGold(cfg.BuildCost))
            {
                EventBus.Publish(new UiToastEvent { Message = $"金币不足（需 {cfg.BuildCost}）" });
                EventBus.Publish(new BuildFailedEvent { Reason = "gold" });
                return false;
            }
            var level0 = cfg.Levels[0];
            var tower = new TowerData
            {
                Id = _ctx.TowerModel.NextId(),
                Config = cfg,
                Position = position,
                Range = level0.Range,
                Damage = level0.Damage,
                FireCooldown = level0.FireCooldown,
                CooldownTimer = 0f,
                ProjectileSpeed = level0.ProjectileSpeed,
            };

            _ctx.TowerModel.Add(tower);
            _ctx.TowerViewFactory.Spawn(tower);

            EventBus.Publish(new TowerBuiltEvent
            {
                TowerId = tower.Id,
                Position = tower.Position
            });

            return true;
        }
    }
}