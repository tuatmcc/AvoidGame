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

        [SerializeField] private ResultCutManager resultCutManager;
        [SerializeField] private Animation faceAnimation;


        private void Start()
        {
            var pattern = resultCutManager.GetCurrentPattern();
            faceAnimation.clip = pattern.faceAnimationClip;
            faceAnimation.Play();
        }
    }
}