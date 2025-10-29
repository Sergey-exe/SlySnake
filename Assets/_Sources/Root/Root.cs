using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private PlayersMover _playersMover;
    [SerializeField] private PlayersSpawner _playersSpawner;
    [SerializeField] private MapSpawner _mapSpawner;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CameraFocuser _cameraFocuser;
    
    private void Start()
    {
        _mapSpawner.Init();
        
        _mapSpawner.Spawned += _cameraFocuser.FocusCameraOnItems;
        _mapSpawner.SpawnMap();
        _inputReader.Activate();
    }
}
