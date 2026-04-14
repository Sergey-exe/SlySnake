using System.Collections;
using System.Collections.Generic;
using _Sources.GameControllers.FSM;
using _Sources.Input;
using _Sources.Map;
using _Sources.TimeManagement;
using _Sources.UI;
using UnityEngine;

public class GameStateFsmExample : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LevelTimeCounter _levelTimeCounter;
    [SerializeField] private GameObject _endWindow;
    [SerializeField] private GameObject _winWindow;
    [SerializeField] private GameObject _pauseWindow;
    [SerializeField] private GameObject _gameWindow;
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private MiniMapPainter _miniMapPainter;
    [SerializeField] private GameStateHandler _gameStateHandler;
    [SerializeField] private LevelStateChanger _levelStateChanger;
    
    private Fsm _fsm;

    public void Init()
    {
        _fsm = new GameStatesFsm();
        
        _fsm.AddState(new FsmStateGame(_gameWindow, _inputReader, _levelTimeCounter, _fsm));
        _fsm.AddState(new FsmStateGameOver(_endWindow, _miniMapPainter, _fsm));
        _fsm.AddState(new FsmStateGameWin(_winWindow, _fsm));
        _fsm.AddState(new FsmStatePause(_pauseWindow,  _fsm));
        _fsm.AddState(new FsmStateMenu(_menuUI, _fsm));
        
        _fsm.SetState<FsmStateMenu>();
    }
    
    public void ChangeState(GameStates newState)
    {
        if(_fsm == null)
            return;
        
        switch (newState)
        {
            case GameStates.Game:
                _fsm.SetState<FsmStateGame>();
                break;
            
            case GameStates.Win:
                _fsm.SetState<FsmStateGameWin>();
                break;
            
            case GameStates.Lose:
                _fsm.SetState<FsmStateGameOver>();
                break;
            
            case GameStates.Pause:
                _fsm.SetState<FsmStatePause>();
                break;
            
            case GameStates.Menu:
                _fsm.SetState<FsmStateMenu>();
                break;
        }
    }
}