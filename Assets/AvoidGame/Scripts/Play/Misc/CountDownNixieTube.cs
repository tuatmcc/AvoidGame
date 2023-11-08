using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace AvoidGame.Play.Misc
{
    public class CountDownNixieTube : MonoBehaviour
    {
        [Inject] private PlaySceneManager _playSceneManager;

        [SerializeField] private Animation animation;

        private void Start()
        {
            _playSceneManager.OnCountChanged += UpdateCount;
        }

        private void UpdateCount(int count)
        {
            // NixieTube Needs pre-transition before 3, 2, 1, 0, so animation was created with 4 seconds.
            if (count == 4)
                animation.Play();
        }
    }
}