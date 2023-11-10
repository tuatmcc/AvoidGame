using System.Collections.Generic;
using AvoidGame.Result.Interface;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace AvoidGame.Result
{
    public class ResultCutManager : MonoBehaviour
    {
        [Inject] private IResultSceneManager _sceneManager;
        [SerializeField] private bool debug = false;
        [SerializeField] private int debugIndex = 0;
        [SerializeField] private AnimationClip[] faceAnimationClips;
        [SerializeField] private AnimationClip[] bodyAnimationClips;
        [SerializeField] private CinemachinePath[] cameraPaths;
        [SerializeField] private GameObject[] activationObjects;

        private int _index;
        private int _inappropriateCountFromBack = 1;


        public struct Pattern
        {
            public AnimationClip faceAnimationClip;
            public AnimationClip bodyAnimationClip;
            public CinemachinePath cameraPath;
            public GameObject activationObject;
        }

        private void Validate()
        {
            var len = faceAnimationClips.Length;
            if (len != bodyAnimationClips.Length || len != cameraPaths.Length || len != activationObjects.Length)
            {
                Debug.LogError("Result Cut Patterns' Length Miss Match!");
            }
        }

        private void Awake()
        {
            Validate();
            if (debug)
            {
                _index = debugIndex;
                return;
            }

            var len = faceAnimationClips.Length;
            if (_sceneManager.PlayerRank <= 5)
            {
                _index = Random.Range(0, len - _inappropriateCountFromBack);
            }
            else
            {
                _index = Random.Range(0, len);
            }
        }

        public Pattern GetCurrentPattern()
        {
            return new Pattern
            {
                cameraPath = cameraPaths[_index],
                faceAnimationClip = faceAnimationClips[_index],
                bodyAnimationClip = bodyAnimationClips[_index],
                activationObject = activationObjects[_index]
            };
        }
    }
}