using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

/// <summary>
/// DBにタイムを記録する
/// </summary>
public class DBRecorder : MonoBehaviour, ITimeRecordable
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

    // 非同期処理で保存することになる？
}
