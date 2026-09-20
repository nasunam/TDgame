using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

namespace Game.Models
{
    public class WaveModel
    {
        public int TotalWaves;
        public int CurrentWaveIndex = -1; // -1表示还没有开始
        public float NextWaveCountdown; //  >0表示正在倒计时
        public int EnemiesRemaining; // 未生成 + 场上存活
    }
}