using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Models
{
    public class EconomyModel
    {
        public int Gold;
        public EconomyModel(int initialGold = 0)
        {
            Gold = initialGold;
        }
    }
}
