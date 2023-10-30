using System;
using UnityEngine;
using Zenject;

namespace AvoidGame
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

        private bool gameStateLocked = false;

        public GameState GameState
        {
            get => _gameState;
            set
            {
                if (!gameStateLocked)
                {
                    _gameState = value;
                    OnGameStateChanged?.Invoke(_gameState);
                }
            }
        }

        private GameState _gameState;

        [Inject] private TimeManager _timeManager;

        private void Awake()
        {
            GameState = GameState.Title;
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
            Debug.Log($"GameState Changhed to : {gameState}");
        }

        /// <summary>
        /// GameStateをロックする
        /// </summary>
        /// <returns></returns>
        public bool LockGameState()
        {
            if (gameStateLocked) return false;
            gameStateLocked = true;
            return true;
        }

        public bool UnlockGameState()
        {
            if (!gameStateLocked) return false;
            gameStateLocked = false;
            return true;
        }
    }
}