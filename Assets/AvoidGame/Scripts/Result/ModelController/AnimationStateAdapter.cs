using AvoidGame.Result.Interface;
using UnityEngine;
using Zenject;

namespace AvoidGame.Result.ModelController
{
    [RequireComponent(typeof(Animator))]
    public class AnimationStateAdapter : MonoBehaviour
    {
        [Inject] IResultSceneManager _sceneManager;

        [SerializeField] private Animator animator;
        private static readonly int Ranking = Animator.StringToHash("Ranking");

        private void Start()
        {
            ChangeAnimation();
        }

        private void ChangeAnimation()
        {
            animator.SetInteger(Ranking, _sceneManager.PlayerRank);
        }
    }
}