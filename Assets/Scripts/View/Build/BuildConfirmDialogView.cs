using Game.Core;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    /// <summary>
    /// 建造确认框。
    /// 显示：订阅 BuildConfirmRequestedEvent
    /// 隐藏：订阅 BuildConfirmClosedEvent
    /// 按钮：直接调 BuildDragSystem.Confirm() / Cancel()，不自行改状态、不碰时间
    /// </summary>

    public class BuildConfirmDialogView : MonoBehaviour
    {
        [SerializeField] private GameLoop gameLoop;
        [SerializeField] private GameObject root;
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        private Vector3 _pendingPos;
        private string _pendingConfig;
        private GameContext Ctx => gameLoop != null ? gameLoop.Context : null;
        private void Awake()
        {
            if (gameLoop == null) gameLoop = FindFirstObjectByType<GameLoop>();
            root.SetActive(false);
        }
        private void OnEnable()
        {
            EventBus.Subscribe<BuildConfirmRequestedEvent>(OnRequested);
            EventBus.Subscribe<BuildConfirmClosedEvent>(OnClosed);
            confirmButton.onClick.AddListener(OnConfirm);
            cancelButton.onClick.AddListener(OnCancel);
        }
        private void OnDisable()
        {
            EventBus.Unsubscribe<BuildConfirmRequestedEvent>(OnRequested);
            EventBus.Unsubscribe<BuildConfirmClosedEvent>(OnClosed);
            confirmButton.onClick.RemoveListener(OnConfirm);
            cancelButton.onClick.RemoveListener(OnCancel);
        }
        private void OnRequested(BuildConfirmRequestedEvent e)
        {
            var cfg = Ctx.TowerCatalog.Get(e.ConfigId);
            infoText.text = cfg != null
                ? $"建造 {cfg.DisplayName}？（{cfg.BuildCost} 金币）"
                : $"建造 {e.ConfigId}";
            root.SetActive(true);
        }
        private void OnClosed(BuildConfirmClosedEvent e)
        {
            root.SetActive(false);
        }
        private void OnConfirm() => Ctx?.BuildDragSystem.Confirm();
        private void OnCancel() => Ctx?.BuildDragSystem.Cancel();
    }
}
