using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Data;

namespace Game.Systems
{
    public class ProjectileSystem
    {
        private readonly GameContext _ctx;
        public ProjectileSystem(GameContext ctx)
        {
            _ctx = ctx;
        }
        public void Tick(float dt)
        {
            if (!_ctx.GameStateModel.IsPlaying) return;
            var projectiles = _ctx.ProjectileModel;
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                var p = projectiles[i];
                var target = _ctx.EnemyModel.FindById(p.TargetEnemyId);
                // 目标已死 / 已到终点 / 不存在 => 子弹消失
                if (target == null || target.State != EnemyState.Moving)
                {
                    _ctx.ProjectileViewFactory.Remove(p.Id);
                    projectiles.RemoveAt(i);
                    continue;
                }
                Vector3 toTarget = target.Position - p.Position;
                float dist = toTarget.magnitude;
                float step = p.Speed * dt;
                if (dist <= step)
                {
                    // 命中
                    p.Position = target.Position;
                    _ctx.EnemySystem.ApplyDamage(target, p.Damage);
                    _ctx.ProjectileViewFactory.Remove(p.Id);
                    projectiles.RemoveAt(i);
                }
                else
                {
                    p.Position += (toTarget / dist) * step;
                }
            }
        }
    }
}
