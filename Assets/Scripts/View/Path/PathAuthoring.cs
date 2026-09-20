using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Views;

namespace Game.Views
{
    /// <summary>
    /// 挂在场景空物体上，下面放几个 Transform 作为路点。
    /// 未来要“多条路径”，就是多个 PathAuthoring，PathId 不同。
    /// </summary>
    public class PathAuthoring : MonoBehaviour
    {
        public string PathId = "main";
        public Transform[] Points;
        public Vector3[] GetWorldPoints()
        {
            if (Points == null) return new Vector3[0];
            var arr = new Vector3[Points.Length];
            for (int i = 0; i < Points.Length; i++)
            {
                arr[i] = Points[i] != null ? Points[i].position : transform.position;
            }
            return arr;
        }
        // 在编辑器里画线，方便看
        private void OnDrawGizmos()
        {
            if (Points == null || Points.Length < 2) return;
            Gizmos.color = Color.yellow;
            for (int i = 0; i < Points.Length - 1; i++)
            {
                if (Points[i] == null || Points[i + 1] == null) continue;
                Gizmos.DrawLine(Points[i].position, Points[i + 1].position);
                Gizmos.DrawSphere(Points[i].position, 0.3f);
            }
            Gizmos.DrawSphere(Points[Points.Length - 1].position, 0.3f);
        }
    }
}
