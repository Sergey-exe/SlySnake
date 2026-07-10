using System;
using System.Collections.Generic;
using _Sources.Map;
using _Sources.UI.Menu.FSM;
using YG; // Используем чисто для чтения статических данных
using UnityEngine;

namespace _Sources.UI.Menu
{
    public class LevelMenu : MonoBehaviour
    {
        [SerializeField] private List<LevelMenuItem> _items;
        [SerializeField] private LevelSequence _sequence;

        public event Action OnStart;

        public int CurrentLevelIndex { get; private set; }

        private void OnEnable()
        {
            foreach (var item in _items) 
                item.OnPlay += Play;
            foreach (var item in _items) 
                item.OnAds += PlayAfterAD;
            
            _sequence.OnNextLevelSteps += ChangeState;
            _sequence.IsGotToAd += ChangeStateToIndex;
        }

        private void OnDisable()
        {
            foreach (var item in _items) 
                item.OnPlay -= Play;
            foreach (var item in _items) 
                item.OnAds -= PlayAfterAD;

            _sequence.OnNextLevelSteps -= ChangeState;
            _sequence.IsGotToAd -= ChangeStateToIndex;
        }
        
        public void Init()
        {
            foreach (var item in _items)
            {
                if (item.TryGetComponent(out LevelMenuFsmExample example))
                {
                    example.Init(); 
                }
            }
            
            LoadSaves();
            UpdateOpeningTypes();
        }

        public void LoadSaves()
        {
            foreach (var index in YG2.saves.RestartLevelsIndexes)
                _items[index].ChangeOpeningType(LevelOpeningType.Restart);
            
            foreach (var index in YG2.saves.ClosedOrADLevelsIndexes)
                _items[index].ChangeOpeningType(LevelOpeningType.ClosedOrAD);
            
            _items[YG2.saves.CurrentLevelIndex].ChangeOpeningType(LevelOpeningType.Open);
            
            CurrentLevelIndex = YG2.saves.CurrentLevelIndex;
        }

        public void Save(int index, LevelOpeningType openingType)
        {
            switch (openingType)
            {
                case LevelOpeningType.Restart:
                    YG2.saves.RestartLevelsIndexes.Add(index);
                    break;
                
                case LevelOpeningType.ClosedOrAD:
                    YG2.saves.ClosedOrADLevelsIndexes.Add(index);
                    break;
                
                case LevelOpeningType.Open:
                    YG2.saves.CurrentLevelIndex = index;
                    break;
            }
            
            YG2.SaveProgress();
        }
        
        public void Play(int levelIndex)
        {
            _sequence.SetCurrentLevelIndex(levelIndex);
            OnStart?.Invoke();
        }

        public void PlayAfterAD(int levelIndex)
        {
            Play(levelIndex);
            UpdateOpeningTypes();
        }

        public void ChangeState(int step)
        {
            CurrentLevelIndex = _sequence.CurrentLevelIndex;
            
            for (int i = _items.Count - 1; i > 0; i--)
            {
                if (_items[i].LevelIndex == CurrentLevelIndex)
                {
                    _items[i].ChangeOpeningType(LevelOpeningType.Open);
                    CurrentLevelIndex = i;
                    Save(i, LevelOpeningType.Open);

                    int nextIndex = i - step;
                    _items[nextIndex].ChangeOpeningType(LevelOpeningType.Restart);
                    Save(nextIndex, LevelOpeningType.Restart);
                    _items[nextIndex].SetBestTime();
                    
                    break;
                }
            }

            UpdateOpeningTypes();
        }

        public void ChangeStateToIndex(int index, LevelOpeningType openingType)
        {
            _items[index].ChangeOpeningType(openingType);
            Save(index, openingType);
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
                    Debug.LogWarning($"Объект UI не имеет компонент {nameof(LevelMenuFsmExample)}");
                }
            }
        }
    }
}
