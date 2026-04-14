using _Sources.Player;
using UnityEngine;

namespace _Sources.Map
{
    public class LevelStateChanger : MonoBehaviour
    {
        [SerializeField] private MapSpawner _mapSpawner;
        [SerializeField] private PlayersSpawner _playersSpawner;

        public void Launch()
        {
            _mapSpawner.SpawnMap();
        }
    
        public void Restart()
        {
            _playersSpawner.Revert();
            _mapSpawner.RestartLevel();
        }

        public void Remove()
        {
            _playersSpawner.Revert();
            _mapSpawner.Revert();
        }

        public void Next()
        {
            _playersSpawner.Revert();
            _mapSpawner.NextLevel();
        }
    }
}
