using System;
using UnityEngine;
using Zenject;

namespace Tracking.Mock
{
    /// <summary>
    /// ゲームの進行管理
    /// </summary>
    public class GameStateManager : MonoBehaviour
    {
        /// <summary>
        /// Stateが変わるごとに呼ばれる
        /// Stateごとに用意したほうが便利かも
        /// </summary>
        public event Action<GameState> OnGameStateChanged;

        public GameState GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
                OnGameStateChanged?.Invoke(_gameState);
            }
        }

        private GameState _gameState;

        // [Inject] private TimeManager _timeManager;

        private void Awake()
        {
            GameState = GameState.Menu;
        }

        private void Start()
        {
            OnGameStateChanged += ChangeGameState;
        }

        /// <summary>
        /// 状態が変更されたときの処理
        /// ここにはなるべく記述しないほうが良いかも
        /// </summary>
        /// <param name="gameState"></param>
        private void ChangeGameState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Menu:
                    Debug.Log("GameState Changed to : Menu");
                    break;
                case GameState.Calibrating:
                    Debug.Log("GameState Changed to : Calibration");
                    break;
                case GameState.Playing:
                    Debug.Log("GameState Changed to : Playing");
                    break;
                case GameState.Result:
                    Debug.Log("GameState Changed to : Result");
                    break;
            }
        }
    }
}