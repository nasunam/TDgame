using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Config;

namespace Game.Services
{
    public class TowerConfigService
    {
        private readonly Dictionary<string, TowerConfig> _map = new();
        public void Register(TowerConfig cfg) => _map[cfg.Id] = cfg;
        public TowerConfig Get(string id)
            => _map.TryGetValue(id, out var c) ? c : null;
        public void RegisterDefaults()
        {
            Register(new TowerConfig
            {
                Id = "basic_tower",
                Range = 5f,
                FireCooldown = 0.8f,
                Damage = 30f,
                Cost = 50,
                ProjectileSpeed = 12f,
            });
        }
    }
}