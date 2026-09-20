using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Config
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "游戏/敌人数据资产")]
    public class EnemyConfigSO : ScriptableObject
    {
        public string Id = "enemy_basic";
        public float Speed = 2f;
        public float Hp = 100f;
        public int CoreDamage = 1;
        public int GoldReward = 10;
        public Color Tint = Color.red;
    }
}
