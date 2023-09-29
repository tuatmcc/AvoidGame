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