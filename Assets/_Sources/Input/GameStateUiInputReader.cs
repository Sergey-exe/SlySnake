using System;
using _Sources.Map;
using _Sources.Player;
using UnityEngine;
using UnityEngine.UI;

namespace _Sources.Input
{
    public class GameStateUiInputReader : MonoBehaviour
    {
        [SerializeField] private Button[] _restartButtons;
        [SerializeField] private Button[] _nextLevelButtons;
        [SerializeField] private Button[] _pauseButtons;
        [SerializeField] private Button[] _runButtons;
        [SerializeField] private Button[] _menuButtons;
        
        [SerializeField] private MapSpawner _mapSpawner;
        [SerializeField] private PlayersSpawner _playersSpawner;

        public event Action IsRestart;
        public event Action IsNextLevel;
        public event Action IsPause;
        public event Action IsRun;
        public event Action IsMenu;
    
        private void OnEnable()
        {
            foreach (Button button in _restartButtons)
                button.onClick.AddListener(Restart);
        
            foreach (Button button in _nextLevelButtons)
                button.onClick.AddListener(Next);
            
            foreach (Button button in _pauseButtons)
                button.onClick.AddListener(Pause);
            
            foreach (Button button in _runButtons)
                button.onClick.AddListener(Run);

            foreach (Button button in _menuButtons)
                button.onClick.AddListener(GoToMenu);
        }

        private void OnDisable()
        {
            foreach (Button button in _restartButtons)
                button.onClick.RemoveListener(Restart);
        
            foreach (Button button in _nextLevelButtons)
                button.onClick.RemoveListener(Next);
            
            foreach (Button button in _pauseButtons)
                button.onClick.RemoveListener(Pause);
            
            foreach (Button button in _runButtons)
                button.onClick.RemoveListener(Run);
            
            foreach (Button button in _menuButtons)
                button.onClick.RemoveListener(GoToMenu);
        }

        private void Restart()
        {
            IsRestart?.Invoke();
        }

        private void Next()
        {
            IsNextLevel?.Invoke();
        }

        private void Pause()
        {
            IsPause?.Invoke();
        }

        private void Run()
        {
            IsRun?.Invoke();
        }

        private void GoToMenu()
        {
            IsMenu?.Invoke();
        }
    }
}
