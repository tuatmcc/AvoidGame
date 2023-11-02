using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration
{
    public sealed class CalibrationStateHolder : ICalibrationStateHolder
    {
        private CalibrationState _state = CalibrationState.Waiting;

        public CalibrationState State
        {
            get => _state;
            set
            {
                _state = value;
                Debug.Log($"CSManager: CalibrationState: {_state}");
            }
        }

        public Action<CalibrationState> OnCalibrationStateChanged { get; set; }

        public CalibrationStateHolder()
        {
            Debug.Log("CalibrationSceneManager Initialized, id: " + GetHashCode());
        }
    }
}