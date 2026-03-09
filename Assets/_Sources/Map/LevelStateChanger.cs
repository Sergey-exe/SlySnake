using UnityEngine;

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

    public void Next()
    {
        _playersSpawner.Revert();
        _mapSpawner.NextLevel();
    }
}
