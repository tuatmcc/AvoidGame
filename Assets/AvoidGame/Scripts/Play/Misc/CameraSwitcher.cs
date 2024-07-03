using Cinemachine;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Misc
{
    public class CameraSwitcher : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera introCamera;
        [SerializeField] private CinemachineVirtualCamera playerFollowCamera;

        [Inject] private PlaySceneManager _playSceneManager;

        private void Start()
        {
            _playSceneManager.OnCountChanged += count =>
            {
                if (count == 5)
                {
                    introCamera.Priority = 0;
                    playerFollowCamera.Priority = 20;
                }
            };
        }
    }
}