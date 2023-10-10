using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

/// <summary>
/// 指定された順位のタイムを表示する
/// </summary>
public class TimePresentater : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private int targetRank;
    [Inject] ResultSceneManager _sceneManager;

    void Start()
    {
        _sceneManager.OnRecordGot += SetTimeText;
    }

    void SetTimeText(List<long> timeList, long time)
    {
        if(targetRank != 0 && targetRank > timeList.Count)
        {
            timeText.text = $"99:99:999";
            return;
        }
        TimeSpan timeSpan = new TimeSpan(targetRank == 0 ? time : timeList[targetRank - 1]);
        timeText.text = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}:{timeSpan.Milliseconds:000}";
    }

    private void Reset()
    {
        timeText = GetComponent<TMP_Text>();
    }
}