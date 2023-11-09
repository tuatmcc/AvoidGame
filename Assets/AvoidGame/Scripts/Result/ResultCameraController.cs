using System;
using AvoidGame.Result.Interface;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace AvoidGame.Result
{
    public class ResultCameraController : MonoBehaviour
    {
        [Inject] private IResultSceneManager _sceneManager;
        [SerializeField] private ResultCutManager resultCutManager;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private CinemachineTrackedDolly _trackedDolly;

        private void Awake()
        {
            _trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        }

        private void Start()
        {
            _trackedDolly.m_Path = resultCutManager.GetCurrentPattern().cameraPath;
            _trackedDolly.m_PathPosition = 0;
        }

        private void Update()
        {
            _trackedDolly.m_PathPosition += 2;
        }
    }
}