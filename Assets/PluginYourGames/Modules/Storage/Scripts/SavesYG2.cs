
using System.Collections.Generic;

namespace YG
{
    [System.Serializable]
    public partial class SavesYG
    {
        public int idSave;

        public List<int> BestLevelsTimeKeys = new List<int>();
        public List<float> BestLevelsTimeValues = new List<float>();
        public List<int> RestartLevelsIndexes =  new List<int>();
        public List<int> ClosedOrADLevelsIndexes =  new List<int>();
        public int CurrentLevelIndex;

        public void CleanSaves()
        {
            BestLevelsTimeKeys.Clear();
            BestLevelsTimeValues.Clear();
            RestartLevelsIndexes.Clear();
            ClosedOrADLevelsIndexes.Clear();
            CurrentLevelIndex = 0;
        }
    }
}
