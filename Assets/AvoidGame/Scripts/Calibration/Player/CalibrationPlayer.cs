using System;
using AvoidGame.Calibration.Interface;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace AvoidGame.Calibration.Player
{
    [RequireComponent(typeof(IKController))]
    public class CalibrationPlayer : MonoBehaviour
    {
        [Inject] private IMediaPipeManager _mediaPipeManager;
        [Inject] private ICalibrationStateManager _calibrationStateManager;

        [FormerlySerializedAs("calibrator")] [SerializeField] private IKController ikController;

        private PoseAccumulator _poseAccumulator;

        // private const int CalibrationTime = 5;

        private bool _mulitplierCalculated = false;

        public void Start()
        {
            _poseAccumulator = new PoseAccumulator();
            // var token = this.GetCancellationTokenOnDestroy();
            // StartTimerAsync(token).Forget();
        }

        public void Update()
        {
            switch (_calibrationStateManager.State)
            {
                case CalibrationState.Waiting:
                {
                    break;
                }
                case CalibrationState.Calibrating:
                {
                    _poseAccumulator.AccumulateLandmarks(_mediaPipeManager.LandmarkData);
                    break;
                }
                case CalibrationState.Dissolving:
                {
                    if (!_mulitplierCalculated)
                    {
                        ikController.CalcRetargetMultiplier(_poseAccumulator.GetAverageLandmarks());
                        _mulitplierCalculated = true;
                    }

                    break;
                }
                case CalibrationState.Finished:
                {
                    ikController.Retarget(_mediaPipeManager.LandmarkData);
                    break;
                }
                case CalibrationState.Transitioning:
                {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}