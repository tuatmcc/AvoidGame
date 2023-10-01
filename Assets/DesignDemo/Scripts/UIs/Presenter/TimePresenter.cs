using System;
using TMPro;
using UnityEngine;
using Zenject;

/// <summary>
/// 残り時間のUIを更新する
/// </summary>
public class TimePresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text timeLeftText;
    
    [Inject] private TimeManager _timeManager;
    
    private void Awake()
    {
        _timeManager.OnTimeChanged += ChangeTimeLeftText;
    }

    private void ChangeTimeLeftText(TimeSpan time)
    {
        timeLeftText.text = $"{time.Minutes:00}:{time.Seconds:00}:{time.Milliseconds:000}";
    }
    
    private void Reset()
    {
        timeLeftText = GetComponent<TMP_Text>();
    }
}