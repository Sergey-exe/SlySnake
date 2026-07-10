using System;

namespace _Sources.TimeManagement
{
    public class LevelTimeHandler : IDisposable, ITimeReset, ITimeSaver, ITimeCounter
    {
        private LevelTimeViewer _levelTimeViewer;
        private LevelBestTimeSaver _levelBestTimeSaver;
        private LevelTimeCounter _levelTimeCounter;
        private bool _isInitialized;
        
        public void Init(
            LevelTimeCounter levelTimeCounter, 
            LevelTimeViewer levelTimeViewer, 
            LevelBestTimeSaver levelBestTimeSaver)
        {
            if (_isInitialized) 
                Release();

            _levelTimeCounter = levelTimeCounter ?? throw new ArgumentNullException(nameof(levelTimeCounter));
            _levelTimeViewer = levelTimeViewer ?? throw new ArgumentNullException(nameof(levelTimeViewer));
            _levelBestTimeSaver = levelBestTimeSaver ?? throw new ArgumentNullException(nameof(levelBestTimeSaver));

            _levelTimeCounter.OnTimeChanged += _levelTimeViewer.ShowTime;
            _isInitialized = true;
        }

        public void StartCounting()
        {
            if (!_isInitialized) 
                return;

            _levelTimeCounter.StartCounting();
            _levelTimeViewer.ShowTimers();
        }

        public void StopCounting()
        {
            if (!_isInitialized) 
                return;

            _levelTimeCounter.StopCounting();
            _levelTimeViewer.HideTimers();
        }

        public void SaveTime(int index)
        {
            if (!_isInitialized) return;

            _levelBestTimeSaver.SaveTime(_levelTimeCounter.GameTime, index);
        }

        public void ResetTime()
        {
            _levelTimeCounter.Revert();
        }

        public void Release()
        {
            if (!_isInitialized) return;

            _levelTimeCounter.StopCounting(); 
            
            _levelTimeCounter.OnTimeChanged -= _levelTimeViewer.ShowTime;

            _levelTimeCounter = null;
            _levelTimeViewer = null;
            _levelBestTimeSaver = null;
            
            _isInitialized = false;
        }

        public void Dispose()
        {
            Release();
        }
    }
    
    public interface ITimeReset
    {
        void ResetTime();
    }

    public interface ITimeSaver
    {
        void SaveTime(int index);
    }
    
    public interface ITimeCounter
    {
        void StartCounting();
        void StopCounting();
    }
}