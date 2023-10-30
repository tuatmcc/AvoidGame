using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration
{
    public class CalibrationSceneManager : MonoBehaviour
    {
        private const int CalibrationTime = 5;
        [Inject] private MediaPipeManager _mediaPipeManager;

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