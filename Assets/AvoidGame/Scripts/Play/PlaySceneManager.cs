using System;
using System.Collections;
using AvoidGame.Play.Interface;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play
{
    /// <summary>
    /// Playシーンを管理する
    /// </summary>
    public class PlaySceneManager : MonoBehaviour, IPlaySceneManager
    {
        /// <summary>
        /// カウントダウン時のイベント
        /// </summary>
        public event Action OnCountStart;

        public event Action<int> OnCountChanged;

        [SerializeField] private float waitForCountdown = 0f;
        [SerializeField] private int countdown = 0;
        [SerializeField] private float waitAfterFinished = 0f;

        [Inject] private GameStateManager _gameStateManager;

        private PlaySceneState _sceneState = PlaySceneState.Countdown;

        public event Action<PlaySceneState> OnPlayStateChanged;

        public PlaySceneState State
        {
            get => _sceneState;
            private set
            {
                OnPlayStateChanged?.Invoke(value);
                _sceneState = value;
            }
        }

        void Start()
        {
            StartCoroutine(Countdown());
        }

        /// <summary>
        /// 一定時間待ってカウントダウンを開始
        /// </summary>
        /// <returns></returns>
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

            OnCountChanged?.Invoke(count);
            State = PlaySceneState.Playing;
        }

        /// <summary>
        /// Playerから呼び出される
        /// GameStateの変更は各Manageクラスが行うようにしている
        /// </summary>
        public void Finished()
        {
            State = PlaySceneState.Finished;
            StartCoroutine(TransitToResult());
        }

        /// <summary>
        /// ゴール後一定時間待ってリザルトへ
        /// </summary>
        /// <returns></returns>
        private IEnumerator TransitToResult()
        {
            yield return new WaitForSeconds(waitAfterFinished);
            _gameStateManager.GameState = GameState.Result;
        }
    }
}