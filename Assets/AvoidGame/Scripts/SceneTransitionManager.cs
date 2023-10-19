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
        [SerializeField] private float loadAtLeast = 0f;

        [SerializeField] private GameObject _canvas;

        private Canvas _loadingCanvas;

        [SerializeField] private List<SceneTransitionStructure> scenes;

        [Inject] private GameStateManager _gameStateManager;

        private bool sceneLoaded = false;

        private void Awake()
        {
            _loadingCanvas = _canvas.GetComponent<Canvas>();
            _loadingCanvas.enabled = false;
        }

        private void Start()
        {
            _gameStateManager.OnGameStateChanged += SceneTransition;
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneTransition(GameState gameState)
        {
            foreach(SceneTransitionStructure s in scenes)
            {
                if(s.targetState == gameState)
                {
                    StartCoroutine(LoadScene(s.sceneName));
                }
            }
        }

        private IEnumerator LoadScene(string sceneName)
        {
            float waited = 0f;
            _loadingCanvas.enabled = true;
            SceneManager.LoadSceneAsync(sceneName);
            while(waited < loadAtLeast || !sceneLoaded)
            {
                yield return new WaitForSeconds(0.1f);
                waited += 0.1f;
            }
            _loadingCanvas.enabled = false;
            sceneLoaded = false;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            sceneLoaded = true;
        }
    }
}
