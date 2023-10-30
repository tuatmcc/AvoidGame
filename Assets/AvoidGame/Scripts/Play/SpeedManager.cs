using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play
{
    /// <summary>
    /// ゲーム中のスピード()倍率を管理する
    /// </summary>
    public class SpeedManager : MonoBehaviour
    {
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
                OnSpeedChanged?.Invoke(Speed);
            }
        }

        private float _speed;

        private void Start()
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
            if(Speed < 0.1f)
            {
                Speed = 0.1f;
            }
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

        private void OnDisable()
        {
            _gameStateManager.OnGameStateChanged -= PlayStart;
        }
    }
}
