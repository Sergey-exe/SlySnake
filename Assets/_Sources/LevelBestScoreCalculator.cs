using System;
using UnityEngine;

namespace _Sources.TimeManagement
{
    public class LevelBestScoreCalculator
    {
        public event Action<int> IsScoreChanged;
        
        public void Calculate(float time)
        {
            
            // float rawPoints = (_gameConfig.LevelSize * _gameConfig.PlayersCount * _gameConfig.LevelNumber * 2f) / (time * 10f);
            //
            // int score = (int)Math.Max(5, Math.Round(rawPoints / 50f) * 5);
            // IsScoreChanged?.Invoke(score);
        }
    }
}