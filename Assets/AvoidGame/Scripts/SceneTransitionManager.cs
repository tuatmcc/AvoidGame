using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame
{
    public class SceneTransitionManager : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingUI;

        [Inject] private GameStateManager _gameStateManager;

        private void Awake()
        {
            _loadingUI.SetActive(false);
        }

        private void Start()
        {
            _gameStateManager.OnGameStateChanged += SceneTransition;
        }

        private void SceneTransition(GameState gameState)
        {

        }
    }
}
