using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
    [SerializeField] private GameStateHandler _gameStateHandler;
    [SerializeField] private EndGameTextSaver _endGameTextSaver;
    [SerializeField] private GameWinerer _gameWinerer;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameStateUiInputReader _gameUiInputReader;
    [SerializeField] private MapSpawner _mapSpawner;
    [SerializeField] private PlayersSpawner _playersSpawner;
    
    private bool _isPaused = false;
    
    private void OnEnable()
    {
        _gameStateHandler.IsVin += ChangeState;
        _gameUiInputReader.IsRestart +=  Revert;
        _gameUiInputReader.IsNextLevel += Next;
    }

    private void OnDisable()
    {
        _gameStateHandler.IsVin -= ChangeState;
    }

    private void ChangeState(bool isVin)
    {
        ChangePause();
        
        if (isVin)
        {
           Debug.Log("Победа!"); 
           _gameWinerer.ShowWine();
        }
        else
        {
            Debug.Log($"Поражение! Игрок {_endGameTextSaver.GetText()}");
        }
    }

    private void Revert()
    {
        _gameWinerer.CloseWine();
        _playersSpawner.Revert();
        _mapSpawner.RestartLevel();
        ChangePause();
    }

    private void Next()
    {
        _gameWinerer.CloseWine();
        _playersSpawner.Revert();
        _mapSpawner.NextLevel();
        ChangePause();
    }

    private void ChangePause()
    {
        if (_isPaused)
        {
            _isPaused = false;
            _inputReader.Activate();
        }
        else
        {
            _isPaused = true;
            _inputReader.Deactivate();
        }
    }
}
