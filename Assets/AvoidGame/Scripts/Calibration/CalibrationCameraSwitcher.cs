using System;
using Cinemachine;
using UnityEngine;

namespace AvoidGame.Calibration
{
    public class CalibrationCameraSwitcher : MonoBehaviour
    {
        public bool debug = true;
        [SerializeField] private CinemachineFreeLook freeLook;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        private CinemachineOrbitalTransposer _virtualCameraOrbitalTransposer;

        private void OnValidate()
        {
            _virtualCameraOrbitalTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            if (debug)
            {
                freeLook.m_Priority = 10;
                virtualCamera.m_Priority = 0;
            }
            else
            {
                freeLook.m_Priority = 0;
                virtualCamera.m_Priority = 10;
            }
        }

        private void Update()
        {
            _virtualCameraOrbitalTransposer.m_XAxis.Value = 1;
        }
    }
}