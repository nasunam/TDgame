using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using Game.Core;

namespace Game.Systems
{
    public class SpawnSystem
    {
        private readonly GameContext _ctx;
        private float _timer;
        private float _interval = 1f;
        private bool _running = true;
        public SpawnSystem(GameContext ctx)
        {
            _ctx = ctx;
        }
        public void Tick(float dt)
        {
            if (!_running) return;
            _timer += dt;
            if (_timer < _interval) return;
            _timer -= _interval;
            SpawnOne();
        }
        private void SpawnOne()
        {
            var data = new EnemyData
            {
                id = _ctx.EnemyModel.NextId(),
                PathId = "main",
                Distance = 0f,
                Speed = 2f,
                Hp = 100f,
                MaxHp = 100f,
                CoreDamage = 1,
                GoldReward = 10,
                State = EnemyState.Moving,
            };
            // 初始化位置
            var route = _ctx.PathService.Get(data.PathId);
            if (route == null) return;
            data.Position = route.Evaluate(0f, out data.Forward);
            _ctx.EnemyModel.Add(data);
            _ctx.EnemyViewFactory.Spawn(data);
        }
        public void SetRunning(bool running) => _running = running;
    }
}
