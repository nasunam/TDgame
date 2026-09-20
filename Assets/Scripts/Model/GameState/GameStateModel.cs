using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Models
{
    public enum GameState
    {
        Playing,
        Victory,
        Defeat
    }
    
    public class GameStateModel
    {
        public GameState State = GameState.Playing;
        public bool IsPlaying => State == GameState.Playing;
    }
}
