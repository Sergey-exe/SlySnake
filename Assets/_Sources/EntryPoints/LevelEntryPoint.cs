using _Sources.Input;
using _Sources.Map;
using _Sources.TimeManagement;
using _Sources.Player;
using UnityEngine;

public class LevelEntryPoint : MonoBehaviour
{
    [SerializeField] private PlayersMover _playersMover;
    [SerializeField] private PlayersSpawner _playersSpawner;
    [SerializeField] private PlayersWayBuilder playersWayBuilder;
    [SerializeField] private MapSpawner _mapSpawner;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CameraFocuser _cameraFocuser;
    [SerializeField] private LevelTimeCounter _levelTimeCounter;
    
    public void Init()
    {
        MapData mapData = new MapData();
        PlayersTransformData playersTransformData = new PlayersTransformData();
        
        _mapSpawner.Init(mapData);
        _playersSpawner.Init(playersTransformData);
        _playersMover.Init(playersTransformData);
        _inputReader.Init();
        _levelTimeCounter.Init();
        _levelTimeCounter.Activate();
        _mapSpawner.Spawned += _cameraFocuser.FocusCameraOnItems;
    }
}