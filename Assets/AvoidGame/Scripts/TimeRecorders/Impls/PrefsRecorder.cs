using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// UnityPlayerPrefsにタイムを保存する
/// </summary>
public class PrefsRecorder : ITimeRecordable
{
    public event Action OnTimeRecorded;

    public List<float> GetTimeRanking(int num)
    {
        throw new System.NotImplementedException();
    }

    public void RecordTime(float time)
    { 
        OnTimeRecorded?.Invoke();
    }

    
}
