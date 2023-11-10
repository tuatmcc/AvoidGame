using System;
using AvoidGame.Interface;
using UnityEngine;
using Zenject;

namespace AvoidGame.Description
{
    public class DescriptionInputManager : MonoBehaviour
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private IDescriptionSceneManager _sceneManager;
        private AvoidGameInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new AvoidGameInputActions();
            _inputActions.Enable();
        }

        private void Start()
        {
            _inputActions.Description.Next.started += (_) => _sceneManager.MoveToNext();
            _inputActions.Description.SkipAll.started += (_) => _gameStateManager.GameState++;
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void OnDestroy()
        {
            _inputActions.Disable();
            _inputActions.Dispose();
        }
    }
}