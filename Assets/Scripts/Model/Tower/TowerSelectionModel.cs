using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Models
{
    public class TowerSelectionModel
    {
        public int SelectedTowerId = -1;
        public bool HasSelection => SelectedTowerId >= 0;
    }
}