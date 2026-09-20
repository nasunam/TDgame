using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using Game.Core;
using Game.Events;
using Game.Models;
using Game.Config;

namespace Game.Systems
{
    public class WaveSystem
    {
        public enum Phase
        {
            Countdown, // 开始前的倒计时
            Spawning, // 生成中
            WaitingClear, // 等待敌人清空
            Finished // 最后一波结束后
        }

        private readonly GameContext _ctx;
        private readonly WaveConfigSO _config;
        private Phase _phase = Phase.Countdown;
        private float _timer;
        private int _spawnedInWave;
        public WaveModel Model { get; }
        public WaveSystem(GameContext ctx, WaveConfigSO config)
        {
            _ctx = ctx;
            _config = config;
            int total = config != null ? config.Waves.Count : 0;
            float initial = config != null ? config.InitialDelay : 3f;
            Model = new WaveModel { TotalWaves = total, CurrentWaveIndex = -1, NextWaveCountdown = initial, };
            _timer = initial;
            Debug.Log($"[WaveSystem] 已加载{total}");
        }
        public void Tick(float dt)
        {
            switch (_phase)
            {
                case Phase.Countdown: TickCountdown(dt); break;
                case Phase.Spawning: TickSpawning(dt); break;
                case Phase.WaitingClear: TickWaitingClear(dt); break;
            }
            PublishStatus();
        }

        // Status BoardCast
        private void PublishStatus()
        {
            int remaining = _ctx.EnemyModel.Count;
            if (Model.CurrentWaveIndex >= 0 && Model.CurrentWaveIndex < Model.TotalWaves)
            {
                var wave = _config.Waves[Model.CurrentWaveIndex];
                remaining += Mathf.Max(0, wave.Count - _spawnedInWave);
            }
            Model.EnemiesRemaining = remaining;
            EventBus.Publish(new WaveStatusEvent
            {
                CurrentWaveIndex = Model.CurrentWaveIndex,
                TotalWaves = Model.TotalWaves,
                Countdown = Model.NextWaveCountdown,
                EnemiesRemaining = remaining,
            });
        }

        // Phase: Countdown
        private void TickCountdown(float dt)
        {
            _timer -= dt;
            Model.NextWaveCountdown = Mathf.Max(0, _timer);
            if (_timer > 0) return;
            // 开下一波
            Model.CurrentWaveIndex++;
            _spawnedInWave = 0;
            _timer = 0f;
            _phase = Phase.Spawning;
            var wave = _config.Waves[Model.CurrentWaveIndex];
            EventBus.Publish(new WaveStartedEvent
            {
                WaveIndex = Model.CurrentWaveIndex,
                TotalWaves = Model.TotalWaves,
                EnemyCount = wave.Count,
            });
        }

        // Phase: Spawning
        private void TickSpawning(float dt)
        {
            var wave = _config.Waves[Model.CurrentWaveIndex];
            if (_spawnedInWave >= wave.Count)
            {
                _phase = Phase.WaitingClear;
                return;
            }
            _timer -= dt;
            if (_timer > 0) return;
            SpawnOne(wave.enemy);
            _spawnedInWave++;
            _timer = wave.IntervalBetweenSpawns;
        }
        // Spawn
        private void SpawnOne(EnemyConfigSO cfg)
        {
            var route = _ctx.PathService.Get("main");
            if (route == null) return;
            var data = new EnemyData
            {
                id = _ctx.EnemyModel.NextId(),
                PathId = "main",
                Distance = 0f,
                Speed = cfg.Speed,
                Hp = cfg.Hp,
                MaxHp = cfg.Hp,
                CoreDamage = cfg.CoreDamage,
                GoldReward = cfg.GoldReward,
                State = EnemyState.Moving,
            };
            data.Position = route.Evaluate(0f, out data.Forward);
            _ctx.EnemyModel.Add(data);
            _ctx.EnemyViewFactory.Spawn(data);
        }

        // Phase: WaitingClear
        private void TickWaitingClear(float dt)
        {
            if (_ctx.EnemyModel.Count > 0) return;
            var wave = _config.Waves[Model.CurrentWaveIndex];
            EventBus.Publish(new WaveClearEvent { WaveIndex = Model.CurrentWaveIndex });
            bool hasMore = Model.CurrentWaveIndex < Model.TotalWaves - 1;
            if (hasMore)
            {
                _timer = wave.DelayAfterWave;
                Model.NextWaveCountdown = _timer;
                _phase = Phase.Countdown;
            }
            else
            {
                _phase = Phase.Finished;
                EventBus.Publish(new AllWavesCompletedEvent());
            }
        }
    }
}