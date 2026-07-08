namespace _Sources.TimeManagement
{
    public class LevelBestTimeSaver
    {
        public void SaveTime(float time, int index)
        {
            LevelTimeDataBroker.TrySaveNewRecord(index, time);
        }
    }
    
    public class LevelBestScoreSaver
    {
        public void SaveTime(float score, int index)
        {
            //LevelTimeDataBroker.TrySaveNewRecord(index, score);
        }
    }
}