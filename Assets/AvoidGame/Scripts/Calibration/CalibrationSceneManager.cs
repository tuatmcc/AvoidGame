using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration
{
    public class CalibrationSceneManager : MonoBehaviour
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private IMediaPipeManager _mediaPipeManager;

        [SerializeField] private bool skipCalibration = false;

        private const int CalibrationTime = 5;

        public enum CalibrationState
        {
            Waiting,
            Calibrating,
            Finished
        }

        public CalibrationState State { get; private set; } = CalibrationState.Waiting;
        public event Action<CalibrationState> OnCalibrationStateChanged;

        private async void Start()
        {
            if (skipCalibration)
            {
                _gameStateManager.GameState = GameState.CountDown;
                return;
            }

            while (!_mediaPipeManager.IsReady)
            {
                await UniTask.Delay(500);
                Debug.Log($"Waiting for MediaPipe. IsReady: {_mediaPipeManager.IsReady}");
            }

            State = CalibrationState.Calibrating;
            OnCalibrationStateChanged?.Invoke(State);
            Debug.Log("Calibration Started");
            await UniTask.Delay(TimeSpan.FromSeconds(CalibrationTime));
            State = CalibrationState.Finished;
            OnCalibrationStateChanged?.Invoke(State);
            Debug.Log("Calibration Finished");
        }
    }
}