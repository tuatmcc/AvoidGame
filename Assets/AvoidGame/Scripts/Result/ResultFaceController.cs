using System;
using System.Threading;
using AvoidGame.Result.Interface;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace AvoidGame.Result
{
    public class ResultFaceController : MonoBehaviour
    {
        [Inject] private IResultSceneManager _sceneManager;
        [SerializeField] private SkinnedMeshRenderer face;
        [SerializeField] private AnimationClip openEyes;
        [SerializeField] private AnimationClip closeEyes;
        [SerializeField] private AnimationClip disappointed;
        [SerializeField] private AnimationClip pleased;
        [SerializeField] private Animation faceAnimation;

        private void Awake()
        {
        }

        private void Start()
        {
            switch (_sceneManager.PlayerRank)
            {
                case 1:
                    faceAnimation.Play("Pleased");
                    break;
                case 2:
                    break;
                default:
                    faceAnimation.Play("Disappointed");
                    break;
            }
        }

        private async UniTask MakeSadFace(CancellationToken token)
        {
            await UniTask.WaitForSeconds(0, cancellationToken: token);
        }
    }
}