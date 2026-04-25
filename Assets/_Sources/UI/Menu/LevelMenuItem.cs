using System;
using TMPro;
using YG;
using UnityEngine;
using UnityEngine.UI;

namespace _Sources.UI.Menu
{
    public class LevelMenuItem : MonoBehaviour
    {
        [SerializeField] private Image _preview;
        [SerializeField] private TextMeshProUGUI _infoText;
    
        [SerializeField] private Button _play;
        [SerializeField] private Button _restart;
        [SerializeField] private Button _ads;

        [SerializeField] private GameObject _closeUI;
    
        [SerializeField] private LevelOpeningType  _openingType;
    
        public LevelOpeningType OpeningType  => _openingType;
    
        [field: SerializeField] public int LevelIndex { get; private set; }

        public event Action<int> OnPlay;
        public event Action<int> OnAds;

        private void OnEnable()
        {
            _play.onClick.AddListener(Play);
            _restart.onClick.AddListener(Restart);
            _ads.onClick.AddListener(Ads);
        }

        private void OnDisable()
        {
            _play.onClick.RemoveListener(Play);
            _restart.onClick.RemoveListener(Restart);
            _ads.onClick.RemoveListener(Ads);
        }

        public void ChangeOpeningType(LevelOpeningType openingType)
        {
            _openingType = openingType;
        }
    
        public void Play()
        {
            OnPlay?.Invoke(LevelIndex);
        }

        public void Restart()
        {
            OnPlay?.Invoke(LevelIndex);
        }

        public void Ads()
        {
            YG2.RewardedAdvShow("BonusLevel", () =>
            {
                _openingType = LevelOpeningType.Restart;
                OnAds?.Invoke(LevelIndex);
            });
        }
    }
}