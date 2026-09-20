using Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Views
{
    public class ProjectileView : MonoBehaviour
    {
        private ProjectileData _data;
        public void Bind(ProjectileData data)
        {
            _data = data;
            transform.position = data.Position;
        }
        private void LateUpdate()
        {
            if (_data == null) return;
            transform.position = _data.Position;
        }
    }
}
