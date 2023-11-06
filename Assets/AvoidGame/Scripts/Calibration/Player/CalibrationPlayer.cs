using System;
using AvoidGame.Calibration.Interface;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration.Player
{
    [RequireComponent(typeof(Calibrator))]
    public class CalibrationPlayer : MonoBehaviour
    {
        [Inject] private IMediaPipeManager _mediaPipeManager;
        [Inject] private ICalibrationStateManager _calibrationStateManager;

        [SerializeField] private Calibrator calibrator;

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
                case CalibrationState.Finished:
                {
                    if (!_mulitplierCalculated)
                    {
                        calibrator.CalcRetargetMultiplier(_poseAccumulator.GetAverageLandmarks());
                        _mulitplierCalculated = true;
                    }

                    calibrator.Retarget(_mediaPipeManager.LandmarkData);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // private async UniTask StartTimerAsync(CancellationToken token)
        // {
        //     _calibrationStateManager.State = CalibrationState.Waiting;
        //     while (!_mediaPipeManager.IsReady)
        //     {
        //         await UniTask.Delay(1000, cancellationToken: token);
        //         Debug.Log($"Waiting for MediaPipe. IsReady: {_mediaPipeManager.IsReady}");
        //     }
        //
        //     _calibrationStateManager.State = CalibrationState.Calibrating;
        //
        //     // Wait for calibration
        //     await UniTask.Delay(TimeSpan.FromSeconds(CalibrationTime), cancellationToken: token);
        //     calibrator.CalcRetargetMultiplier(_mediaPipeManager.LandmarkData);
        //     _calibrationStateManager.State = CalibrationState.Finished;
        // }
    }
}