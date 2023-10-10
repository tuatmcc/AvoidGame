using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Resultシーンを管理
/// </summary>
public class ResultSceneManager : MonoBehaviour
{
    public event Action<ResultSceneSubState> OnSubStateChanged;
    public event Action<List<long>, long> OnRecordGot;

    // [Inject] ITimeRecordable _timeRecordable;

    private List<long> _records;

    private bool calledEvent = false;

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

    void Start()
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

    void Update()
    {
        if (!calledEvent)
        {
            OnRecordGot?.Invoke(_records, _records[1]);
            _resultSceneSubState = ResultSceneSubState.ShowDetail;

            calledEvent = true;
        }
        
    }


}
