using UnityEngine;
using UnityEngine.Serialization;

public class Root : MonoBehaviour
{
    [SerializeField] private PlayersMover _playersMover;
    [SerializeField] private PlayersSpawner _playersSpawner;
    [FormerlySerializedAs("_playerWayBuilder")] [SerializeField] private PlayersWayBuilder playersWayBuilder;
    [SerializeField] private MapSpawner _mapSpawner;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CameraFocuser _cameraFocuser;
    
    private void Start()
    {
        MapData mapData = new MapData();
        PlayersTransformData playersTransformData = new PlayersTransformData();
        
        _mapSpawner.Init(mapData);
        _playersSpawner.Init(playersTransformData);
        _playersMover.Init(playersTransformData);
        
        _mapSpawner.Spawned += _cameraFocuser.FocusCameraOnItems;
        _mapSpawner.SpawnMap();
        _inputReader.Activate();
    }
}
