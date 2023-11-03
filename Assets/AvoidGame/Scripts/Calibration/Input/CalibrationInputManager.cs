using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace AvoidGame.Calibration.Input
{
    public class CalibrationInputManager : MonoBehaviour
    {
        [Inject] private GameStateManager _gameStateManager;
        private DefaultInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new DefaultInputActions();
            _inputActions.Enable();
        }

        private void Start()
        {
            _inputActions.UI.Submit.started += (ctx) => { _gameStateManager.GameState = GameState.CountDown; };
        }
    }
}