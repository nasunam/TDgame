using Game.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    public class TowerData
    {
        public int Id;
        public TowerConfigSO Config; // 保留引用，升级时读下一级
        public int LevelIndex; // 0 = 一级塔
        public Vector3 Position;
        public float Range;
        public float Damage;
        public float FireCooldown;
        public float CooldownTimer; // 剩余时间
        public float ProjectileSpeed;
        public int TargetEnemyId = -1; // -1表示无目标
        public TowerLevel CurrentLevel => Config.Levels[LevelIndex];
        public bool CanUpgrade => Config != null && LevelIndex < Config.Levels.Count - 1;
        public TowerLevel NextLevel => CanUpgrade ? Config.Levels[LevelIndex + 1] : null;

    }
}