using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using UnityEngine.EventSystems;

namespace Game.Views
{
    public class TowerSelectInputController : MonoBehaviour
    {
        [SerializeField] private GameLoop gameLoop;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask towerLayer;
        private void Awake()
        {
            if (gameLoop == null) gameLoop = FindFirstObjectByType<GameLoop>();
            if (mainCamera == null) mainCamera = Camera.main;
        }
        private void Update()
        {
            if (gameLoop == null || gameLoop.Context == null) return;
            if (!Input.GetMouseButtonDown(0)) return;
            var ctx = gameLoop.Context;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 1000f, towerLayer))
            {
                var view = hit.collider.GetComponentInParent<TowerView>();
                if (view != null && view.TowerId >= 0)
                {
                    ctx.TowerSelectionSystem.Select(view.TowerId);
                    return;
                }
            }
            // 点到非塔的位置 -> 取消选中
            ctx.TowerSelectionSystem.Deselect();
        }
    }
}