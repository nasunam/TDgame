using Game.Core;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    /// <summary>
    /// 选中蒙黑遮罩。点击空白区域 → 取消选中。
    /// 面板子物体会挡住点击（不穿透）。
    /// </summary>
    public class SelectionOverlayView : MonoBehaviour
    {
        [SerializeField] private GameLoop gameLoop;
        [SerializeField] private GameObject root;
        [SerializeField] private Button backgroundButton; // 覆盖背景的Image;
        private GameContext Ctx => gameLoop != null ? gameLoop.Context : null;
        private void Awake()
        {
            if (gameLoop == null) gameLoop = FindFirstObjectByType<GameLoop>();
        }
        private void OnEnable()
        {
            EventBus.Subscribe<TowerSelectedEvent>(OnSelected);
            EventBus.Subscribe<TowerDeselectedEvent>(OnDeselected);
            backgroundButton.onClick.AddListener(OnBackgroundClicked);
        }
        private void OnDisable()
        {
            EventBus.Unsubscribe<TowerSelectedEvent>(OnSelected);
            EventBus.Unsubscribe<TowerDeselectedEvent>(OnDeselected);
            backgroundButton.onClick.RemoveListener(OnBackgroundClicked);
        }
        private void OnSelected(TowerSelectedEvent e) => root.SetActive(true);
        private void OnDeselected(TowerDeselectedEvent e) => root.SetActive(false);
        private void OnBackgroundClicked()
        {
            Ctx?.TowerSelectionSystem.Deselect();
        }
    }
}