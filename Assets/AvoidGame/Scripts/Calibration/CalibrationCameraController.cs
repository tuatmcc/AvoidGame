using Cinemachine;
using UnityEngine;

namespace AvoidGame.Calibration
{
    public class CalibrationCameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        private CinemachineOrbitalTransposer _virtualCameraOrbitalTransposer;

        private void Awake()
        {
            _virtualCameraOrbitalTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }

        private void Update()
        {
            _virtualCameraOrbitalTransposer.m_XAxis.Value = 1;
        }
    }
}