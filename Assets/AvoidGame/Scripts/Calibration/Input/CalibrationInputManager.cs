using System;
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
            _inputActions.Calibration.Submit.started += (_) => { _gameStateManager.GameState += 1; };
            _inputActions.Calibration.Next.started += (_) =>
            {
                if (_calibrationStateManager.State ==
                    (CalibrationState)(Enum.GetValues(typeof(CalibrationState)).Length - 1))
                {
                    _gameStateManager.GameState += 1;
                    return;
                }

                _calibrationStateManager.State += 1;
            };
        }

        private void OnDestroy()
        {
            _inputActions.Disable();
            _inputActions.Dispose();
        }
    }
}