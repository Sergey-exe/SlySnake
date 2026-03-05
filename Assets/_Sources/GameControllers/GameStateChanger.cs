using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
    [SerializeField] private GameStateHandler _gameStateHandler;
    [SerializeField] private EndGameTextSaver _endGameTextSaver;

    private void OnEnable()
    {
        _gameStateHandler.IsVin += ChangeState;
    }

    private void OnDisable()
    {
        _gameStateHandler.IsVin -= ChangeState;
    }

    private void ChangeState(bool isVin)
    {
        if(isVin)
            Debug.Log("Победа!");
        else
            Debug.Log($"Поражение! Игрок {_endGameTextSaver.GetText()}");
    }
}
