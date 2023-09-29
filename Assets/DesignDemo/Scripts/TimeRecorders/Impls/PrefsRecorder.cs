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

    public List<float> GetTimeRanking(int num)
    {
        throw new System.NotImplementedException();
    }

    public void RecordTime(float time)
    { 
        OnTimeRecorded?.Invoke();
    }

    
}
