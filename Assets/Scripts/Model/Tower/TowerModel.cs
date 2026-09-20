using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

namespace Game.Models
{
    public class TowerModel
    {
        private readonly List<TowerData> _list = new();
        private int _nextId = 1;
        public IReadOnlyList<TowerData> All => _list;
        public int Count => _list.Count;
        public TowerData this[int i] => _list[i];
        public int NextId() => _nextId++;
        public void Add(TowerData t) => _list.Add(t);
    }
}
