using _Sources.Player;
using UnityEngine;

namespace _Sources.Map
{
    public class LevelStateChanger : MonoBehaviour
    {
        [SerializeField] private PlayersSpawner _playersSpawner;
        
        private MapHandler _mapHandler;

        public int CurrentLevelIndex => _mapHandler.CurrentLevelIndex;

        public void Init(MapHandler mapHandler)
        {
            _mapHandler = mapHandler;
        }

        public void Launch()
        {
            _mapHandler.LoadCurrentLevel();
        }
    
        public void Restart()
        {
            _playersSpawner.Revert();
            _mapHandler.RestartLevel();
        }

        public void Remove()
        {
            _playersSpawner.Revert();
            _mapHandler.Revert(true);
        }

        public void Next()
        {
            _playersSpawner.Revert();
            _mapHandler.NextLevel();
        }
    }
}
