using Game.Core;
using Game.Data;
using Game.Events;
using UnityEngine;

namespace Game.Systems
{
    public class BuildSystem
    {
        private readonly GameContext _ctx;

        private const float MinTowerSpacingSq = 1.5f * 1.5f;

        public BuildSystem(GameContext ctx) { _ctx = ctx; }

        public bool TryBuild(Vector3 position, string configId)
        {
            if (!_ctx.GameStateModel.IsPlaying) return false;

            var cfg = _ctx.TowerConfigService.Get(configId);
            if (cfg == null)
            {
                Debug.LogWarning($"[BuildSystem] 未知塔配置: {configId}");
                return false;
            }

            if (!_ctx.EconomySystem.SpendGold(cfg.Cost))
            {
                Debug.Log("[BuildSystem] 金币不足");
                return false;
            }

            // 距离检测：与已有塔太近
            var towers = _ctx.TowerModel.All;
            for (int i = 0; i < towers.Count; i++)
            {
                if ((towers[i].Position - position).sqrMagnitude < MinTowerSpacingSq)
                {
                    _ctx.EconomySystem.AddGold(cfg.Cost);  // 退还
                    Debug.Log("[BuildSystem] 位置被占用");
                    return false;
                }
            }

            var tower = new TowerData
            {
                Id = _ctx.TowerModel.NextId(),
                ConfigId = cfg.Id,
                Position = position,
                Range = cfg.Range,
                Damage = cfg.Damage,
                FireCooldown = cfg.FireCooldown,
                CooldownTimer = 0f,
                ProjectileSpeed = cfg.ProjectileSpeed,
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