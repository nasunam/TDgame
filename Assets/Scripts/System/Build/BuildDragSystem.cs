using Game.Core;
using Game.Events;
using Game.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    /// <summary>
    /// 拖拽建造的编排器。三个状态：
    ///   Idle          无虚影、无拖拽、时间正常
    ///   Dragging      虚影跟随指针、时间减速
    ///   PreviewLocked 虚影冻结 + 确认框打开、时间减速、拒绝其他图标
    ///
    /// 时间规则：Idle → 非Idle 时 RequestSlow 一次；非Idle → Idle 时 ReleaseSlow 一次。
    /// 中间状态切换不碰时间。
    ///
    /// 不负责：合法性判定（BuildSystem）、幽灵渲染（GhostFactory）、时间（TimeScaleService）。
    /// </summary>
    public class BuildDragSystem
    {
        public enum DragState { Idle, Dragging, PreviewLocked }
        public enum DragResult { None, Valid, Invalid }
        private readonly GameContext _ctx;
        private readonly Camera _camera;
        private readonly LayerMask _groundLayer;
        private DragState _state = DragState.Idle;
        private string _configId;
        private Vector3 _currentPosition;
        private DragResult _currentResult = DragResult.None;
        private TowerGhostView _ghost;
        // 公共属性
        public DragState State => _state;
        public bool IsDragging => _state == DragState.Dragging;
        public bool IsActive => _state != DragState.Idle;
        public DragResult CurrentResult => _currentResult;
        public Vector3 CurrentPosition => _currentPosition;
        public BuildDragSystem(GameContext ctx, Camera camera, LayerMask groundLayer)
        {
            _ctx = ctx;
            _camera = camera;
            _groundLayer = groundLayer;
        }
        // 外部入口
        /// <summary>从 UI 图标开始拖拽。Idle 下接受；PreviewLocked 下仅接受同 configId。</summary>
        public void BeginDrag(string configId)
        {
            if (!_ctx.GameStateModel.IsPlaying) return;
            // Idle：正常启动.RequestSlow 在此处唯一触发.
            if (_state == DragState.Idle)
            {
                var cfg = _ctx.TowerCatalog.Get(configId);
                if (cfg == null) return;
                _ghost = _ctx.GhostFactory.Spawn(cfg, Vector3.zero);
                _configId = configId;
                _currentResult = DragResult.Invalid;
                _state = DragState.Dragging;
                _ctx.TimeScaleService.RequestSlow();
                EventBus.Publish(new BuildDragStartedEvent { ConfigId = configId });
                return;
            }
            // Dragging：忽略重复触发（正常流程不会发生，防御）
            if (_state == DragState.Dragging) return;
            // PreviewLocked：同图标 → 回 Dragging；其他图标 → 拒绝。
            if (_state == DragState.PreviewLocked)
            {
                if (configId != _configId)
                {
                    EventBus.Publish(new UiToastEvent { Message = "请先完成当前制造" });
                    return;
                }
                // 关确认框，保留虚影，回到 Dragging。时间已是慢的，不重复 RequestSlow。
                EventBus.Publish(new BuildConfirmClosedEvent());
                _currentResult = DragResult.Invalid;
                _state = DragState.Dragging;
                // 不重发 BuildDragStartedEvent：整段建造流程尚未结束
            }
        }
        /// <summary>指针移动时调用。Dragging 下每次移动都重算校验。</summary>
        public void UpdatePointer(Vector2 screenPos)
        {
            if (_state != DragState.Dragging) return;
            var ray = _camera.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out var hit, 1000f, _groundLayer))
            {
                _currentPosition = hit.point;
                _currentResult = _ctx.BuildSystem.CanBuild(_currentPosition, _configId, out _)
                    ? DragResult.Valid
                    : DragResult.Invalid;
            }
            else
            {
                _currentResult = DragResult.Invalid;
            }
            _ghost?.SetPosition(_currentPosition);
            _ghost?.SetValid(_currentResult == DragResult.Valid);
        }
        /// <summary>玩家松开指针。</summary>
        public void EndDrag(Vector2 screenPos)
        {
            if (_state != DragState.Dragging) return;
            UpdatePointer(screenPos);
            if (_currentResult == DragResult.Valid)
            {
                // 合法：进入 PreviewLocked，虚影冻结，弹确认框。时间保持不变。
                _state = DragState.PreviewLocked;
                EventBus.Publish(new BuildConfirmRequestedEvent { ConfigId = _configId, position = _currentPosition });
            }
            else
            {
                // 非法：卸虚影 → Idle → 恢复时间。
                DespawnGhost();
                _configId = null;
                _state = DragState.Idle;
                _ctx.TimeScaleService.ReleaseSlow();
                EventBus.Publish(new BuildDragEndedEvent { Success = false });
                EventBus.Publish(new UiToastEvent { Message = "无法在此处建造" });
            }
        }
        /// <summary>点确认按钮。仅 PreviewLocked 有效。</summary>
        public void Confirm()
        {
            if (_state != DragState.PreviewLocked) return;
            var pos = _currentPosition;
            var id = _configId;
            // 关确认框 → 卸虚影 → Idle → 恢复时间
            EventBus.Publish(new BuildConfirmClosedEvent());
            DespawnGhost();
            _configId = null;
            _state = DragState.Idle;
            _ctx.TimeScaleService.ReleaseSlow();
            // 建造（可能失败，内部会发 toast）
            _ctx.BuildSystem.TryBuild(pos, id);
            EventBus.Publish(new BuildDragEndedEvent { Success = true });
        }
        /// <summary>点取消按钮，或外部强制取消。任意非 Idle 状态有效。</summary>
        public void Cancel()
        {
            if (_state == DragState.Idle) return;
            if (_state == DragState.PreviewLocked)
            {
                EventBus.Publish(new BuildConfirmClosedEvent());
            }
            DespawnGhost();
            _configId = null;
            _state = DragState.Idle;
            _ctx.TimeScaleService.ReleaseSlow();
            EventBus.Publish(new BuildDragEndedEvent { Success = false });
        }
        private void DespawnGhost()
        {
            if (_ghost == null) return;
            _ctx.GhostFactory.Despawn(_ghost);
            _ghost = null;
        }
    }
}
