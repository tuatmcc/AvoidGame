using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.SceneManagement;
using Zenject;
using AvoidGame.System.TimeRecorder;

/// <summary>
/// ゲームの時間を管理する
/// </summary>
namespace AvoidGame
{
    public class TimeManager : MonoBehaviour
    {
        private bool counting = false;

        private long start_time;

        public event Action<TimeSpan> OnTimeChanged;

        public long MainTimer
        {
            get => _mainTimer;
            set
            {
                _mainTimer = value;
                OnTimeChanged?.Invoke(new TimeSpan(MainTimer));
            }
        }

        private long _mainTimer;

        [Inject] private GameStateManager _gameStateManager;
        private ITimeRecordable _timeRecordable;

        public void Start()
        {
            _gameStateManager.OnGameStateChanged += ChangeCount;
            // _timeRecordable = GameObject.Find("Static").GetComponent<ITimeRecordable>();
        }

        private void ChangeCount(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Playing:
                    StartCount();
                    break;
                case GameState.Result:
                    StopCount(); 
                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            if (counting)
            {
                MainTimer = DateTime.Now.Ticks - start_time;
            }
        }

        public void StartCount()
        {
            if(counting) return; 
            MainTimer = 0;
            start_time = DateTime.Now.Ticks;
            counting = true;
        }

        public void StopCount()
        {
            _timeRecordable.RecordTime(MainTimer);
            counting = false;
        }
    }
}