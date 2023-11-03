using System.Collections;
using System.Collections.Generic;
using AvoidGame.Calibration;
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

        [SerializeField] private float loadAtLeast = 0f;
        [SerializeField] private GameObject canvas;
        [SerializeField] private SceneName from;
        [SerializeField] private SceneName to;
        [SerializeField] private bool skipCalibration = false;
        [SerializeField] private List<SceneTransitionStructure> scenes;

        private Canvas _loadingCanvas;

        private void Awake()
        {
            _loadingCanvas = canvas.GetComponent<Canvas>();
            _loadingCanvas.enabled = false;
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
        }

        public void Start()
        {
            RegisterEvents();
            StartCoroutine(LoadScene(from.ToString()));
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

                    StartCoroutine(LoadScene(s.sceneName.ToString()));
                }
            }
        }

        public IEnumerator LoadScene(string sceneName)
        {
            float waited = 0f;
            if (sceneName == from.ToString())
            {
                waited = loadAtLeast;
            }

            _loadingCanvas.enabled = true;
            while (waited < loadAtLeast)
            {
                yield return new WaitForSeconds(0.1f);
                waited += 0.1f;
            }

            SceneManager.LoadSceneAsync(sceneName);
        }

        public void RegisterEvents()
        {
            _gameStateManager.OnGameStateChanged += SceneTransition;
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            _loadingCanvas.enabled = false;
            _gameStateManager.UnlockGameState();
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PassiveInTest"))
            {
                obj.SetActive(false);
            }
        }
    }
}