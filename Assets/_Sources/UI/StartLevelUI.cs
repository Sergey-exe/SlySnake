using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Sources.UI
{
    public class StartLevelUI : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private GameObject _startMenu;

        public event Action OnStart; 

        private void OnEnable()
        {
            _startButton.onClick.AddListener(StartGame);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(StartGame);
        }

        public void OpenMenu()
        {
            _startMenu.SetActive(true);
        }

        public void CloseMenu()
        {
            _startMenu.SetActive(false);
        }
    
        public void StartGame()
        {
            OnStart?.Invoke();
            CloseMenu();
        }
    }
}
