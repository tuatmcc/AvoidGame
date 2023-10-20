using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignDemo
{
    /// <summary>
    /// ゲーム中のスピードを管理する
    /// </summary>
    public class SpeedManager : MonoBehaviour
    {
        public event Action<float> OnSpeedChanged;

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
            Speed = 1.0f;
        }

        /// <summary>
        /// スピードを変更する処理
        /// </summary>
        /// <param name="add"></param>
        public void AddPlayerSpeed(float add)
        {
            Speed += add;
        }
    }
}
