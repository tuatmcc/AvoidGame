using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play
{
    public class PlaySceneManager : MonoBehaviour
    {
        public event Action OnCountStart;
        public event Action<int> OnCountChanged;

        [SerializeField] private float waitForCountdown = 0f;
        [SerializeField] private int countdown = 0;
        [SerializeField] private float waitAfterFinished = 0f;

        [Inject] private GameStateManager _gameStateManager;

        void Start()
        {
            StartCoroutine(Countdown());
        }

        private IEnumerator Countdown()
        {
            yield return new WaitForSeconds(waitForCountdown);

            var count = countdown;
            OnCountStart?.Invoke();
            while (count > 0)
            {
                OnCountChanged?.Invoke(count);
                yield return new WaitForSeconds(1);
                count--;
            }
            _gameStateManager.GameState = GameState.Playing;
        }

        public void Finished()
        {
            _gameStateManager.GameState = GameState.Finished;
            StartCoroutine(TransitToResult());
        }

        private IEnumerator TransitToResult()
        {
            yield return new WaitForSeconds(waitAfterFinished);
            _gameStateManager.GameState = GameState.Result;
        }
    }
}
