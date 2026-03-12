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
    }

    private void Revert()
    {
        _gameStateHandler.Revert();
        _levelStateChanger.Restart();
        _gameWineUI.CloseWine();
        _gameOwerUI.CloseLose();
        ChangePause();
    }

    private void Next()
    {
        _levelStateChanger.Next();
        _gameWineUI.CloseWine();
        _gameOwerUI.CloseLose();
        ChangePause();
    }

    private void ChangeState(bool isVin)
    {
        ChangePause();
        
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
        }
        else
        {
            _isPaused = true;
            _inputReader.Deactivate();
        }
    }
}
