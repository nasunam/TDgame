using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    public enum EnemyState
    {
        Moving,
        Dead,
        ReachedEnd
    }
    public class EnemyData
    {
        public int id;
        public string PathId; // 走哪一条路径
        public float Distance; // 沿路径走了多远
        public float Speed; // 行走速度
        public float Hp;
        public float MaxHp;
        public int CoreDamage = 1; // 对目标点的伤害
        public int GoldReward = 10; // 掉落金币数量
        public Vector3 Position;
        public Vector3 Forward; // 朝向
        public Color Tint;
        public EnemyState State = EnemyState.Moving;
    }
}
