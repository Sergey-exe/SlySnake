using System;
using System.Collections.Generic;
using UnityEngine;
using _Sources.Map;
using _Sources.Player;

public class MapHandler
{
    private LevelSequence _sequence;
    
    private EnvironmentPresenter _environmentPresenter;
    private PlayersWayBuilder _wayBuilder;
    private MapItemChanger _mapItemChanger;
    private MapFactory _mapFactory;

    private MapGenerator _generator;

    public int CurrentLevelIndex => _sequence.CurrentLevelIndex;

    public event Action<List<Transform>> LevelLoaded;

    public void Init(
        MapGenerator generator, 
        LevelSequence sequence, 
        EnvironmentPresenter environmentPresenter, 
        PlayersWayBuilder wayBuilder, 
        MapItemChanger mapItemChanger, 
        MapFactory mapFactory)
    {
        _generator = generator;
        _sequence = sequence;
        _environmentPresenter = environmentPresenter;
        _wayBuilder = wayBuilder;
        _mapItemChanger = mapItemChanger;
        _mapFactory = mapFactory;
    }


    [ContextMenu(nameof(LoadCurrentLevel))]
    public void LoadCurrentLevel()
    {
        if (_generator == null)
        {
            Debug.LogWarning($"[{nameof(MapHandler)}] {nameof(MapGenerator)} не инициализирован. Запустите игру через Entry Point.");
            return;
        }

        Level currentLevel = _sequence.GetCurrentLevel();
        List<Transform> mapElements = _generator.Generate(currentLevel);
        
        LevelLoaded?.Invoke(mapElements);
    }

    public void RestartLevel()
    {
        CleanUp(setDefault: false);
        LoadCurrentLevel();
    }

    public void NextLevel()
    {
        _sequence.AdvanceToNextLevel();
        CleanUp(setDefault: false);
        LoadCurrentLevel();
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
        _mapItemChanger.Revert();
        _wayBuilder.Revert();

        if (setDefault)
        {
            _environmentPresenter.ResetToDefault();
        }
    }
}
