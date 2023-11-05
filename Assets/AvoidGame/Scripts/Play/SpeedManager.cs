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
        [Inject] PlaySceneManager _playSceneManager;

        /// <summary>
        /// スピード倍率
        /// </summary>
        private float Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                OnSpeedMultiplierChanged?.Invoke(_speed);
                OnSpeedChanged?.Invoke(_speed * PlayerConstants.default_player_speed);
            }
        }

        private float _speed;

        public void Initialize()
        {
            Speed = 0f;
            _playSceneManager.OnPlayStateChanged += PlayStart;
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
        /// <param name="sceneState"></param>
        private void PlayStart(PlaySceneState sceneState)
        {
            if (sceneState == PlaySceneState.Playing)
            {
                Speed = 1f;
            }
        }

        public void Dispose()
        {
            _playSceneManager.OnPlayStateChanged -= PlayStart;
        }
    }
}