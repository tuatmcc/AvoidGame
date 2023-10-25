using AvoidGame.TimeRecorder;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Resultシーンを管理
/// </summary>
namespace AvoidGame.Result
{
    public class ResultSceneManager : MonoBehaviour
    {
        public event Action<ResultSceneSubState> OnSubStateChanged;

        [Inject] ITimeRecordable _timeRecordable;

        [Inject] TimeManager _timeManager;

        private List<long> _records;

        public ResultSceneSubState ResultSceneSubState
        {
            get { return _resultSceneSubState; }
            set 
            { 
                _resultSceneSubState = value;
                OnSubStateChanged?.Invoke(value);
            }

        }

        private ResultSceneSubState _resultSceneSubState = ResultSceneSubState.LoadingRecord;

        private void Awake()
        {
            _records = _timeRecordable.GetTimeRanking();
            _records.Sort();
        }

        void Start()
        {
        
        }

        void Update()
        {

        }

        public (List<long> timeList, long playerTime) GetTimeData()
        {
            return (_records, _timeManager.MainTimer);
        }
    }
}
