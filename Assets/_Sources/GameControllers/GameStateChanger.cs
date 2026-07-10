using _Sources.GameControllers.FSM;
using _Sources.Input;
using _Sources.Map;
using _Sources.Player;
using _Sources.TimeManagement;
using _Sources.TimeManagement;
using _Sources.UI;
using _Sources.UI.Menu;
using YG;
using UnityEngine;

namespace _Sources.GameControllers
{
    public class GameStateChanger : MonoBehaviour
    {
        [SerializeField] private GameStateHandler _gameStateHandler;
        [SerializeField] private EndGameTextSaver _endGameTextSaver;
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private GameStateUiInputReader _gameUiInputReader;
        [SerializeField] private LevelStateChanger _levelStateChanger;
        [SerializeField] private LevelMenu _levelMenu;
        [SerializeField] private PlayersMover _playersMover;
        
        [SerializeField] private GameStateFsmExample _fsmExample;
        
        private ITimeReset _timeReset;
        private ITimeSaver _timeSaver;
    
        private bool _isPaused = false;
    
        private void OnEnable()
        {
            _gameStateHandler.IsVin += ChangeState;
            _gameUiInputReader.IsRestart += Revert;
            _gameUiInputReader.IsNextLevel += Next;
            _gameUiInputReader.IsPause += Pause;
            _gameUiInputReader.IsRun += Run;
            _gameUiInputReader.IsMenu += BackToMenu;
            _levelMenu.OnStart += Launch;
        }

        private void OnDisable()
        {
            _gameStateHandler.IsVin -= ChangeState;
            _gameUiInputReader.IsRestart -= Revert;
            _gameUiInputReader.IsNextLevel -= Next;
            _gameUiInputReader.IsPause -= Pause;
            _gameUiInputReader.IsRun -= Run;
            _gameUiInputReader.IsMenu -= BackToMenu;
            _levelMenu.OnStart -= Launch;
        }

        public void Init(ITimeReset levelTimeReset, ITimeSaver timeSaver, ITimeCounter timeCounter)
        {
            _fsmExample.Init(timeCounter);
            
            _timeReset = levelTimeReset;
            _timeSaver = timeSaver;
        }

        public void Launch()
        {
            _levelStateChanger.Launch();

            Run();
        }

        public void Pause()
        {
            YG2.InterstitialAdvShow();
            
            _fsmExample.ChangeState(GameStates.Pause);
        }

        public void Run()
        {
            _fsmExample.ChangeState(GameStates.Game);
        }

        private void Revert()
        {
            YG2.InterstitialAdvShow();
            
            _gameStateHandler.Revert();
            _levelStateChanger.Restart();
            _timeReset.ResetTime();
            _playersMover.StopActiveCoroutines();
            
            Run();
        }

        private void Next()
        {
            YG2.InterstitialAdvShow();
            
            _timeSaver.SaveTime(_levelStateChanger.CurrentLevelIndex);

            _levelStateChanger.Next();
            _timeReset.ResetTime();
            
            Run();
        }

        private void BackToMenu()
        {
            YG2.InterstitialAdvShow();
            
            _levelStateChanger.Remove();
            _gameStateHandler.Revert();
            _timeReset.ResetTime();
            _playersMover.StopActiveCoroutines();
            _fsmExample.ChangeState(GameStates.Menu);
        }

        private void ChangeState(bool isVin)
        {
            if (isVin)
                _fsmExample.ChangeState(GameStates.Win);
            else
                _fsmExample.ChangeState(GameStates.Lose);
        }
    }
}
