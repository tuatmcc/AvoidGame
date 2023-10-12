using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// UnityPlayerPrefsにタイムを保存する
/// </summary>
public class PrefsRecorder : MonoBehaviour, ITimeRecordable
{
    public event Action OnTimeRecorded;

    private void Start()
    {
        
    }

    public List<long> GetTimeRanking()
    {
        throw new System.NotImplementedException();
    }

    public void RecordTime(long time)
    { 
        OnTimeRecorded?.Invoke();
    }

    
}
