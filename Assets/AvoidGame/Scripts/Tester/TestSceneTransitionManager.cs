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
        [SerializeField] private SceneName from;
        [SerializeField] private SceneName to;
        [SerializeField] private bool skipCalibration = false;
        [SerializeField] private List<SceneTransitionStructure> scenes;

        private AvoidGameInputActions _inputActions;


        private void Awake()
        {
            _inputActions = new AvoidGameInputActions();
            loadingCanvas.enabled = false;

            // GameStateを開始Stateに設定してロック
            foreach (SceneTransitionStructure s in scenes)
            {
                if (s.sceneName == from)
                {
                    _gameStateManager.LockGameState(s.targetState);
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
            foreach (SceneTransitionStructure s in scenes)
            {
                if (s.targetState == gameState)
                {
                    if (s.sceneName == SceneName.Calibration && skipCalibration)
                    {
                        _gameStateManager.LockGameState(GameState.CountDown);
                        return;
                    }

                    if (!(from <= s.sceneName && s.sceneName <= to))
                    {
                        Debug.Log($"Test finished at : {to}");
                        return;
                    }

                    LoadSceneAsync(s.sceneName.ToString(), this.GetCancellationTokenOnDestroy()).Forget();
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
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PassiveInTest"))
            {
                Destroy(obj);
            }
        }
    }
}