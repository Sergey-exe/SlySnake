using System;

namespace _Sources.TimeManagement
{
    public class LevelBestScoreCalculator
    {
        private int _score = 0;
        
        public void Calculate(int countPlayers, int levelSize, int levelNumber, float time)
        {
            float rawPoints = (levelSize * countPlayers * levelNumber * 2f) / (time * 10f);
            
            _score = (int)Math.Max(5, Math.Round(rawPoints / 50f) * 5);
        }

    }
}