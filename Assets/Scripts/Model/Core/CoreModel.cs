using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Models
{
    public class CoreModel
    {
        public int Hp;
        public int MaxHp;
        public bool IsDead => Hp <= 0;
        public CoreModel(int maxHp)
        {
            MaxHp = maxHp;
            Hp = maxHp;
        }
    }
}
