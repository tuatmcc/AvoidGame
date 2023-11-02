using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration.Tests
{
    public class ToPlay : MonoBehaviour
    {
        [Inject] GameStateManager _gameStateManager;
        void Start()
        {
            _gameStateManager.GameState = GameState.CountDown;
        }
    }
}
