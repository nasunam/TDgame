using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    /// <summary>
    /// 一条路径。未来多条路径就是多个 PathRoute。
    /// 路径用“累计距离”来做进度，避免每帧找段。
    /// </summary>
    public class PathRoute
    {
        public string Id;
        public Vector3[] Points; // 路点
        public float[] CumulativeDistances; // 从Point[0]到Point[i]的距离
        public float TotalLength; // 总长度
        public PathRoute(string id, Vector3[] points)
        {
            Id = id;
            Points = points;
            BulidCache();
        }
        private void BulidCache()
        {
            if (Points == null || Points.Length < 2)
            {
                Debug.LogError($"[PathRoute] Path '{Id}' 至少需要两个点");
                TotalLength = 0;
                CumulativeDistances = new float[0];
                return;
            }
            CumulativeDistances = new float[Points.Length];
            CumulativeDistances[0] = 0f;
            float sum = 0f;
            for (int i = 1; i < Points.Length; i++)
            {
                sum += Vector3.Distance(Points[i - 1], Points[i]);
                CumulativeDistances[i] = sum;
            }
            TotalLength = sum;
        }
        /// <summary>
        /// 根据已走距离，返回世界坐标和朝向。
        /// distance 会自动 clamp 到 [0, TotalLength]。
        /// </summary>
        public Vector3 Evaluate(float distance, out Vector3 direction)
        {
            direction = Vector3.forward;
            if (Points == null || Points.Length < 2) return Vector3.zero;
            distance = Mathf.Clamp(distance, 0f, TotalLength);
            // 找到所在段：CumulativeDistances[i] <= distance <= CumulativeDistances[i+1]
            int seg = 0;
            for (int i = 1; i < CumulativeDistances.Length; i++)
            {
                if (distance <= CumulativeDistances[i])
                {
                    seg = i - 1;
                    break;
                }
                seg = i - 1;
            }
            float segStart = CumulativeDistances[seg];
            float segEnd = CumulativeDistances[seg + 1];
            float segLen = segEnd - segStart;
            float t = segLen <= 0.0001f ? 0f : (distance - segStart) / segLen;
            Vector3 a = Points[seg];
            Vector3 b = Points[seg + 1];
            Vector3 pos = Vector3.Lerp(a, b, t);
            direction = (b - a).normalized;
            return pos;
        }
    }
}
