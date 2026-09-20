using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using Game.Views;

namespace Game.Services
{
    public class PathService
    {
        private readonly Dictionary<string, PathRoute> _routes = new();
        public void Register(PathRoute route)
        {
            _routes[route.Id] = route;
        }
        public PathRoute Get(string pathId)
        {
            if (_routes.TryGetValue(pathId, out var route)) return route;
            Debug.LogError($"[PathService]找不到路径:{pathId}");
            return null;
        }
        public IEnumerable<PathRoute> All => _routes.Values;
        /// <summary>
        /// 从场景里所有 PathAuthoring 收集路径。
        /// </summary>
        public void BuildFromScene()
        {
            _routes.Clear();
            var authors = Object.FindObjectsByType<PathAuthoring>(FindObjectsSortMode.None);
            foreach (var a in authors)
            {
                var points = a.GetWorldPoints();
                if (points.Length < 2)
                {
                    Debug.LogWarning($"[PathService] PathAuthoring '{a.PathId}' 路点不足，已跳过");
                    continue;
                }
                Register(new PathRoute(a.PathId, points));
            }
            Debug.Log($"[PathService] 已注册 {_routes.Count} 条路径");
        }
    }
}
