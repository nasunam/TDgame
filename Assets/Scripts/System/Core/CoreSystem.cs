using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Models;
using Game.Events;
using Game.Core;

namespace Game.Systems
{
    public class CoreSystem
    {
        private readonly CoreModel _core;
        private readonly GameStateModel _state;
        public CoreSystem(CoreModel core, GameStateModel state) // 初始化
        {
            _core = core;
            _state = state;
            EventBus.Subscribe<EnemyReachedEndEvent>(OnEnemyReachedEnd);
            EventBus.Subscribe<AllWavesCompletedEvent>(OnAllWavesCompleted);
        }
        public void Dispose() // 处置订阅
        {
            EventBus.Unsubscribe<EnemyReachedEndEvent>(OnEnemyReachedEnd);
            EventBus.Unsubscribe<AllWavesCompletedEvent>(OnAllWavesCompleted);
        }
        private void OnEnemyReachedEnd(EnemyReachedEndEvent e)
        {
            if (_core.IsDead) return;
            _core.Hp = Mathf.Max(0, _core.Hp - e.CoreDamage);
            EventBus.Publish(new CoreHpChangedEvent
            {
                Hp = _core.Hp,
                MaxHp = _core.MaxHp
            });
            if (_core.IsDead)
            {
                _state.State = GameState.Defeat;
                EventBus.Publish(new GameOverEvent { IsVictory = false });
            }
        }
        private void OnAllWavesCompleted(AllWavesCompletedEvent e)
        {
            if (!_state.IsPlaying) return;
            _state.State = GameState.Victory;
            EventBus.Publish(new GameOverEvent { IsVictory = true });
        }
    }
}
