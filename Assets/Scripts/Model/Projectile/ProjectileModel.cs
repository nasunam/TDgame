using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

namespace Game.Models
{
    public class ProjectileModel
    {
        private readonly List<ProjectileData> _list = new();
        private int _nextId = 1;
        public IReadOnlyList<ProjectileData> All => _list;
        public int Count => _list.Count;
        public ProjectileData this[int i] => _list[i];
        public int NextId() => _nextId++;
        public void Add(ProjectileData p) => _list.Add(p);
        public void RemoveAt(int i) => _list.RemoveAt(i);
    }
}
