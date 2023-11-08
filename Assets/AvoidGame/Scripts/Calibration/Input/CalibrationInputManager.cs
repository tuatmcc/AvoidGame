using AvoidGame.Calibration.Interface;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration.Input
{
    public class CalibrationInputManager : MonoBehaviour
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private ICalibrationStateManager _calibrationStateManager;
        private AvoidGameInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new AvoidGameInputActions();
            _inputActions.Enable();
        }

        private void Start()
        {
            // Next Game State
            _inputActions.Calibration.Submit.started += HandleSubmit;
            // Next Calibration State
            _inputActions.Calibration.Next.started += HandleNext;
        }

        private void OnDestroy()
        {
            _inputActions.Disable();
            _inputActions.Dispose();
        }

        private void HandleNext(UnityEngine.InputSystem.InputAction.CallbackContext _)
        {
            if (_calibrationStateManager.State == CalibrationState.Transitioning)
            {
                _gameStateManager.GameState += 1;
            }
            else
            {
                _calibrationStateManager.State += 1;
            }
        }

        private void HandleSubmit(UnityEngine.InputSystem.InputAction.CallbackContext _)
        {
            _gameStateManager.GameState += 1;
        }
    }
}