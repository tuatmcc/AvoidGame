using System;
using System.Collections.Generic;
using System.Threading;
using AvoidGame.Calibration;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace AvoidGame
{
    /// <summary>
    /// シーン遷移を管理する
    /// </summary>
    public class SceneTransitionManager : MonoBehaviour, ISceneTransitionManager
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private AvoidGameInputActions _inputActions;

        [SerializeField] private float loadAtLeast = 0f;
        [SerializeField] private Canvas canvas;
        [SerializeField] private List<SceneTransitionStructure> scenes;


        private void Awake()
        {
            canvas.enabled = false;
            _inputActions.Enable();
            _inputActions.UI.ForceExit.started += (ctx) =>
            {
                Debug.Log("ForceExit");
                ForceExit();
            };
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        private void Start()
        {
            _gameStateManager.OnGameStateChanged += SceneTransition;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void ForceExit()
        {
            // to title
            _gameStateManager.GameState = GameState.Title;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ForceExit();
            }
        }

        private void OnDestroy()
        {
            _gameStateManager.OnGameStateChanged -= SceneTransition;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _inputActions.Disable();
        }

        /// <summary>
        /// あらかじめ決められたGameStateに遷移したらシーン遷移を開始
        /// </summary>
        /// <param name="gameState"></param>
        private void SceneTransition(GameState gameState)
        {
            foreach (SceneTransitionStructure s in scenes)
            {
                if (s.targetState == gameState)
                {
                    // fire and forget
                    LoadScene(s.sceneName.ToString(), default).Forget();
                }
            }
        }

        /// <summary>
        /// canvasを有効にした後ロードを開始, LoadSceneAsyncの終了を待つ
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask LoadScene(string sceneName, CancellationToken token)
        {
            canvas.enabled = true;
            var asyncResult = SceneManager.LoadSceneAsync(sceneName);
            asyncResult.allowSceneActivation = false;
            await UniTask.Delay(TimeSpan.FromSeconds(loadAtLeast), cancellationToken: token);
            while (!asyncResult.isDone)
            {
                if (asyncResult.progress >= 0.9f)
                {
                    asyncResult.allowSceneActivation = true;
                }

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

        /// <summary>
        /// ロードが終了したらcanvasを無効に
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="loadSceneMode"></param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            canvas.enabled = false;
            _gameStateManager.UnlockGameState();
        }
    }
}