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

        public bool CanBuild(Vector3 position, string configId, out string reason)
        {
            if (!_ctx.GameStateModel.IsPlaying) { reason = "not_playing"; return false; }
            var cfg = _catalog.Get(configId);
            if (cfg == null) { reason = "unkonwn_config"; return false; }
            var towers = _ctx.TowerModel.All;
            for (int i = 0; i < towers.Count; i++)
            {
                if ((towers[i].Position - position).sqrMagnitude < MinTowerSpacingSq)
                {
                    reason = "occupied";
                    return false;
                }
            }
            if (_ctx.EconomyModel.Gold < cfg.BuildCost)
            {
                reason = "gold";
                return false;
            }
            reason = null;
            return true;
        }
        public bool TryBuild(Vector3 position, string configId)
        {
            if (!CanBuild(position, configId, out var reason))
            {
                string msg = reason switch
                {
                    "occupied" => "位置已被占用",
                    "gold" => $"金币不足（需 {_catalog.Get(configId)?.BuildCost}）",
                    _ => "无法建造"
                };
                EventBus.Publish(new UiToastEvent { Message = msg });
                EventBus.Publish(new BuildFailedEvent { Reason = reason });
                return false;
            }
            var cfg = _catalog.Get(configId);
            _ctx.EconomySystem.SpendGold(cfg.BuildCost);
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