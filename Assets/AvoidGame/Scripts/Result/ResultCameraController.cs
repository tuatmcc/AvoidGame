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
        [SerializeField] private CinemachinePath path1;
        [SerializeField] private CinemachinePath path2;
        [SerializeField] private CinemachinePath path3;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private CinemachineTrackedDolly _trackedDolly;

        private void Awake()
        {
            _trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        }

        private void Start()
        {
            if (_sceneManager.PlayerRank == 1)
            {
                _trackedDolly.m_Path = path1;
            }
            else if (_sceneManager.PlayerRank == 2)
            {
                _trackedDolly.m_Path = path2;
            }
            else
            {
                _trackedDolly.m_Path = path1;
            }

            _trackedDolly.m_PathPosition = 0;
        }

        private void Update()
        {
            _trackedDolly.m_PathPosition += Time.deltaTime * 2f;
        }
    }
}