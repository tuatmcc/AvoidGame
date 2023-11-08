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
            if (count == 3)
                animation.Play();
        }
    }
}