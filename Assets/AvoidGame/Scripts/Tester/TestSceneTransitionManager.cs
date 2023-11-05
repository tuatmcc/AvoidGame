using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using AvoidGame.Calibration;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Zenject;

namespace AvoidGame.Tester
{
    /// <summary>
    /// シーン遷移を管理する(テスト用)
    /// </summary>
    public class TestSceneTransitionManager : MonoBehaviour, ISceneTransitionManager
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private AvoidGameInputActions _inputActions;

        [SerializeField] private float loadAtLeast = 0.5f;
        [SerializeField] private Canvas loadingCanvas;
        [SerializeField] private SceneName from;
        [SerializeField] private SceneName to;
        [SerializeField] private bool skipCalibration = false;
        [SerializeField] private List<SceneTransitionStructure> scenes;

        private CancellationTokenSource _cts;

        private void Awake()
        {
            // _loadingCanvas = canvas.GetComponent<Canvas>();
            // _loadingCanvas.enabled = false;
            loadingCanvas.enabled = false;
            // テスト時に無効化すべきGameObjectを無効に
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PassiveInTest"))
            {
                obj.SetActive(false);
            }

            // GameStateを開始Stateに設定してロック
            foreach (SceneTransitionStructure s in scenes)
            {
                if (s.sceneName == from)
                {
                    _gameStateManager.LockGameState(s.targetState);
                    break;
                }
            }

            // Escでタイトルに戻る
            _inputActions.Enable();
            _inputActions.UI.ForceExit.started += (ctx) => ForceExit();
        }

        public void Start()
        {
            _gameStateManager.OnGameStateChanged += SceneTransition;
            SceneManager.sceneLoaded += SceneLoaded;
            LoadScene(from.ToString(), default).Forget();
        }

        private void OnDestroy()
        {
            _gameStateManager.OnGameStateChanged -= SceneTransition;
            SceneManager.sceneLoaded -= SceneLoaded;
            _inputActions.Disable();
        }

        private void SceneTransition(GameState gameState)
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

                    LoadScene(s.sceneName.ToString(), default).Forget();
                }
            }
        }

        private async UniTask LoadScene(string sceneName, CancellationToken token)
        {
            loadingCanvas.enabled = true;
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

        public void ForceExit()
        {
            Debug.Log("Force Exit");
            // back to title
            _gameStateManager.GameState = GameState.Title;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            loadingCanvas.enabled = false;
            _gameStateManager.UnlockGameState();
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PassiveInTest"))
            {
                obj.SetActive(false);
            }
        }
    }
}