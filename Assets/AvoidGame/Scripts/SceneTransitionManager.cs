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
        private CancellationTokenSource _sceneLoadCts;

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
            _sceneLoadCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
            LoadSceneAsync(gameState.ToString(), _sceneLoadCts.Token).Forget();
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
            try
            {
                // Wait for at least loadAtLeast seconds
                await UniTask.Delay(TimeSpan.FromSeconds(loadAtLeast), cancellationToken: token);
                // Wait for scene loading end
                loadAsyncResult.allowSceneActivation = true;
                await loadAsyncResult;
                loadingCanvas.enabled = false;
            }
            catch (Exception e)
            {
                // even if loading is cancelled, scene loading should be finished
                // so that the scene loading will not remain in the background
                loadAsyncResult.allowSceneActivation = true;
                await loadAsyncResult;
                loadingCanvas.enabled = false;
            }
        }

        /// <summary>
        /// ロードが終了したらcanvasを無効に
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="loadSceneMode"></param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
        }

        /// <summary>
        /// Back to Title and destroy this object
        /// </summary>
        public void ForceExit()
        {
            Debug.Log("ForceExit");
            // immediately finish scene loading (if not finished)
            _sceneLoadCts?.Cancel();
            // UniTask is not connected to MonoBehaviour lifecycle
            UniTask.Create(async () =>
            {
                // avoid double force exit
                _inputActions.Disable();
                // remove OnGameStateChanged event before changing GameState
                _gameStateManager.OnGameStateChanged -= OnGameStateChanged;
                _gameStateManager.GameState = GameState.Title;
                // remove OnSceneLoaded event before loading scene
                SceneManager.sceneLoaded -= OnSceneLoaded;
                await SceneManager.LoadSceneAsync(SceneName.Title.ToString());
                Destroy(gameObject);
            }).Forget();
        }
    }
}