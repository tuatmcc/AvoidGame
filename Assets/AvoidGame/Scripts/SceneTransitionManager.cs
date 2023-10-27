using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
/// <summary>
/// シーン遷移を管理する
/// </summary>
namespace AvoidGame
{
    public class SceneTransitionManager : MonoBehaviour
    {
        [SerializeField] protected private float loadAtLeast = 0f;

        [SerializeField] protected private GameObject _canvas;

        protected private Canvas _loadingCanvas;

        [SerializeField] protected private List<SceneTransitionStructure> scenes;

        [Inject] protected private GameStateManager _gameStateManager;

        /// <summary>
        /// ロード画面用canvasを取得
        /// </summary>
        virtual public void Awake()
        {
            _loadingCanvas = _canvas.GetComponent<Canvas>();
            _loadingCanvas.enabled = false;
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        virtual public void Start()
        {
            _gameStateManager.OnGameStateChanged += SceneTransition;
            SceneManager.sceneLoaded += SceneLoaded;
        }

        /// <summary>
        /// あらかじめ決められたGameStateに遷移したらシーン遷移を開始
        /// </summary>
        /// <param name="gameState"></param>
        protected virtual private void SceneTransition(GameState gameState)
        {
            foreach(SceneTransitionStructure s in scenes)
            {
                if(s.targetState == gameState)
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
        protected virtual private IEnumerator LoadScene(string sceneName)
        {
            float waited = 0f;
            _loadingCanvas.enabled = true;
            while(waited < loadAtLeast)
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
        protected virtual private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            _loadingCanvas.enabled = false;
        }
    }
}
