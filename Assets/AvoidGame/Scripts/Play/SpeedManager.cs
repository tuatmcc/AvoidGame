using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using AvoidGame.Play.Player;

namespace AvoidGame.Play
{
    /// <summary>
    /// ゲーム中のスピード()倍率を管理する
    /// </summary>
    public class SpeedManager : ISpeedManager, IInitializable, IDisposable
    {
        public event Action<float> OnSpeedMultiplierChanged;
        public event Action<float> OnSpeedChanged;
        [Inject] GameStateManager _gameStateManager;

        /// <summary>
        /// スピード倍率
        /// </summary>
        public float Speed 
        {
            get => _speed;
            set 
            {
                _speed = value;
                OnSpeedMultiplierChanged?.Invoke(_speed);
                OnSpeedChanged?.Invoke(_speed*PlayerConstants.default_player_speed);
            }
        }

        private float _speed;

        public void Initialize()
        {
            Speed = 0f;
            _gameStateManager.OnGameStateChanged += PlayStart;
        }

        /// <summary>
        /// スピードを変更する処理
        /// 現状0.1を下回らないように
        /// </summary>
        /// <param name="add"></param>
        public void AddPlayerSpeed(float add)
        {
            Speed += add;
            Speed = Math.Max(Speed, PlayerConstants.min_speed_multiplier);
        }

        /// <summary>
        /// 開始時に倍率を1.0に
        /// </summary>
        /// <param name="gameState"></param>
        private void PlayStart(GameState gameState)
        {
            if(gameState == GameState.Playing)
            {
                Speed = 1f;
            }
        }

        public void Dispose()
        {
            _gameStateManager.OnGameStateChanged -= PlayStart;
        }
    }
}
