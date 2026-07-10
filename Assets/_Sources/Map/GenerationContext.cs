using System.Collections.Generic;
using _Sources.Map;
using UnityEngine;

public class GenerationContext
{
    public Level Level { get; }
    public Vector2 TileSize { get; set; }
    public List<Map> Maps { get; } = new();
    public List<PlayerSpawnPoint> SpawnPoints { get; } = new();
    public List<Transform> AllTransforms { get; } = new();

    public GenerationContext(Level level)
    {
        Level = level;
    }
}

public interface IGenerationStep
{
    void Execute(GenerationContext context);
}