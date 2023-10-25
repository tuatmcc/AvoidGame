using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Test.UI
{
    /// <summary>
    /// パネルの基底クラス
    /// GameStateに応じたUIの表示・非表示を行う
    /// TargetStateに指定されたGameStateのときにのみ表示される
    /// </summary>
    public abstract class PanelBase : MonoBehaviour
    {
        public virtual GameState TargetState { get; }
    
        [Inject] private GameStateManager _gameStateManager;
    
        private void Awake()
        {
            _gameStateManager.OnGameStateChanged += ChangePanelActivation;
        }

        private void ChangePanelActivation(GameState gameState)
        {
            gameObject.SetActive(gameState == TargetState);
        }

        private void OnDisable()
        {
            _gameStateManager.OnGameStateChanged -= ChangePanelActivation;
        }
    }
}