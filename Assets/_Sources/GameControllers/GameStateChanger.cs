using _Sources.Model;
using UnityEngine;
using UnityEngine.Serialization;

public class GameStateChanger : MonoBehaviour
{
    [SerializeField] private GameStateHandler _gameStateHandler;
    [SerializeField] private EndGameTextSaver _endGameTextSaver;
    [SerializeField] private GameWineUI _gameWineUI;
    [SerializeField] private GameOverUI _gameOwerUI;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameStateUiInputReader _gameUiInputReader;
    [SerializeField] private LevelStateChanger _levelStateChanger;
    [SerializeField] private StartLevelUI _startLevelUI;
    [SerializeField] private LevelTimeViewer _levelTimeViewer;
    [SerializeField] private LevelTimeCounter _levelTimeCounter;
    
    private bool _isPaused = false;
    
    private void OnEnable()
    {
        _gameStateHandler.IsVin += ChangeState;
        _gameUiInputReader.IsRestart += Revert;
        _gameUiInputReader.IsNextLevel += Next;
        _startLevelUI.OnStart += Launch;
    }

    private void OnDisable()
    {
        _gameStateHandler.IsVin -= ChangeState;
        _gameUiInputReader.IsRestart -= Revert;
        _gameUiInputReader.IsNextLevel -= Next;
        _startLevelUI.OnStart -= Launch;
    }

    public void Launch()
    {
        _levelStateChanger.Launch();
        _inputReader.Activate();
        _levelTimeViewer.ShowTimers();
        _levelTimeCounter.StartCounting();
    }

    private void Revert()
    {
        _gameStateHandler.Revert();
        _levelStateChanger.Restart();
        _gameWineUI.CloseWine();
        _gameOwerUI.CloseLose();
        _levelTimeCounter.Revert();
        ChangePause();
    }

    private void Next()
    {
        _levelStateChanger.Next();
        _gameWineUI.CloseWine();
        _gameOwerUI.CloseLose();
        _levelTimeCounter.Revert();
        ChangePause();
    }

    private void ChangeState(bool isVin)
    {
        ChangePause();
        _levelTimeViewer.HideTimers();
        
        if (isVin)
        {
           Debug.Log("Победа!"); 
           _gameWineUI.ShowWine();
        }
        else
        {
            Debug.Log($"Поражение! Игрок {_endGameTextSaver.GetText()}");
            _gameOwerUI.ShowLose();
        }
    }

    private void ChangePause()
    {
        if (_isPaused)
        {
            _isPaused = false;
            _inputReader.Activate();
            _levelTimeCounter.StartCounting();
        }
        else
        {
            _isPaused = true;
            _inputReader.Deactivate();
            _levelTimeCounter.StopCounting();
        }
    }
}
