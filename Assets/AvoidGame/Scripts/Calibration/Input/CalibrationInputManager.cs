using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration.Input
{
    public class CalibrationInputManager : MonoBehaviour
    {
        [Inject] private GameStateManager _gameStateManager;
        private AvoidGameInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new AvoidGameInputActions();
            _inputActions.Enable();
        }

        private void Start()
        {
            _inputActions.UI.Submit.started += (_) => { _gameStateManager.GameState = GameState.Play; };
        }

        private void OnDestroy()
        {
            _inputActions.Disable();
            _inputActions.Dispose();
        }
    }
}