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

        private int _index;


        public struct Pattern
        {
            public AnimationClip faceAnimationClip;
            public AnimationClip bodyAnimationClip;
            public CinemachinePath cameraPath;
        }

        private void Awake()
        {
            if (debug)
            {
                _index = debugIndex;
                return;
            }

            _index = Random.Range(0,
                Mathf.Min(faceAnimationClips.Length, faceAnimationClips.Length, faceAnimationClips.Length));
        }

        public Pattern GetCurrentPattern()
        {
            return new Pattern
            {
                cameraPath = cameraPaths[_index],
                faceAnimationClip = faceAnimationClips[_index],
                bodyAnimationClip = bodyAnimationClips[_index]
            };
        }
    }
}