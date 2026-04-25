using _Sources.GameControllers.FSM;
using _Sources.Input;
using _Sources.Map;
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
        [SerializeField] private LevelTimeCounter _levelTimeCounter;
        
        [SerializeField] private GameStateFsmExample _fsmExample;
    
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

        private void Start()
        {
            _fsmExample.Init();
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
            _levelTimeCounter.Revert();
            
            Run();
        }

        private void Next()
        {
            YG2.InterstitialAdvShow();
            
            _levelStateChanger.Next();
            _levelTimeCounter.Revert();
            
            Run();
        }

        private void BackToMenu()
        {
            YG2.InterstitialAdvShow();
            
            _levelStateChanger.Remove();
            _gameStateHandler.Revert();
            _levelTimeCounter.Revert();
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
