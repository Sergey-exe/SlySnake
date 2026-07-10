using System;
using UnityEngine;

public class LevelSequence : MonoBehaviour
{
    [SerializeField] private AllLevels _levels;

    public int CurrentLevelIndex { get; private set; } = 0;

    public event Action<int, LevelOpeningType> IsGotToAd;
    public event Action<int> OnNextLevelSteps;

    public void SetCurrentLevelIndex(int index)
    {
        if (index < 0 || index >= _levels.CountLevels)
            throw new ArgumentOutOfRangeException(nameof(index));

        CurrentLevelIndex = index;
    }

    public Level GetCurrentLevel()
    {
        return _levels.GetLevel(CurrentLevelIndex);
    }

    public void AdvanceToNextLevel()
    {
        int step = 1;
        int nextIndex = CurrentLevelIndex + step;

        if (_levels.GetLevelOpeningType(nextIndex) == LevelOpeningType.ClosedOrAD)
        {
            step++;
            IsGotToAd?.Invoke(nextIndex, LevelOpeningType.ClosedOrAD);
        }

        CurrentLevelIndex = (CurrentLevelIndex + step) % _levels.CountLevels;
        OnNextLevelSteps?.Invoke(step);
    }
}