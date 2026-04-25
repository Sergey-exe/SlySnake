using System;
using System.Collections.Generic;
using _Sources.Map;
using _Sources.UI.Menu.FSM;
using UnityEngine;

namespace _Sources.UI.Menu
{
    public class LevelMenu : MonoBehaviour
    {
        [SerializeField] private List<LevelMenuItem> _items;
        [SerializeField] private MapSpawner _mapSpawner;

        public event Action OnStart;

        public int CurrentLevelIndex { get; private set; }

        private void OnEnable()
        {
            foreach (var item in _items)
                item.OnPlay += Play;
            
            foreach (var item in _items)
                item.OnAds += PlayAfterAD;
        
            _mapSpawner.OnNextLevel += ChangeState;
            _mapSpawner.IsGotToAd += ChangeStateToIndex;
        }

        private void OnDisable()
        {
            foreach (var item in _items)
                item.OnPlay -= Play;
            
            foreach (var item in _items)
                item.OnAds -= PlayAfterAD;
        
            _mapSpawner.OnNextLevel -= ChangeState;
            _mapSpawner.IsGotToAd -= ChangeStateToIndex;
        }

        public void Init()
        {
            foreach (var item in _items)
            {
                if (item.TryGetComponent(out LevelMenuFsmExample _example))
                    _example.Init();
            }
        
            UpdateOpeningTypes();
        }
        
        public void Play(int levelIndex)
        {
            _mapSpawner.SetCurrentLevelIndex(levelIndex);
            OnStart?.Invoke();
        }

        public void PlayAfterAD(int levelIndex)
        {
            Play(levelIndex);
            
            UpdateOpeningTypes();
        }

        public void ChangeState(int step)
        {
            for (int i = _items.Count - 1; i > 0; i--)
            {
                if (_items[i].LevelIndex == _mapSpawner.CurrentLevelIndex)
                {
                    _items[i].ChangeOpeningType(LevelOpeningType.Open);
                    CurrentLevelIndex = i;

                    _items[i - step].ChangeOpeningType(LevelOpeningType.Restart);
                
                    break;
                }
            }

            UpdateOpeningTypes();
        }

        public void ChangeStateToIndex(int index, LevelOpeningType openingType)
        {
            _items[index].ChangeOpeningType(openingType);
        }

        private void UpdateOpeningTypes()
        {
            foreach (var item in _items)
            {
                if (item.TryGetComponent(out LevelMenuFsmExample _example))
                {
                    _example.ChangeState(item.OpeningType);
                }
                else
                {
                    Debug.LogWarning($"Объект UI не имеект компонент {nameof(LevelMenuFsmExample)}");
                }
            }
        }
    }
}