using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace AvoidGame.Result
{
    public class ResultCutManager : MonoBehaviour
    {
        [SerializeField] private bool debug = false;
        [SerializeField] private int debugIndex = 0;
        [SerializeField] private AnimationClip[] faceAnimationClips;
        [SerializeField] private AnimationClip[] bodyAnimationClips;
        [SerializeField] private CinemachinePath[] cameraPaths;
        [SerializeField] private GameObject[] activationObjects;

        private int _index;


        public struct Pattern
        {
            public AnimationClip faceAnimationClip;
            public AnimationClip bodyAnimationClip;
            public CinemachinePath cameraPath;
            public GameObject activationObject;
        }

        private void Awake()
        {
            if (debug)
            {
                _index = debugIndex;
                return;
            }

            _index = Random.Range(0,
                Mathf.Min(faceAnimationClips.Length, faceAnimationClips.Length, faceAnimationClips.Length,
                    activationObjects.Length));
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