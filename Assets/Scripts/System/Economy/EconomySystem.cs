using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Events;
using Game.Models;

namespace Game.Systems
{
    public class EconomySystem
    {
        private readonly EconomyModel _model;
        public EconomySystem(EconomyModel model)
        {
            _model = model;
            EventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);
        }
        public void Dispose()
        {
            EventBus.Unsubscribe<EnemyKilledEvent>(OnEnemyKilled);
        }
        private void OnEnemyKilled(EnemyKilledEvent e) => AddGold(e.GoldReward);
        public void AddGold(int amount)
        {
            _model.Gold += amount;
            EventBus.Publish(new GoldChangedEvent { Gold = _model.Gold });
        }
        public bool SpendGold(int amount)
        {
            if (_model.Gold < amount) return false;
            _model.Gold -= amount;
            EventBus.Publish(new GoldChangedEvent { Gold = _model.Gold });
            return true;
        }
    }
}
