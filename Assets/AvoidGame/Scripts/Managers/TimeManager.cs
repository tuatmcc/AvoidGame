using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using Zenject;

/// <summary>
/// ゲームの時間を管理する
/// </summary>
public class TimeManager : MonoBehaviour
{
    public event Action<float> OnTimeChanged;

    public float MainTimer
    {
        get => _mainTimer;
        set
        {
            _mainTimer = value;
            OnTimeChanged?.Invoke(value);
        }
    }

    private float _mainTimer;

    [Inject] GameStateManager _gameStateManager;
    [Inject] ITimeRecordable _timeRecordable;

    public void Start()
    {
        _gameStateManager.OnGameStateChanged += ChangeCount;
    }

    private void ChangeCount(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Playing:
                StartCount();
                break;
            case GameState.Result:
                StopCount(); 
                break;
            default:
                break;
        }
    }

    public void StartCount()
    {
        MainTimer = 0;
        StartCoroutine(CountCoroutine());
    }

    public void StopCount()
    {
        StopCoroutine(CountCoroutine());
        _timeRecordable.RecordTime(MainTimer);
    }
    
    private IEnumerator CountCoroutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            MainTimer++;
        }
    }
}