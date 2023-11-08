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
        public float DissolvingDuration { get; } = 5f;
        public float FinishedDuration { get; } = 30f;


        public float TransitioningDuration { get; } = 3f;

        public CalibrationState State
        {
            get => _state;
            set
            {
                _state = value;
                OnCalibrationStateChanged?.Invoke(_state);
                Debug.Log($"CalibrationState Changed: {_state}");
            }
        }

        public Action<CalibrationState> OnCalibrationStateChanged { get; set; }

        private CancellationTokenSource _cts;

        public void Initialize()
        {
            _cts = new CancellationTokenSource();
            StartWaitingAsync(_cts.Token).Forget();
            OnCalibrationStateChanged += HandleCalibrationStateChange;
        }

        private async UniTask StartTimerAsync(CancellationToken token)
        {
            // Wait for MediaPipeManager to be ready.(MediaPipeManager is ready when it receives the first landmark data.)
            State = CalibrationState.Waiting;
            while ((!_mediaPipeManager.IsReady || State == CalibrationState.Waiting) && !token.IsCancellationRequested)
            {
                await UniTask.Delay(500, cancellationToken: token);
                Debug.Log($"Waiting for MediaPipe. IsReady: {_mediaPipeManager.IsReady}");
            }

            // Start Calibration
            if (State != CalibrationState.Calibrating)
                State = CalibrationState.Calibrating;
        }

        public void Dispose()
        {
            _cts?.Cancel();
        }

        private void HandleCalibrationStateChange(CalibrationState state)
        {
            switch (state)
            {
                case CalibrationState.Waiting:
                    break;
                case CalibrationState.Calibrating:
                    _cts = new CancellationTokenSource();
                    StartCalibratingAsync(_cts.Token).Forget();
                    break;
                case CalibrationState.Dissolving:
                    _cts = new CancellationTokenSource();
                    StartDissolvingAsync(_cts.Token).Forget();
                    break;
                case CalibrationState.Finished:
                    _cts = new CancellationTokenSource();
                    StartFinishedAsync(_cts.Token).Forget();
                    break;
                case CalibrationState.Transitioning:
                    _cts = new CancellationTokenSource();
                    StartTransitionAsync(_cts.Token).Forget();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private async UniTask StartWaitingAsync(CancellationToken token)
        {
            await UniTask.WaitUntil(() => _mediaPipeManager.IsReady, cancellationToken: token);
            if (State == CalibrationState.Waiting)
                State++;
        }

        private async UniTask StartDissolvingAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(DissolvingDuration), cancellationToken: token);
            if (State == CalibrationState.Dissolving)
                State++;
        }

        private async UniTask StartTransitionAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(TransitioningDuration), cancellationToken: token);
            _gameStateManager.GameState++;
        }

        private async UniTask StartCalibratingAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(CalibratingDuration), cancellationToken: token);
            if (State == CalibrationState.Calibrating)
                State++;
        }

        private async UniTask StartFinishedAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(FinishedDuration), cancellationToken: token);
            if (State == CalibrationState.Finished)
                State++;
        }
    }
}