using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Config
{
    [CreateAssetMenu(fileName = "TowerCatalog", menuName = "游戏/炮塔目录资产")]
    public class TowerCatalogSO : ScriptableObject
    {
        public List<TowerConfigSO> Towers = new();
        public Dictionary<string, TowerConfigSO> _map;
        public TowerConfigSO Get(string id)
        {
            if (_map == null)
            {
                _map = new();
                foreach (var t in Towers)
                {
                    if (t != null) _map[t.Id] = t;
                }
            }
            return _map.TryGetValue(id, out var c) ? c : null;
        }
    }
}
