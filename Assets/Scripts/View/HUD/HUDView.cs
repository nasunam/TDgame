using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Events;
using TMPro;

namespace Game.Views
{
    public class HUDViews : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coreHpText;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI waveText;

        private void Start()
        {
            RefreshFromContext();
        }
        private void OnEnable()
        {
            EventBus.Subscribe<CoreHpChangedEvent>(OnCoreHpChanged);
            EventBus.Subscribe<GoldChangedEvent>(OnGoldChanged);
            EventBus.Subscribe<GameOverEvent>(OnGameOver);
            EventBus.Subscribe<WaveStatusEvent>(OnWaveStatus);
        }
        private void OnDisable()
        {
            EventBus.Unsubscribe<CoreHpChangedEvent>(OnCoreHpChanged);
            EventBus.Unsubscribe<GoldChangedEvent>(OnGoldChanged);
            EventBus.Unsubscribe<GameOverEvent>(OnGameOver);
            EventBus.Unsubscribe<WaveStatusEvent>(OnWaveStatus);
        }
        private void RefreshFromContext()
        {
            var ctx = GameContext.Current;
            if (ctx == null) return;
            OnCoreHpChanged(new CoreHpChangedEvent
            {
                Hp = ctx.CoreModel.Hp,
                MaxHp = ctx.CoreModel.MaxHp
            });
            OnGoldChanged(new GoldChangedEvent { Gold = ctx.EconomyModel.Gold });
            if (waveText != null && ctx.WaveSystem != null)
            {
                var m = ctx.WaveSystem.Model;
                waveText.text = $"Wave 0/{m.TotalWaves}";
            }
        }
        private void OnCoreHpChanged(CoreHpChangedEvent e)
        {
            if (coreHpText != null)
                coreHpText.text = $"Core: {e.Hp}/{e.MaxHp} ";
        }
        private void OnGoldChanged(GoldChangedEvent e)
        {
            if (goldText != null)
                goldText.text = $"Gold: {e.Gold}";
        }
        private void OnWaveStatus(WaveStatusEvent e)
        {
            if (waveText == null) return;
            int display = Mathf.Max(1, e.CurrentWaveIndex + 1);
            if (e.Countdown > 0f)
                waveText.text = $"Wave {display}/{e.TotalWaves} in {e.Countdown:F1}s";
            else
                waveText.text = $"Wave {display}/{e.TotalWaves} - Enemies {e.EnemiesRemaining}";
        }
        private void OnGameOver(GameOverEvent e)
        {
            Debug.Log(e.IsVictory ? "VICTORY" : "DEFEAT");
        }
    }
}
