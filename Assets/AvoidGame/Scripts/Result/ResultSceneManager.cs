using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Resultシーンを管理
/// </summary>
namespace Result
{
    public class ResultSceneManager : MonoBehaviour
    {
        public event Action<ResultSceneSubState> OnSubStateChanged;

        // [Inject] ITimeRecordable _timeRecordable;

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
            // _records = _timeRecordable.GetTimeRanking();

            //test
            _records = new List<long>
            {
                100000000,
                200000000,
                300000000,
                400000000,
                500000000,
                600000000
            };

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
            return (_records, _records[1]);
        }
    }
}
