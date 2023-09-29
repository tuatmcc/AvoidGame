using System;
using UnityEngine;
using Zenject;

/// <summary>
/// ゲームの進行管理
/// </summary>
public class GameStateManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChanged;
    
    public GameState GameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            OnGameStateChanged?.Invoke(_gameState);
        }
    }

    private GameState _gameState;
    
    [Inject] private TimeManager _timeManager;
    
    private void Start()
    {
        OnGameStateChanged += ChangeGameState;
        
        GameState = GameState.Playing;
    }

    /// <summary>
    /// 状態が変更されたときの処理
    /// </summary>
    /// <param name="gameState"></param>
    private void ChangeGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Menu:
                break;
            case GameState.Playing:
                _timeManager.StartCount();
                break;
            case GameState.Result:
                _timeManager.StopCount();
                break;
        }
    }
}