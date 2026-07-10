using System;
using Game.LevelSystem;

public class LevelBestScoreCalculator
{
    private const float BaseMultiplier = 2f;
    private const float TimeWeight = 10f;
    private const float ScoreScale = 50f;
    private const int MinScore = 5;
    private const int RoundStep = 5;

    public int Calculate(int levelNumber, int levelSize, int playersCount, float levelTime)
    {
        if (levelTime <= 0.01f) 
            levelTime = 0.01f; 

        float rawPoints = (levelSize * playersCount * levelNumber * BaseMultiplier) / (levelTime * TimeWeight);

        int score = (int)Math.Max(MinScore, Math.Round(rawPoints / ScoreScale) * RoundStep);
        
        return score;
    }
}

namespace Game.LevelSystem
{
    public class LevelBestScoreHandler
    {
        private readonly ILevelNumberProvider _levelNumberProvider;
        private readonly ILevelSizeProvider _levelSizeProvider;
        private readonly IPlayerDataProvider _playerDataProvider;
        private readonly ITimerDataProvider _timerDataProvider;
        private readonly LevelBestScoreCalculator _calculator;
        private readonly IScoreStorage _scoreStorage;

        public LevelBestScoreHandler(
            ILevelNumberProvider levelNumberProvider,
            ILevelSizeProvider levelSizeProvider,
            IPlayerDataProvider playerDataProvider,
            ITimerDataProvider timerDataProvider,
            LevelBestScoreCalculator calculator,
            IScoreStorage scoreStorage)
        {
            _levelNumberProvider = levelNumberProvider;
            _levelSizeProvider = levelSizeProvider;
            _playerDataProvider = playerDataProvider;
            _timerDataProvider = timerDataProvider;
            _calculator = calculator;
            _scoreStorage = scoreStorage;
        }

        public void ProcessLevelCompletion()
        {
            int levelNumber = _levelNumberProvider.CurrentLevelNumber;
            int levelSize = _levelSizeProvider.CurrentLevelSize;
            int playersCount = _playerDataProvider.ActivePlayersCount;
            float levelTime = _timerDataProvider.ElapsedTime;

            int finalScore = _calculator.Calculate(levelNumber, levelSize, playersCount, levelTime);

            _scoreStorage.SaveIfBest(levelNumber, finalScore);
        }
    }
}


namespace Game.LevelSystem
{
    public interface ILevelNumberProvider
    {
        int CurrentLevelNumber { get; }
    }

    public interface ILevelSizeProvider
    {
        int CurrentLevelSize { get; }
    }

    public interface IPlayerDataProvider
    {
        int ActivePlayersCount { get; }
    }

    public interface ITimerDataProvider
    {
        float ElapsedTime { get; }
    }

    public interface IScoreStorage
    {
        void SaveIfBest(int levelNumber, int calculatedScore);
    }
}

