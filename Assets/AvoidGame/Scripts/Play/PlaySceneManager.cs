using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play
{
    public class PlaySceneManager : MonoBehaviour
    {
        public event Action<int> OnCountChanged;

        [SerializeField] private float waitForCountdown = 0f;
        [SerializeField] private int countdown = 0;

        [Inject] private GameStateManager _gameStateManager;

        void Start()
        {
            StartCoroutine(WaitForCountdown());
        }

        private IEnumerator WaitForCountdown()
        {
            yield return new WaitForSeconds(waitForCountdown);
            StartCoroutine(CountDown());
        }

        private IEnumerator CountDown()
        {
            var count = countdown;
            OnCountChanged?.Invoke(count);
            while (count > 0)
            {
                yield return new WaitForSeconds(1);
                count--;
                OnCountChanged?.Invoke(count);
            }
            _gameStateManager.GameState = GameState.Playing;
        }
    }
}
