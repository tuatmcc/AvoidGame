using Cinemachine;
using UnityEngine;

namespace AvoidGame.Play.Misc
{
    public class IntroCameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera introCamera;
        [SerializeField] private Transform playerToLookAt;
        private readonly float _speed = 0.5f;
        private CinemachineTrackedDolly _dolly;

        private void Start()
        {
            _dolly = introCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
            _dolly.m_PathPosition = 0;
            introCamera.m_LookAt = playerToLookAt;
        }

        private void Update()
        {
            _dolly.m_PathPosition += _speed * Time.deltaTime;
        }
    }
}