using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.Title
{
    public class TitleSceneManager : MonoBehaviour
    {
        [Inject] GameStateManager _gameStateManager;
        void Start()
        {
        
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _gameStateManager.GameState = GameState.Calibration;
            }
        }
    }
}
