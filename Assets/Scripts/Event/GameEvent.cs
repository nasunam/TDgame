using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public struct EnemyReachedEndEvent { public int EnemyId; public int CoreDamage; }
    public struct CoreHpChangedEvent { public int Hp; public int MaxHp; }
    public struct GoldChangedEvent { public int Gold; }
    public struct GameOverEvent { public bool IsVictory; }
    public struct TowerBuiltEvent { public int TowerId; public Vector3 Position; }
    public struct EnemyDamagedEvent { public int EnemyId; public float Hp; public float MaxHp; }
    public struct EnemyKilledEvent { public int EnemyId; public int GoldReward; }
    public struct WaveStartedEvent { public int WaveIndex; public int TotalWaves; public int EnemyCount; }
    public struct WaveClearEvent { public int WaveIndex; }
    public struct AllWavesCompletedEvent { }
    public struct WaveStatusEvent { public int CurrentWaveIndex; public int TotalWaves; public float Countdown; public int EnemiesRemaining; }
}