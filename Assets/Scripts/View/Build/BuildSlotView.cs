using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Views
{
    /// <summary>
    /// UI 槽位。按下拖拽开始建造；松开时通知拖拽系统。
    /// 具体坐标走 PointerEventData.position（屏幕空间），交给 BuildDragSystem。
    /// </summary>
    public class BuildSlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private GameLoop gameLoop;
        [SerializeField] private string towerConfigId = "basic_tower";
        private void Awake()
        {
            if (gameLoop == null) gameLoop = FindFirstObjectByType<GameLoop>();
        }
        private GameContext Ctx => gameLoop != null ? gameLoop.Context : null;
        public void OnBeginDrag(PointerEventData e)
        {
            Ctx?.BuildDragSystem.BeginDrag(towerConfigId);
            Ctx?.BuildDragSystem.UpdatePointer(e.position);
        }
        public void OnDrag(PointerEventData e)
        {
            Ctx?.BuildDragSystem.UpdatePointer(e.position);
        }
        public void OnEndDrag(PointerEventData e)
        {
            Ctx?.BuildDragSystem.EndDrag(e.position);
        }
    }
}
