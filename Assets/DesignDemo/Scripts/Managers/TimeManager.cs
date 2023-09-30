using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// ゲームの時間を管理する
/// </summary>
public class TimeManager : MonoBehaviour
{
    private bool counting = false;

    public event Action<int> OnTimeChanged;

    public int MainTimer
    {
        get => _mainTimer;
        set
        {
            _mainTimer = value;
            OnTimeChanged?.Invoke(value);
        }
    }

    private int _mainTimer;

    [Inject] private GameStateManager _gameStateManager;
    private ITimeRecordable _timeRecordable;

    public void Start()
    {
        _gameStateManager.OnGameStateChanged += ChangeCount;
        _timeRecordable = GameObject.Find("Static").GetComponent<ITimeRecordable>();
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
        if(counting) return; 
        MainTimer = 0;
        StartCoroutine(CountCoroutine());
        counting = true;
    }

    public void StopCount()
    {
        StopCoroutine(CountCoroutine());
        _timeRecordable.RecordTime(MainTimer);
        counting = false;
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