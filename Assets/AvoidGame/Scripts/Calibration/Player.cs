using System;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration
{
    [RequireComponent(typeof(Calibrator))]
    public class Player : MonoBehaviour
    {
        [Inject] private IMediaPipeManager _mediaPipeManager;
        [Inject] private CalibrationSceneManager _calibrationSceneManager;
        [Inject] private GameStateManager _gameStateManager;

        [SerializeField] private Calibrator calibrator;

        private readonly PoseAccumulator _poseAccumulator = new PoseAccumulator();

        private bool _calculationFinished = false;

        private void Awake()
        {
            _calibrationSceneManager.OnCalibrationStateChanged += OnCalibrationStateChanged;
        }

        private void OnCalibrationStateChanged(CalibrationSceneManager.CalibrationState state)
        {
            // Calculate Multiplier
            if (state != CalibrationSceneManager.CalibrationState.Finished) return;
            var averagedLandmarks = _poseAccumulator.GetAverageLandmarks();
            calibrator.CalcRetargetMultiplier(_mediaPipeManager.LandmarkData);
            _calculationFinished = true;
        }


        private void Update()
        {
            switch (_calibrationSceneManager.State)
            {
                case CalibrationSceneManager.CalibrationState.Waiting:
                {
                    break;
                }
                case CalibrationSceneManager.CalibrationState.Calibrating:
                {
                    _poseAccumulator.AccumulateLandmarks(_mediaPipeManager.LandmarkData);
                    break;
                }
                case CalibrationSceneManager.CalibrationState.Finished:
                {
                    if (!_calculationFinished) return;
                    calibrator.Retarget(_mediaPipeManager.LandmarkData);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _gameStateManager.GameState = GameState.CountDown;
            }
        }
    }
}