using System;
using UnityEngine;
using Zenject;

namespace AvoidGame
{
    /// <summary>
    /// ゲームの進行管理
    /// </summary>
    public class GameStateManager : IGameStateManager, IInitializable
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
                if (gameStateLocked)
                {
                    Debug.LogWarning($"GameState is locked to {_gameState}");
                }
                else
                {
                    _gameState = value;
                    OnGameStateChanged?.Invoke(_gameState);
                }
            }
        }

        private GameState _gameState;

        public void Initialize()
        {
            GameState = GameState.Title;
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
            Debug.Log($"登録されたイベント数 : {OnGameStateChanged.GetInvocationList().Length}");
        }

        /// <summary>
        /// GameStateをロックする
        /// </summary>
        /// <returns></returns>
        public bool LockGameState(GameState gameState)
        {
            if (gameStateLocked) return false;
            GameState = gameState;
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