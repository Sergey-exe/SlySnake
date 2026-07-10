using _Sources.Map;
using _Sources.Player;
using UnityEngine;

public class SpawnPlayersStep : IGenerationStep
{
    private readonly PlayersSpawner _spawner;
    private readonly PlayersWayBuilder _wayBuilder;
    private readonly MapItemChanger _itemChanger;
    private readonly EnvironmentPresenter _envPresenter;

    public SpawnPlayersStep(PlayersSpawner spawner, PlayersWayBuilder wayBuilder, MapItemChanger itemChanger, EnvironmentPresenter envPresenter)
    {
        _spawner = spawner;
        _wayBuilder = wayBuilder;
        _itemChanger = itemChanger;
        _envPresenter = envPresenter;
    }

    public void Execute(GenerationContext context)
    {
        AudioClip impactSound = _envPresenter.GetImpactSound(context.Level.EnvironmentSetType);

        foreach (var point in context.SpawnPoints)
        {
            _spawner.Spawn(point.MapIndex, point.DefaultSprite, point.Transform, impactSound);
            point.Transform.GetComponent<SpriteRenderer>().sprite = point.TailSprite;
        }

        _wayBuilder.SetMaps(context.Maps);
        _itemChanger.SetMaps(context.Maps);
    }
}