using System;
using System.Threading;
using AvoidGame.Calibration.Interface;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace AvoidGame.Calibration
{
    /// <summary>
    /// Holds and Controls CalibrationState. Behaves like a MonoBehaviour.
    /// </summary>
    public class CalibrationStateManager : ICalibrationStateManager, IInitializable, IDisposable
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private IMediaPipeManager _mediaPipeManager;

        private CalibrationState _state = CalibrationState.Waiting;

        public float CalibratingDuration { get; } = 5f;
        public float FinishingDuration { get; } = 2f;
        public float FinishedDuration { get; } = 10f;

        public CalibrationState State
        {
            get => _state;
            set
            {
                _state = value;
                OnCalibrationStateChanged?.Invoke(_state);
                Debug.Log($"CSManager: CalibrationState: {_state}");
            }
        }

        public Action<CalibrationState> OnCalibrationStateChanged { get; set; }

        private CancellationTokenSource _cts;


        public void Initialize()
        {
            _cts = new CancellationTokenSource();
            StartTimerAsync(_cts.Token).Forget();
        }

        private async UniTask StartTimerAsync(CancellationToken token)
        {
            // Wait for MediaPipeManager to be ready.(MediaPipeManager is ready when it receives the first landmark data.)
            State = CalibrationState.Waiting;
            while (!_mediaPipeManager.IsReady)
            {
                await UniTask.Delay(500, cancellationToken: token);
                Debug.Log($"Waiting for MediaPipe. IsReady: {_mediaPipeManager.IsReady}");
            }

            // Start calibration.
            State = CalibrationState.Calibrating;
            await UniTask.Delay(TimeSpan.FromSeconds(CalibratingDuration), cancellationToken: _cts.Token);

            // Wait for finishing.
            State = CalibrationState.Finishing;
            await UniTask.Delay(TimeSpan.FromSeconds(FinishingDuration), cancellationToken: _cts.Token);

            // Finish calibration.
            State = CalibrationState.Finished;
            await UniTask.Delay(TimeSpan.FromSeconds(FinishedDuration), cancellationToken: _cts.Token);
            _gameStateManager.GameState = GameState.Play;
        }

        public void Dispose()
        {
            _cts?.Cancel();
        }
    }
}