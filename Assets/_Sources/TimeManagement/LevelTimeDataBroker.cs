using System.Collections.Generic;
using _Sources.Map;
using UnityEngine;
using YG;

namespace _Sources.TimeManagement
{
    public static class LevelTimeDataBroker
    {
        private static Dictionary<int, float> _cachedTimes;

        public static Dictionary<int, float> GetAllBestTimes()
        {
            LoadCache();
            return new Dictionary<int, float>(_cachedTimes);
        }

        public static float GetBestTime(int levelIndex, float defaultTime = float.MaxValue)
        {
            LoadCache();
            
            if (_cachedTimes.TryGetValue(levelIndex, out float time))
                return time;
            
            return defaultTime;
        }

        public static bool TrySaveNewRecord(int levelIndex, float newTime)
        {
            LoadCache();

            if (_cachedTimes.TryGetValue(levelIndex, out float oldTime))
            {
                if (newTime >= oldTime) 
                    return false;

                _cachedTimes[levelIndex] = newTime;
                
                int listIndex = YG2.saves.BestLevelsTimeKeys.IndexOf(levelIndex);
                YG2.saves.BestLevelsTimeValues[listIndex] = newTime;
            }
            else
            {
                _cachedTimes.Add(levelIndex, newTime);
                
                YG2.saves.BestLevelsTimeKeys.Add(levelIndex);
                YG2.saves.BestLevelsTimeValues.Add(newTime);
            }

            YG2.SaveProgress();
            return true;
        }

        public static void ClearCache()
        {
            _cachedTimes = null;
        }
        
        private static void LoadCache()
        {
            if (_cachedTimes != null) 
                return;

            _cachedTimes = new Dictionary<int, float>();
            
            var keys = YG2.saves.BestLevelsTimeKeys;
            var values = YG2.saves.BestLevelsTimeValues;
            int count = Mathf.Min(keys.Count, values.Count);

            for (int i = 0; i < count; i++)
                _cachedTimes.Add(keys[i], values[i]);
        }
    }
}