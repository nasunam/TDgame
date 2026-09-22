using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Events;
using Game.Data;
using Game.Models;

namespace Game.Systems
{
    /// <summary>
    /// 管理"当前选中哪座塔"。事件驱动，不需要 Tick。
    /// 发布：TowerSelectedEvent / TowerDeselectedEvent
    /// </summary>
    public class TowerSelectionSystem
    {
        private readonly GameContext _ctx;
        public TowerSelectionModel Model { get; }
        public TowerSelectionSystem(GameContext ctx)
        {
            _ctx = ctx;
            Model = new TowerSelectionModel();
            EventBus.Subscribe<TowerSoldEvent>(OnTowerSold);
        }
        public void Dispose()
        {
            EventBus.Unsubscribe<TowerSoldEvent>(OnTowerSold);
        }
        public void Select(int towerId)
        {
            if (Model.SelectedTowerId == towerId) return;
            Model.SelectedTowerId = towerId;
            EventBus.Publish(new TowerSelectedEvent { TowerId = towerId });
        }
        public void Deselect()
        {
            if (!Model.HasSelection) return;
            Model.SelectedTowerId = -1;
            EventBus.Publish(new TowerDeselectedEvent());
        }
        public TowerData GetSelected()
        {
            if (!Model.HasSelection) return null;
            return _ctx.TowerModel.FindById(Model.SelectedTowerId);
        }
        private void OnTowerSold(TowerSoldEvent e)
        {
            if (Model.SelectedTowerId == e.TowerId) Deselect();
        }
    }
}
