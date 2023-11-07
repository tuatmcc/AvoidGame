using System;
using UnityEngine;
using Zenject;
using AvoidGame.TimeRecorder;

namespace AvoidGame.Play
{
    /// <summary>
    /// ゲームの時間を管理する
    /// </summary>
    public class TimeManager : MonoBehaviour
    {
        [Inject] PlaySceneManager _playSceneManager;

        [Inject] PlayerInfo _playerInfo;

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

        [Inject] private ITimeRecordable _timeRecordable;

        public void Awake()
        {
            _playSceneManager.OnPlayStateChanged += ChangeTimerCondition;
        }

        private void ChangeTimerCondition(PlaySceneState state)
        {
            switch (state)
            {
                case PlaySceneState.Countdown:
                    ResetParams();
                    break;
                case PlaySceneState.Playing:
                    StartCount();
                    break;
                case PlaySceneState.Finished:
                    StopCount();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
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
            if (counting) return;
            MainTimer = 0;
            start_time = DateTime.Now.Ticks;
            counting = true;
        }

        public void StopCount()
        {
            counting = false;
            _timeRecordable.RecordTime(MainTimer);
            _playerInfo.Time = MainTimer;
        }

        private void ResetParams()
        {
            counting = false;
            MainTimer = 0;
        }
    }
}