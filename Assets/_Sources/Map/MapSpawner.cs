using System;
using System.Collections.Generic;
using UnityEngine;
using _Sources.Map;
using _Sources.Player;

public class MapSpawner : MonoBehaviour
{
    [Header("Data References")]
    [SerializeField] private LevelSequence _sequence;
    [SerializeField] private MapsProgressCollection _mapsProgressCollection;

    [Header("Systems References")]
    [SerializeField] private MapFactory _mapFactory;
    [SerializeField] private EnvironmentPresenter _environmentPresenter;
    [SerializeField] private PlayersSpawner _playersSpawner;
    [SerializeField] private PlayersWayBuilder _wayBuilder;
    [SerializeField] private MapItemChanger _mapItemChanger;

    private MapGenerator _generator;
    private LevelLayoutCalculator _layoutCalculator;

    public int CurrentLevelIndex => _sequence.CurrentLevelIndex;

    public event Action<List<Transform>> Spawned;

    public void Init()
    {
        _layoutCalculator = new LevelLayoutCalculator(_mapFactory.GetTileSize());

        _generator = new MapGenerator(
            _environmentPresenter,
            _mapFactory,
            _layoutCalculator,
            _mapsProgressCollection,
            _playersSpawner,
            _wayBuilder,
            _mapItemChanger
        );
    }

    [ContextMenu(nameof(SpawnMap))]
    public void SpawnMap()
    {
        Level currentLevel = _sequence.GetCurrentLevel();

        List<Transform> mapElements = _generator.Generate(currentLevel);
        
        Spawned?.Invoke(mapElements);
    }

    public void RestartLevel()
    {
        CleanUp(false);
        SpawnMap();
    }

    public void NextLevel()
    {
        _sequence.AdvanceToNextLevel();
        RestartLevel();
    }

    public void Revert(bool setDefault)
    {
        CleanUp(setDefault);
    }

    public void SetCurrentLevelIndex(int index)
    {
        _sequence.SetCurrentLevelIndex(index);
    }

    private void CleanUp(bool setDefault)
    {
        _mapFactory.ClearCollector();
        _layoutCalculator.Reset();

        _mapItemChanger.Revert();
        _wayBuilder.Revert();

        if (setDefault)
            _environmentPresenter.ResetToDefault();
    }
}
