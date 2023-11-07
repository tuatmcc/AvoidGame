using System;
using AvoidGame.Calibration.Interface;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration
{
    public class CalibrationCameraController : MonoBehaviour
    {
        [Inject] private ICalibrationStateManager _calibrationStateManager;
        [SerializeField] private CinemachineVirtualCamera virtualOrbitalCamera;
        [SerializeField] private CinemachineVirtualCamera virtualFocusCamera;
        private CinemachineOrbitalTransposer _virtualCameraOrbitalTransposer;

        private void Awake()
        {
            _virtualCameraOrbitalTransposer =
                virtualOrbitalCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            _calibrationStateManager.OnCalibrationStateChanged += OnCalibrationStateChanged;
        }

        private void Update()
        {
            if (_calibrationStateManager.State == CalibrationState.Finishing) return;
            _virtualCameraOrbitalTransposer.m_XAxis.Value += Time.deltaTime * 50;
        }

        private void OnCalibrationStateChanged(CalibrationState state)
        {
            switch (state)
            {
                case CalibrationState.Waiting:
                    break;
                case CalibrationState.Calibrating:
                    break;
                case CalibrationState.Finishing:
                    virtualFocusCamera.Priority = 10;
                    virtualOrbitalCamera.Priority = 0;
                    break;
                case CalibrationState.Finished:
                    virtualFocusCamera.Priority = 0;
                    virtualOrbitalCamera.Priority = 10;
                    _virtualCameraOrbitalTransposer.m_XAxis.Value = 0;
                    // centerize camera
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}