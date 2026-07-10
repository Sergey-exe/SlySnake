using System.Collections.Generic;
using UnityEngine;
using _Sources.Input;
using _Sources.Map;
using _Sources.TimeManagement;
using _Sources.Player;

public class LevelEntryPoint : MonoBehaviour
{
    [Header("Core Systems")]
    [SerializeField] private LevelSequence _levelSequence;

    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CameraFocuser _cameraFocuser;
    [SerializeField] private LevelTimeCounter _levelTimeCounter;
    [SerializeField] private LevelStateChanger _levelStateChanger;

    [Header("Player Systems")]
    [SerializeField] private PlayersMover _playersMover;
    [SerializeField] private PlayersSpawner _playersSpawner;
    [SerializeField] private PlayersWayBuilder _wayBuilder;

    [Header("Map Dependencies (For Generator)")]
    [SerializeField] private MapFactory _mapFactory;
    [SerializeField] private MapsProgressCollection _mapsProgressCollection;
    [SerializeField] private MapItemChanger _mapItemChanger;
    [SerializeField] private EnvironmentPresenter _environmentPresenter;

    private MapHandler _mapHandler; 

    public void Init()
    {
        PlayersTransformData playersTransformData = new PlayersTransformData();

        LevelLayoutCalculator layoutCalculator = new LevelLayoutCalculator(_mapFactory.GetTileSize());
        
        MapGenerator mapGenerator = new MapGenerator(
            _environmentPresenter,
            _mapFactory,
            layoutCalculator,
            _mapsProgressCollection,
            _playersSpawner,
            _wayBuilder,
            _mapItemChanger
        );

        _mapHandler = new MapHandler();

        _mapHandler.Init(
            mapGenerator,
            _levelSequence,
            _environmentPresenter,
            _wayBuilder,
            _mapItemChanger,
            _mapFactory
        );

        _levelStateChanger.Init(_mapHandler);
        _playersSpawner.Init(playersTransformData);
        _playersMover.Init(playersTransformData);
        _inputReader.Init();
        
        _levelTimeCounter.Init();
        _levelTimeCounter.Activate();

        _mapHandler.LevelLoaded += _cameraFocuser.FocusCameraOnItems;
    }
}
