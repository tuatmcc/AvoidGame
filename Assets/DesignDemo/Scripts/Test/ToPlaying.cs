using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DesignDemo
{
    public class ToPlaying : MonoBehaviour
    {
        [Inject] GameStateManager _gameStateManager;
        void Update()
        {
            if(_gameStateManager.GameState != GameState.Playing) { 
                _gameStateManager.GameState = GameState.Playing;
            }
        }

    }
}
