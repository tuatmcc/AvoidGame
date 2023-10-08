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
    public event Action<List<long>, long> OnRecordGot;

    // [Inject] ITimeRecordable _timeRecordable;

    private List<long> _records;

    private bool calledEvent = false;

    void Start()
    {
        // _records = _timeRecordable.GetTimeRanking();

        //test
        _records = new List<long>
        {
            100000000,
            200000000,
            300000000
        };

        _records.Sort();
    }

    void Update()
    {
        if (!calledEvent)
        {
            OnRecordGot?.Invoke(_records, _records[1]);
            calledEvent = true;
        }
        
    }


}
