using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using Game.Core;

namespace Game.Views
{
    public class TowerView : MonoBehaviour
    {
        [SerializeField] private Transform head; // 炮管子物体
        [SerializeField] private float rotateSpeed = 10f; // 旋转速度
        private TowerData _data;
        public int TowerId => _data != null ? _data.Id : -1;
        public void Bind(TowerData data)
        {
            _data = data;
            transform.position = data.Position;
        }
        private void Update()
        {
            if (_data == null || head == null) return;
            if (_data.TargetEnemyId < 0) return;
            var ctx = GameContext.Current;
            if (ctx == null) return;
            var target = ctx.EnemyModel.FindById(_data.TargetEnemyId);
            if (target == null) return;
            Vector3 dir = target.Position - head.position;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.01f) return;
            Quaternion want = Quaternion.LookRotation(dir, Vector3.up);
            head.rotation = Quaternion.Slerp(head.rotation, want, Time.deltaTime * rotateSpeed);
        }
    }
}
