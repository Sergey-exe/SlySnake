using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateUiInputReader : MonoBehaviour
{
    [SerializeField] private Button[] _restartButtons;
    [SerializeField] private Button[] _nextLevelButtons;
    [SerializeField] private MapSpawner _mapSpawner;
    [SerializeField] private PlayersSpawner _playersSpawner;

    public event Action IsRestart;
    public event Action IsNextLevel;
    
    private void OnEnable()
    {
        foreach (Button button in _restartButtons)
            button.onClick.AddListener(Restart);
        
        foreach (Button button in _nextLevelButtons)
            button.onClick.AddListener(Next);
    }

    private void OnDisable()
    {
        foreach (Button button in _restartButtons)
            button.onClick.RemoveListener(Restart);
        
        foreach (Button button in _nextLevelButtons)
            button.onClick.RemoveListener(Next);
    }

    private void Restart()
    {
        // _mapSpawner.Revert();
        // _playersSpawner.Revert();
        //
        // _mapSpawner.SpawnMap();
        //
        // _gameWinerer.CloseWine();
        IsRestart?.Invoke();
    }

    private void Next()
    {
        IsNextLevel?.Invoke();
    }
}
