using AvoidGame.Play;
using AvoidGame.TimeRecorder;
using System;
using System.Collections;
using System.Collections.Generic;
using AvoidGame.Result.Interface;
using UnityEngine;
using Zenject;

namespace AvoidGame.Result
{
    /// <summary>
    /// Resultシーンを管理
    /// </summary>
    public class ResultSceneManager : IResultSceneManager, IInitializable
    {
        public event Action<ResultSceneSubState> OnSubStateChanged;

        [Inject] ITimeRecordable _timeRecordable;

        [Inject] PlayerInfo _playerInfo;

        private List<long> _records;

        public ResultSceneSubState ResultSceneSubState
        {
            get => _resultSceneSubState;
            set
            {
                _resultSceneSubState = value;
                OnSubStateChanged?.Invoke(value);
            }
        }

        private int _playerRank;

        public int PlayerRank
        {
            get => _playerRank;
            set => _playerRank = value;
        }

        private ResultSceneSubState _resultSceneSubState = ResultSceneSubState.LoadingRecord;

        public void Initialize()
        {
            _records = _timeRecordable.GetTimeRanking();
            _records.Sort();
            _playerRank = _records.IndexOf(_playerInfo.Time) + 1;
            Debug.Log($"Rank: {_playerRank}");
        }

        public (List<long> timeList, long playerTime) GetTimeData()
        {
            return (_records, _playerInfo.Time);
        }
    }
}