using System;
using System.Collections.Generic;
using System.Threading;
using AvoidGame.Calibration;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace AvoidGame.Tester
{
    /// <summary>
    /// シーン遷移を管理する(テスト用)
    /// </summary>
    public class TestSceneTransitionManager : MonoBehaviour, ISceneTransitionManager
    {
        [Inject] private GameStateManager _gameStateManager;

        [SerializeField] private float loadAtLeast = 0.5f;
        [SerializeField] private Canvas loadingCanvas;
        [SerializeField] private GameState from;
        [SerializeField] private GameState to;
        [SerializeField] private bool skipCalibration = false;
        [SerializeField] private List<GameState> scenes;

        private AvoidGameInputActions _inputActions;


        private void Awake()
        {
            _inputActions = new AvoidGameInputActions();
            loadingCanvas.enabled = false;

            // GameStateを開始Stateに設定してロック
            foreach (var s in scenes)
            {
                if (s == from)
                {
                    _gameStateManager.LockGameState(s);
                    break;
                }
            }
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        public void Start()
        {
            _gameStateManager.OnGameStateChanged += OnGameStateChanged;
            SceneManager.sceneLoaded += SceneLoaded;
            // Escでタイトルに戻る
            _inputActions.Global.ForceExit.started += (_) => ForceExit();
            LoadSceneAsync(from.ToString(), this.GetCancellationTokenOnDestroy()).Forget();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void OnDestroy()
        {
            _gameStateManager.OnGameStateChanged -= OnGameStateChanged;
            SceneManager.sceneLoaded -= SceneLoaded;
            _inputActions.Dispose();
        }

        public void OnGameStateChanged(GameState gameState)
        {
            foreach (var s in scenes)
            {
                if (s == gameState)
                {
                    if (s == GameState.Calibration && skipCalibration)
                    {
                        _gameStateManager.LockGameState(GameState.Play);
                        return;
                    }

                    if (!(from <= s && s <= to))
                    {
                        Debug.Log($"Test finished at : {to}");
                        return;
                    }

                    LoadSceneAsync(s.ToString(), this.GetCancellationTokenOnDestroy()).Forget();
                }
            }
        }

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

        public void ForceExit()
        {
            Debug.Log("Force Exit");
            // back to title
            _gameStateManager.LockGameState(GameState.Title);
        }

        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            _gameStateManager.UnlockGameState();
            // テスト時に無効化すべきGameObjectを無効に
            foreach (var obj in GameObject.FindObjectsOfType<SceneTransitionManager>())
            {
                // to avoid object duplication, destroy, not disable
                Destroy(obj.gameObject);
            }
        }
    }
}