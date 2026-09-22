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
        public void Remove(TowerData t) => _list.Remove(t);
        public TowerData FindById(int id)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].Id == id) return _list[i];
            }
            return null;
        }
    }
}
