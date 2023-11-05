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

        [SerializeField] private float loadAtLeast = 0f;
        [SerializeField] private Canvas loadingCanvas;
        [SerializeField] private List<SceneTransitionStructure> scenes;

        private AvoidGameInputActions _inputActions;


        private void Awake()
        {
            _inputActions = new AvoidGameInputActions();
            loadingCanvas.enabled = false;
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        private void Start()
        {
            _gameStateManager.OnGameStateChanged += OnGameStateChanged;
            SceneManager.sceneLoaded += OnSceneLoaded;
            // Escでタイトルに戻る
            _inputActions.Global.ForceExit.started += (_) => ForceExit();
        }

        private void OnDestroy()
        {
            _gameStateManager.OnGameStateChanged -= OnGameStateChanged;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _inputActions.Dispose();
        }


        /// <summary>
        /// あらかじめ決められたGameStateに遷移したらシーン遷移を開始
        /// </summary>
        /// <param name="gameState"></param>
        public void OnGameStateChanged(GameState gameState)
        {
            LoadSceneAsync(gameState.ToString(), this.GetCancellationTokenOnDestroy()).Forget();
        }

        /// <summary>
        /// canvasを有効にした後ロードを開始, LoadSceneAsyncの終了を待つ
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask LoadSceneAsync(string sceneName, CancellationToken token)
        {
            loadingCanvas.enabled = true;
            // Start unloading current scene and loading next scene
            var loadAsyncResult = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            loadAsyncResult.allowSceneActivation = false;
            // Wait for at least loadAtLeast seconds
            await UniTask.Delay(TimeSpan.FromSeconds(loadAtLeast), cancellationToken: token);
            // Wait for scene loading end
            loadAsyncResult.allowSceneActivation = true;
            await loadAsyncResult;
            loadingCanvas.enabled = false;
        }

        /// <summary>
        /// ロードが終了したらcanvasを無効に
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="loadSceneMode"></param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
        }

        public void ForceExit()
        {
            Debug.Log("ForceExit");
            // to title and destroy this
            UniTask.Create(async () =>
            {
                _gameStateManager.OnGameStateChanged -= OnGameStateChanged;
                SceneManager.sceneLoaded -= OnSceneLoaded;
                _gameStateManager.GameState = GameState.Title;
                await LoadSceneAsync("Title", this.GetCancellationTokenOnDestroy());
                Destroy(gameObject);
            }).Forget();
        }
    }
}