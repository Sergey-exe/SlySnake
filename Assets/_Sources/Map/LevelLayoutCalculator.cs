using System;
using System.Collections.Generic;
using UnityEngine;
using _Sources.Map;

public class LevelLayoutCalculator
{
    private readonly Vector2 _tileSize;
    private readonly List<Rect> _placedRects = new();

    public LevelLayoutCalculator(Vector2 tileSize)
    {
        _tileSize = tileSize;
    }

    public void Reset()
    {
        _placedRects.Clear();
    }

    public void AddRectForLevel(Maze maze, Vector3 position)
    {
        Vector2 size = GetLevelSizeInUnits(maze);
        Rect rect = new Rect(position.x - size.x / 2f, position.y - size.y / 2f, size.x, size.y);
        _placedRects.Add(rect);
    }

    public Vector2 GetLevelSizeInUnits(Maze maze)
    {
        return new Vector2(maze.Map.GridSize.x * _tileSize.x, maze.Map.GridSize.y * _tileSize.y);
    }

    public Vector3 FindClosestFitPosition(Vector2 size)
    {
        float maxRadius = 50f;
        float step = _tileSize.x;
        int minPoints = 8;
        float radius = 0f;

        while (radius <= maxRadius)
        {
            int points = Mathf.CeilToInt(2 * Mathf.PI * radius / step);
            points = Math.Max(points, minPoints);

            for (int i = 0; i < points; i++)
            {
                float angle = i * 2 * Mathf.PI / points;
                Vector3 candidate = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0);
                Rect rect = new Rect(candidate.x - size.x / 2f, candidate.y - size.y / 2f, size.x, size.y);

                if (IsPositionFree(rect)) 
                    return candidate;
            }
            radius += step;
        }
        return Vector3.zero;
    }

    private bool IsPositionFree(Rect rect)
    {
        foreach (var r in _placedRects)
        {
            if (r.Overlaps(rect)) return false;
        }
        return true;
    }
}