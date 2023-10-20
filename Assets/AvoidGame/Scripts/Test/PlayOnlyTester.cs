using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using AvoidGame;

namespace AvoidGame.Test
{
    public class PlayOnlyTester : MonoBehaviour
    {
        [Inject] GameStateManager gameStateManager;
        private void Start()
        {
            gameStateManager.GameState = GameState.Playing;
        }
    }
}
