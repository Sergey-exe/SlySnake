using UnityEngine;

namespace _Sources.TimeManagement
{
    public class TimeRoot : MonoBehaviour
    {
        [SerializeField] private LevelTimeViewer _levelTimeViewer;
        [SerializeField] private LevelTimeCounter _levelTimeCounter;
    
        private LevelBestTimeSaver _levelBestTimeSaver;
        private LevelTimeHandler _levelTimeHandler;

        public LevelTimeHandler LevelTimeHandler => _levelTimeHandler;

        public void Root()
        {
            _levelTimeCounter.Init();
        
            _levelBestTimeSaver = new LevelBestTimeSaver();
            _levelTimeHandler = new LevelTimeHandler();
            
            _levelTimeHandler.Init(_levelTimeCounter, _levelTimeViewer, _levelBestTimeSaver);
        }

        private void OnDestroy()
        {
            _levelTimeHandler?.Dispose();
        }
    }
}