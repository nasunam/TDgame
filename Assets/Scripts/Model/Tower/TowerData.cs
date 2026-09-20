using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    public class TowerData
    {
        public int Id;
        public string ConfigId;
        public Vector3 Position;
        public float Range;
        public float Damage;
        public float FireCooldown;
        public float CooldownTimer; // 剩余时间
        public float ProjectileSpeed;
        public int TargetEnemyId = -1; // -1表示无目标

    }
}