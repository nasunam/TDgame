using Game.Core;
using Game.Data;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    /// <summary>
    /// 塔详情面板。订阅选中事件显示/隐藏，
    /// 按钮点击调 TowerSystem.Upgrade / Sell —— 自己不修改任何数据。
    /// </summary>
    public class TowerInfoPanelView : MonoBehaviour
    {
        [SerializeField] private GameLoop gameLoop;
        [SerializeField] private GameObject panelRoot;
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI statsText;
        [SerializeField] private TextMeshProUGUI upgradeButtonText;
        [SerializeField] private TextMeshProUGUI sellButtonText;
        [Header("Buttons")]
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button sellButton;
        private GameContext Ctx => gameLoop != null ? gameLoop.Context : null;
        private void Awake()
        {
            if (gameLoop == null) gameLoop = FindFirstObjectByType<GameLoop>();
        }
        private void OnEnable()
        {
            EventBus.Subscribe<TowerSelectedEvent>(OnSelected);
            EventBus.Subscribe<TowerDeselectedEvent>(OnDeselected);
            EventBus.Subscribe<TowerUpgradedEvent>(OnUpgraded);
            EventBus.Subscribe<GoldChangedEvent>(OnGoldChanged);
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
            sellButton.onClick.AddListener(OnSellClicked);
            panelRoot.SetActive(false);
        }
        private void OnDisable()
        {
            EventBus.Unsubscribe<TowerSelectedEvent>(OnSelected);
            EventBus.Unsubscribe<TowerDeselectedEvent>(OnDeselected);
            EventBus.Unsubscribe<TowerUpgradedEvent>(OnUpgraded);
            EventBus.Unsubscribe<GoldChangedEvent>(OnGoldChanged);
            upgradeButton.onClick.RemoveListener(OnUpgradeClicked);
            sellButton.onClick.RemoveListener(OnSellClicked);
        }
        // 事件处理
        private void OnSelected(TowerSelectedEvent e) { Refresh(); panelRoot.SetActive(true); }
        private void OnDeselected(TowerDeselectedEvent e) => panelRoot.SetActive(false);
        private void OnUpgraded(TowerUpgradedEvent e) { Refresh(); }
        private void OnGoldChanged(GoldChangedEvent e) { RefreshUpgradeAffordability(); }
        // 按钮回调
        private void OnUpgradeClicked()
        {
            var tower = Ctx?.TowerSelectionSystem.GetSelected();
            if (tower == null) return;
            Ctx.TowerSystem.Upgrade(tower);
            Refresh();
        }
        private void OnSellClicked()
        {
            var tower = Ctx?.TowerSelectionSystem.GetSelected();
            if (tower == null) return;
            Ctx.TowerSystem.Sell(tower);
            // Sell 内部会发 TowerSoldEvent → TowerSelectionSystem 会 Deselect → 面板隐藏
        }
        // 刷新显示
        private void Refresh()
        {
            var ctx = Ctx;
            if (ctx == null) return;
            var tower = ctx.TowerSelectionSystem.GetSelected();
            if (tower == null) { panelRoot.SetActive(false); return; }
            titleText.text = $"{tower.Config.DisplayName}  Lv.{tower.LevelIndex + 1}";
            statsText.text =
                $"Damage : {tower.Damage}\n" +
                $"Range  : {tower.Range}\n" +
                $"Rate   : {1f / Mathf.Max(0.01f, tower.FireCooldown):F1}/s";
            if (tower.CanUpgrade)
            {
                var next = tower.NextLevel;
                upgradeButtonText.text = $"Upgrade ({next.UpgradeCost})";
                upgradeButton.interactable = ctx.EconomyModel.Gold >= next.UpgradeCost;
            }
            else
            {
                upgradeButtonText.text = "MAX";
                upgradeButton.interactable = false;
            }
            // 出售返还额（与 TowerSystem.Sell 的算法保持一致）
            int invested = tower.Config.BuildCost;
            for (int i = 1; i <= tower.LevelIndex; i++)
                invested += tower.Config.Levels[i].UpgradeCost;
            int refund = Mathf.FloorToInt(invested * 0.7f);
            sellButtonText.text = $"Sell ({refund})";
        }
        private void RefreshUpgradeAffordability()
        {
            if (!panelRoot.activeSelf) return;
            Refresh();
        }
    }
}
