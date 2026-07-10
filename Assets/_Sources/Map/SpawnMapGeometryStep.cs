using _Sources.Map;
using UnityEngine;

public class SpawnMapGeometryStep : IGenerationStep
{
    private readonly MapFactory _factory;
    private readonly LevelLayoutCalculator _calculator;

    public SpawnMapGeometryStep(MapFactory factory, LevelLayoutCalculator calculator)
    {
        _factory = factory;
        _calculator = calculator;
    }

    public void Execute(GenerationContext context)
    {
        var mazes = context.Level.GetMazes();
        Vector2 tileSize = _factory.GetTileSize();
        context.TileSize = tileSize;

        for (int i = 0; i < mazes.Count; i++)
        {
            Maze maze = mazes[i];
            Vector3 pos = Vector3.zero;

            if (i > 0)
            {
                Vector2 size = _calculator.GetLevelSizeInUnits(maze);
                pos = _calculator.FindClosestFitPosition(size);
            }

            Map map = _factory.SpawnLevelAtPosition(i, maze, pos, tileSize, out var points);
            context.Maps.Add(map);
            context.SpawnPoints.AddRange(points);
            _calculator.AddRectForLevel(maze, pos);
            context.AllTransforms.AddRange(map.GetItemTransforms());
        }
    }
}