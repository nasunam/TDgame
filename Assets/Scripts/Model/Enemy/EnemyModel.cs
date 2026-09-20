using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

namespace Game.Models
{
    public class EnemyModel
    {
        private readonly List<EnemyData> _enemies = new();
        private int _nextId = 1;
        public IReadOnlyList<EnemyData> All => _enemies;
        public int NextId() => _nextId++;
        public void Add(EnemyData enemy) => _enemies.Add(enemy);
        public void Remove(EnemyData enemy) => _enemies.Remove(enemy);
        // 倒序遍历删除, Tick使用
        public void RemoveAt(int index) => _enemies.RemoveAt(index);
        public int Count => _enemies.Count;
        public EnemyData this[int index] => _enemies[index];
        public EnemyData FindById(int id)
        {
            for (int i = 0; i < _enemies.Count; i++)
                if (_enemies[i].id == id) return _enemies[i];
            return null;
        }
    }
}
