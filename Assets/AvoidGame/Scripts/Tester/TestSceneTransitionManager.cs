using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
/// <summary>
/// シーン遷移を管理する(テスト用)
/// </summary>
namespace AvoidGame.Tester
{
    public class SceneTransitionManager : MonoBehaviour
    {
        [SerializeField] private float loadAtLeast = 0f;

        [SerializeField] private GameObject _canvas;

        [SerializeField] private SceneName _from;

        [SerializeField] private SceneName _to;

        private Canvas _loadingCanvas;

        [SerializeField] private List<SceneTransitionStructure> scenes;

        [Inject] private GameStateManager _gameStateManager;


        private void Awake()
        {
            _loadingCanvas = _canvas.GetComponent<Canvas>();
            _loadingCanvas.enabled = false;

            // テスト時に無効化すべきGameObjectを無効に
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PassiveInTest"))
            {
                obj.SetActive(false);
            }
            // GameStateを開始Stateに設定してロック
            foreach (SceneTransitionStructure s in scenes)
            {
                if(s.sceneName == _from)
                {
                    _gameStateManager.GameState = s.targetState; 
                    break;
                }
            }
            _gameStateManager.LockGameState();
        }

        private void Start()
        {
            _gameStateManager.OnGameStateChanged += SceneTransition;

            SceneManager.sceneLoaded += SceneLoaded;
            StartCoroutine(LoadScene(_from.ToString()));
        }

        private void SceneTransition(GameState gameState)
        {
            foreach(SceneTransitionStructure s in scenes)
            {
                if(s.targetState == gameState)
                {
                    if(!(_from <= s.sceneName && s.sceneName <= _to))
                    {
                        Debug.Log($"Test finished at : {_to}");
                        return;
                    }
                    StartCoroutine(LoadScene(s.sceneName.ToString()));
                }
            }
        }

        private IEnumerator LoadScene(string sceneName)
        {
            float waited = 0f;
            if(sceneName == _from.ToString())
            {
                waited = loadAtLeast;
            }
            _loadingCanvas.enabled = true;
            while(waited < loadAtLeast)
            {
                yield return new WaitForSeconds(0.1f);
                waited += 0.1f;
            }
            SceneManager.LoadSceneAsync(sceneName);
        }

        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            _loadingCanvas.enabled = false;
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("PassiveInTest"))
            {
                obj.SetActive(false);
            }
            _gameStateManager.UnlockGameState();
        }
    }
}
