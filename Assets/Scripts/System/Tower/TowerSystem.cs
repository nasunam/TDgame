using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Data;
using Game.Events;
using Game.Config;

namespace Game.Systems
{
    /// <summary>
    /// 塔的索敌、冷却、开火、升级、出售。
    /// 不负责：子弹移动（ProjectileSystem），建造（BuildSystem），选中（TowerSelectionSystem）。
    public class TowerSystem
    {
        private readonly GameContext _ctx;
        public TowerSystem(GameContext ctx) { _ctx = ctx; }
        // 每帧 索敌 + 开火
        public void Tick(float dt)
        {
            if (!_ctx.GameStateModel.IsPlaying) return;
            var towers = _ctx.TowerModel;
            for (int i = 0; i < towers.Count; i++)
            {
                var tower = towers[i];
                tower.CooldownTimer -= dt;
                if (tower.CooldownTimer > 0f) continue;
                var target = FindTarget(tower);
                if (target == null) continue;
                Fire(tower, target);
                tower.CooldownTimer = tower.FireCooldown;
            }
        }
        // 索敌，选择范围内走的最远的敌人
        private EnemyData FindTarget(TowerData tower)
        {
            EnemyData best = null;
            float bestDist = -1f;
            var enemies = _ctx.EnemyModel.All;
            float rangeSq = tower.Range * tower.Range;
            for (int i = 0; i < enemies.Count; i++)
            {
                var e = enemies[i];
                if (e.State != EnemyState.Moving) continue;
                float dSq = (tower.Position - e.Position).sqrMagnitude;
                if (dSq > rangeSq) continue;
                // 走的最远，优先索敌
                if (e.Distance > bestDist)
                {
                    bestDist = e.Distance;
                    best = e;
                }
            }
            return best;
        }
        // 开火
        private void Fire(TowerData tower, EnemyData target)
        {
            tower.TargetEnemyId = target.id;
            var proj = new ProjectileData
            {
                Id = _ctx.ProjectileModel.NextId(),
                Position = tower.Position,
                Speed = tower.ProjectileSpeed,
                Damage = tower.Damage,
                TargetEnemyId = target.id
            };
            _ctx.ProjectileModel.Add(proj);
            _ctx.ProjectileViewFactory.Spawn(proj);
        }
        // 升级
        public bool Upgrade(TowerData tower)
        {
            if (tower == null) return false;
            if (!tower.CanUpgrade)
            {
                EventBus.Publish(new UiToastEvent { Message = "已是最高等级" });
                return false;
            }
            var next = tower.NextLevel;
            if (!_ctx.EconomySystem.SpendGold(next.UpgradeCost))
            {
                EventBus.Publish(new UiToastEvent { Message = $"金币不足（需 {next.UpgradeCost}）" });
                return false;
            }
            tower.LevelIndex++;
            ApplyLevel(tower, next);
            EventBus.Publish(new TowerUpgradedEvent
            {
                TowerId = tower.Id,
                NewLevelIndex = tower.LevelIndex
            });
            return true;
        }
        // 应用升级
        private void ApplyLevel(TowerData tower, TowerLevel level)
        {
            tower.Range = level.Range;
            tower.Damage = level.Damage;
            tower.FireCooldown = level.FireCooldown;
            tower.ProjectileSpeed = level.ProjectileSpeed;
        }
        // 出售
        public bool Sell(TowerData tower)
        {
            if (tower == null) return false;
            // 已投入金币 = 建造费 + 升级费
            int invested = tower.Config.BuildCost;
            for (int i = 0; i < tower.LevelIndex; i++)
                invested += tower.Config.Levels[i].UpgradeCost;
            int refund = Mathf.FloorToInt(invested * 0.7f);
            _ctx.TowerModel.Remove(tower);
            _ctx.TowerViewFactory.Remove(tower.Id);
            _ctx.EconomySystem.AddGold(refund);
            EventBus.Publish(new TowerSoldEvent
            {
                TowerId = tower.Id,
                Refund = refund
            });
            return true;
        }
    }
}
