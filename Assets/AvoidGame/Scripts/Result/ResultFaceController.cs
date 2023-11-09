using System;
using System.Threading;
using AvoidGame.Result.Interface;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using Zenject;

namespace AvoidGame.Result
{
    public class ResultFaceController : MonoBehaviour
    {
        [Inject] private IResultSceneManager _sceneManager;

        [SerializeField] private ResultCutManager resultCutManager;
        [SerializeField] private Animator animator;
        private AnimationClipPlayable _playable;
        private AnimationPlayableOutput _playableOutput;
        private PlayableGraph _graph;

        private void Start()
        {
            _graph = PlayableGraph.Create(name);
            var clip = resultCutManager.GetCurrentPattern().faceAnimationClip;
            _playable = AnimationClipPlayable.Create(_graph, clip);
            AnimationPlayableOutput.Create(_graph, name, animator);
            var output = _graph.GetOutput(0);
            output.SetSourcePlayable(_playable);
            _graph.Play();
        }
    }
}