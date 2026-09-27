using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Data;
using Game.Events;

namespace Game.Systems
{
    public class EnemySystem
    {
        private readonly GameContext _ctx;
        public EnemySystem(GameContext ctx)
        {
            _ctx = ctx;
        }
        public void Tick(float dt)
        {
            if (!_ctx.GameStateModel.IsPlaying) return;
            var model = _ctx.EnemyModel;
            var pathService = _ctx.PathService;
            // 倒序遍历，方便删除
            for (int i = model.Count - 1; i >= 0; i--)
            {
                var e = model[i];
                // 1. 更新移动
                if (e.State == EnemyState.Moving)
                {
                    var route = pathService.Get(e.PathId);
                    if (route == null) // 判断敌人是否按照路径Id移动
                    {
                        _ctx.EnemyViewFactory.Remove(e.id);
                        model.RemoveAt(i);
                        continue;
                    }
                    e.Distance += e.Speed * dt;
                    if (e.Distance >= route.TotalLength)
                    {
                        e.Distance = route.TotalLength;
                        e.Position = route.Evaluate(e.Distance, out e.Forward);
                        e.State = EnemyState.ReachedEnd;
                    }
                    else
                    {
                        e.Position = route.Evaluate(e.Distance, out e.Forward);
                    }
                }
                // 2. 处理到达终点
                if (e.State == EnemyState.ReachedEnd)
                {
                    EventBus.Publish(new EnemyReachedEndEvent
                    {
                        EnemyId = e.id,
                        CoreDamage = e.CoreDamage
                    });
                    _ctx.EnemyViewFactory.Remove(e.id);
                    model.RemoveAt(i);
                }
                // 3. 死亡
                else if (e.State == EnemyState.Dead)
                {
                    _ctx.EnemyViewFactory.Remove(e.id);
                    model.RemoveAt(i);
                }
            }
        }
        // 塔/子弹通过它造成伤害
        public void ApplyDamage(EnemyData enemy, float damage, Vector3 impactDir)
        {
            if (enemy == null) return;
            if (enemy.State != EnemyState.Moving) return;
            enemy.Hp -= damage;
            EventBus.Publish(new EnemyDamagedEvent
            {
                EnemyId = enemy.id,
                Hp = enemy.Hp,
                MaxHp = enemy.MaxHp
            });
            EventBus.Publish(new EnemyHitEvent
            {
                Position = enemy.Position,
                ImpactDir = impactDir
            });
            if (enemy.Hp <= 0f)
            {
                enemy.Hp = 0f;
                enemy.State = EnemyState.Dead;
                EventBus.Publish(new EnemyKilledEvent
                {
                    EnemyId = enemy.id,
                    GoldReward = enemy.GoldReward,
                    Position = enemy.Position,
                    Tint = enemy.Tint,
                    ImpactDir = impactDir
                });
            }
        }
    }
}