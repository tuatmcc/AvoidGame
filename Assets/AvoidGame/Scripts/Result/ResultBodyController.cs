using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace AvoidGame.Result
{
    public class ResultBodyController : MonoBehaviour
    {
        [SerializeField] private ResultCutManager resultCutManger;
        [SerializeField] private Animator animator;
        private AnimationClipPlayable _playable;
        private AnimationPlayableOutput _playableOutput;
        private PlayableGraph _graph;

        private void Start()
        {
            _graph = PlayableGraph.Create(name);
            _playable = AnimationClipPlayable.Create(_graph, resultCutManger.GetCurrentPattern().bodyAnimationClip);
            AnimationPlayableOutput.Create(_graph, name, animator);
            var output = _graph.GetOutput(0);
            output.SetSourcePlayable(_playable);
            _graph.Play();
        }
    }
}