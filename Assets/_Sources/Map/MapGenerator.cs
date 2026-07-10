using System.Collections.Generic;
using _Sources.Map;
using _Sources.Player;
using UnityEngine;

public class MapGenerator
{
    private readonly List<IGenerationStep> _steps = new();

    public MapGenerator(
        EnvironmentPresenter envPresenter,
        MapFactory mapFactory,
        LevelLayoutCalculator layoutCalculator,
        MapsProgressCollection progressCollection,
        PlayersSpawner playersSpawner,
        PlayersWayBuilder wayBuilder,
        MapItemChanger mapItemChanger)
    {
        _steps.Add(new SetupEnvironmentStep(envPresenter));
        _steps.Add(new SpawnMapGeometryStep(mapFactory, layoutCalculator));
        _steps.Add(new SetupProgressStep(progressCollection));
        _steps.Add(new SpawnPlayersStep(playersSpawner, wayBuilder, mapItemChanger, envPresenter));
    }

    public List<Transform> Generate(Level level)
    {
        var context = new GenerationContext(level);

        foreach (var step in _steps)
        {
            step.Execute(context);
        }

        return context.AllTransforms;
    }
}