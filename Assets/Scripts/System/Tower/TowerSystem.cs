using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Data;

namespace Game.Systems
{
    public class TowerSystem
    {
        private readonly GameContext _ctx;
        public TowerSystem(GameContext ctx) { _ctx = ctx; }
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
    }
}
