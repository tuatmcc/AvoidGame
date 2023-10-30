using System;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration
{
    [RequireComponent(typeof(Calibrator))]
    public class Player : MonoBehaviour
    {
        [Inject] private MediaPipeManager _mediaPipeManager;
        [Inject] private CalibrationSceneManager _calibrationSceneManager;

        private Calibrator _calibrator;

        private readonly PoseAccumulator _poseAccumulator;

        private void Awake()
        {
            _calibrationSceneManager.OnCalibrationStateChanged += OnCalibrationStateChanged;
        }

        private void OnCalibrationStateChanged(CalibrationSceneManager.CalibrationState state)
        {
            Debug.Log($"Calibration State Changed to {state}");
            // Calculate Multiplier
            if (state != CalibrationSceneManager.CalibrationState.Finished) return;
            var averagedLandmarks = _poseAccumulator.GetAverageLandmarks();
            _calibrator.CalcRetargetMultiplier(averagedLandmarks);
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
                    _calibrator.Retarget(_mediaPipeManager.LandmarkData);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}