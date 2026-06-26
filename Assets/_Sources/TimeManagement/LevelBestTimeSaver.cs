namespace _Sources.TimeManagement
{
    public class LevelBestTimeSaver
    {
        public void SaveTime(float time, int index)
        {
            LevelTimeDataBroker.TrySaveNewRecord(index, time);
        }
    }
}