using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    #region 全局状态事件
    public struct EnemyReachedEndEvent { public int EnemyId; public int CoreDamage; }
    public struct GameOverEvent { public bool IsVictory; }
    public struct CoreHpChangedEvent { public int Hp; public int MaxHp; }
    public struct GoldChangedEvent { public int Gold; }
    #endregion
    #region 波次状态事件
    public struct WaveStartedEvent { public int WaveIndex; public int TotalWaves; public int EnemyCount; }
    public struct WaveClearEvent { public int WaveIndex; }
    public struct AllWavesCompletedEvent { }
    public struct WaveStatusEvent { public int CurrentWaveIndex; public int TotalWaves; public float Countdown; public int EnemiesRemaining; }
    #endregion
    #region 敌人相关事件
    public struct EnemyDamagedEvent { public int EnemyId; public float Hp; public float MaxHp; }
    public struct EnemyHitEvent { public Vector3 Position; public Vector3 ImpactDir; }
    public struct EnemyKilledEvent
    {
        public int EnemyId;
        public int GoldReward;
        public Vector3 Position;
        public Color Tint;
        public Vector3 ImpactDir; // 子弹作用的方向
    }
    #endregion
    #region 炮塔相关事件
    public struct TowerBuiltEvent { public int TowerId; public Vector3 Position; }
    public struct TowerSelectedEvent { public int TowerId; }
    public struct TowerDeselectedEvent { }
    public struct TowerUpgradedEvent { public int TowerId; public int NewLevelIndex; }
    public struct TowerSoldEvent { public int TowerId; public int Refund; }
    #endregion
    #region 建造状态相关事件
    public struct BuildFailedEvent { public string Reason; }
    public struct BuildDragStartedEvent { public string ConfigId; }
    public struct BuildDragEndedEvent { public bool Success; }
    public struct BuildConfirmClosedEvent { }
    public struct BuildConfirmRequestedEvent { public Vector3 position; public string ConfigId; }
    #endregion
    #region UI通知事件
    public struct UiToastEvent { public string Message; }
    #endregion
}