using System;
using System.Threading;
using AvoidGame.MediaPipe;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace AvoidGame.Calibration
{
    [RequireComponent(typeof(Calibrator))]
    public class Player : MonoBehaviour
    {
        [Inject] private IMediaPipeManager _mediaPipeManager;
        [Inject] private ICalibrationStateHolder _calibrationStateHolder;

        [SerializeField] private Calibrator calibrator;

        private PoseAccumulator _poseAccumulator;

        private const int CalibrationTime = 5;

        public void Start()
        {
            _poseAccumulator = new PoseAccumulator();
            var token = this.GetCancellationTokenOnDestroy();
            StartTimerAsync(token).Forget();
        }

        public void Update()
        {
            switch (_calibrationStateHolder.State)
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
                    calibrator.Retarget(_mediaPipeManager.LandmarkData);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async UniTask StartTimerAsync(CancellationToken token)
        {
            _calibrationStateHolder.State = CalibrationState.Waiting;
            while (!_mediaPipeManager.IsReady)
            {
                await UniTask.Delay(1000, cancellationToken: token);
                Debug.Log($"Waiting for MediaPipe. IsReady: {_mediaPipeManager.IsReady}");
            }

            _calibrationStateHolder.State = CalibrationState.Calibrating;

            // Wait for calibration
            await UniTask.Delay(TimeSpan.FromSeconds(CalibrationTime), cancellationToken: token);
            calibrator.CalcRetargetMultiplier(_mediaPipeManager.LandmarkData);
            _calibrationStateHolder.State = CalibrationState.Finished;
        }
    }
}