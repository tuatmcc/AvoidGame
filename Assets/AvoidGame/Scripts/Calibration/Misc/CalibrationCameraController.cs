using System;
using AvoidGame.Calibration.Interface;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace AvoidGame.Calibration.Misc
{
    public class CalibrationCameraController : MonoBehaviour
    {
        [Inject] private ICalibrationStateManager _calibrationStateManager;
        [SerializeField] private CinemachineVirtualCamera virtualOrbitalCamera;
        [SerializeField] private CinemachineVirtualCamera virtualFocusCamera;
        [SerializeField] private CinemachineVirtualCamera virtualFinalCamera;

        private CinemachineOrbitalTransposer _virtualCameraOrbitalTransposer;

        private void Awake()
        {
            _virtualCameraOrbitalTransposer =
                virtualOrbitalCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            _calibrationStateManager.OnCalibrationStateChanged += OnCalibrationStateChanged;
        }

        private void Start()
        {
            virtualFocusCamera.Priority = 10;
            virtualFocusCamera.Priority = 0;
            virtualFinalCamera.Priority = 0;
        }

        private void Update()
        {
            if (_calibrationStateManager.State == CalibrationState.Dissolving) return;
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
                case CalibrationState.Dissolving:
                    virtualFocusCamera.Priority = 10;
                    virtualOrbitalCamera.Priority = 0;
                    break;
                case CalibrationState.Finished:
                    virtualOrbitalCamera.Priority = 10;
                    virtualFocusCamera.Priority = 0;
                    break;
                case CalibrationState.Transitioning:
                    virtualFinalCamera.Priority = 10;
                    virtualOrbitalCamera.Priority = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}