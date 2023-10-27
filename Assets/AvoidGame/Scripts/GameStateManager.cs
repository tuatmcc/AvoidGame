using AvoidGame.Calibration;
using Cysharp.Threading.Tasks;
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

        public GameState GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
                OnGameStateChanged?.Invoke(_gameState);
            }
        }

        public RetargetController RetargetController { get; set; }
        public Receiver Receiver { get; private set; }


        private GameState _gameState;

        [Inject] private TimeManager _timeManager;

        private void Awake()
        {
            GameState = GameState.Title;
            Receiver = new Receiver();
            RetargetController = new RetargetController();
        }

        private void Start()
        {
            OnGameStateChanged += ChangeGameState;

            // Start Receiver
            var token = this.GetCancellationTokenOnDestroy();
            Receiver.StartReceiver(token).Forget();
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
                case GameState.Title:
                    Debug.Log("GameState Changhed to : Title");
                    break;
                case GameState.Calibration:
                    Debug.Log("GameState Changhed to : Calibration");
                    break;
                case GameState.CountDown:
                    Debug.Log("GameState Changhed to : CountDown");
                    break;
                case GameState.Playing:
                    Debug.Log("GameState Changhed to : Playing");
                    break;
                case GameState.Finished:
                    Debug.Log("GameState Changhed to : Finished");
                    break;
                case GameState.Result:
                    Debug.Log("GameState Changhed to : Result");
                    break;
            }
        }
    }
}