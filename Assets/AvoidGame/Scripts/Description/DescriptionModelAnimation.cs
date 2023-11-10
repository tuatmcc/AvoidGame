using UnityEngine;
using Zenject;

namespace AvoidGame.Description
{
    [RequireComponent(typeof(Animator))]
    public class DescriptionModelAnimation : MonoBehaviour
    {
        [Inject] private IDescriptionSceneManager _sceneManager;
        [SerializeField] private Animator animator;

        private readonly int _state = Animator.StringToHash("State");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            animator.SetInteger(_state, (int)_sceneManager.State);

            _sceneManager.OnStateChanged += ChangeAnimation;
        }

        private void ChangeAnimation(DescriptionState state)
        {
            animator.SetInteger(_state, (int)state);
        }

        private void OnDestroy()
        {
            _sceneManager.OnStateChanged -= ChangeAnimation;
        }
    }
}