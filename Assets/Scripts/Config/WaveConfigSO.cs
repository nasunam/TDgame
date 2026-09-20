using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Config
{
    [CreateAssetMenu(fileName = "WaveConfig", menuName = "游戏/波次数据资产")]
    public class WaveConfigSO : ScriptableObject
    {
        public float InitialDelay = 3f; // 第一波之前的倒计时
        public List<WaveEntry> Waves = new();
    }
    [Serializable]
    public class WaveEntry
    {
        public EnemyConfigSO enemy;
        public int Count = 5;
        public float IntervalBetweenSpawns = 1f; // 本波敌人之间的间隔
        public float DelayAfterWave = 5f; // 本波清空后到下一波开始的倒计时
    }
}
