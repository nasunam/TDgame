using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

namespace Game.Views
{
    public class TowerView : MonoBehaviour
    {
        private TowerData _data;
        public int TowerId => _data != null ? _data.Id : -1;
        public void Bind(TowerData data)
        {
            _data = data;
            transform.position = data.Position;
        }
    }
}
