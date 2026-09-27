using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Config
{
    [CreateAssetMenu(fileName = "TowerConfig", menuName = "游戏/炮塔数据资产")]
    public class TowerConfigSO : ScriptableObject
    {
        public string Id = "basic_tower";
        public string DisplayName = "Basic Tower";
        public int BuildCost = 50;
        public GameObject ViewPrefab;
        [Tooltip("索引 0 = 一级塔，索引 1 = 二级塔，依次类推")]
        public List<TowerLevel> Levels = new();
    }
    [Serializable]
    public class TowerLevel
    {
        [Tooltip("升到这一级需要的金币。一级塔填 0（建造时已付）")]
        public int UpgradeCost;
        public float Range = 5f;
        public float Damage = 30f;
        public float FireCooldown = 0.8f;
        public float ProjectileSpeed = 12f;
    }
}
