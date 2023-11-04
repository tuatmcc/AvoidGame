using System.Collections;
using System.Collections.Generic;
using AvoidGame.Calibration;
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
        [SerializeField] private float loadAtLeast = 0f;

        [SerializeField] private GameObject canvas;

        private Canvas _loadingCanvas;

        [SerializeField] private List<SceneTransitionStructure> scenes;

        [Inject] private GameStateManager _gameStateManager;

        /// <summary>
        /// ロード画面用canvasを取得
        /// </summary>
        private void Awake()
        {
            _loadingCanvas = canvas.GetComponent<Canvas>();
            _loadingCanvas.enabled = false;
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        private void Start()
        {
            RegisterEvents();
        }

        public void RegisterEvents()
        {
            _gameStateManager.OnGameStateChanged += SceneTransition;
            SceneManager.sceneLoaded += SceneLoaded;
        }

        virtual public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToTitle();
            }
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
                    StartCoroutine(LoadScene(s.sceneName.ToString()));
                }
            }
        }

        /// <summary>
        /// canvasを有効にした後loadAtLeast時間待ってからロードを開始
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public IEnumerator LoadScene(string sceneName)
        {
            float waited = 0f;
            _loadingCanvas.enabled = true;
            while (waited < loadAtLeast)
            {
                yield return new WaitForSeconds(0.1f);
                waited += 0.1f;
            }

            SceneManager.LoadSceneAsync(sceneName);
        }

        /// <summary>
        /// ロードが終了したらcanvasを無効に
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="loadSceneMode"></param>
        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            _loadingCanvas.enabled = false;
            _gameStateManager.UnlockGameState();
        }

        public void ToTitle()
        {
            _gameStateManager.LockGameState(GameState.Title);
        }
    }
}