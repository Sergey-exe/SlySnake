using System;
using System.Collections.Generic;
using _Sources.Map;
using Array2DEditor;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _mapItemPrefab;
    [SerializeField] private PlayersSpawner _playersSpawner;
    [SerializeField] private MapData _mapData;
    [SerializeField] private Transform _spawnHub;
    [SerializeField] private List<Level> _currentLevels;
    
    private Vector2 _tileSize;
    private List<List<MapItem>> _mapItemsGoups = new();
    private List<Rect> _placedRects = new(); 
    private Dictionary<int, List<MapItem>> _changingItemsMap = new();
    private bool _isInit;

    public event Action<List<Transform>> Spawned; 

    public void Init()
    {
        _tileSize = _mapItemPrefab.GetComponent<SpriteRenderer>().bounds.size;
        
        _isInit = true;
    }

    public void SetLevels(List<Level> levels)
    {
        _currentLevels = levels ?? throw new ArgumentNullException(nameof(levels)); 
    }

    public GameMapVector2 SearchPlayer(int mapIndex)
    {
        int[,] map = _mapData.GetCurrentMap(mapIndex);
        
        for (int i = 0; i < map.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < map.GetLength(1) - 1; j++)
            {
                if(map[i, j] == (int)MapItemType.Player)
                    return new GameMapVector2(i, j);
            }
        }
        
        throw new Exception("Игрок не найден!");
    }
    
    public void SpawnMap()
    {
        if (_isInit == false)
        {
            Debug.LogError("Класс не инициализирован!");
            return;
        }
        
        if (_currentLevels == null || _currentLevels.Count == 0)
            throw new ArgumentNullException(nameof(_currentLevels));

        int newMapIndex = 0;
        
        _placedRects.Clear();
        _mapItemsGoups.Clear();

        Level firstLevel = _currentLevels[0];
        SpawnLevelAtPosition(newMapIndex, firstLevel, Vector3.zero);
        AddRectForLevel(firstLevel, Vector3.zero);
        newMapIndex++;
        
        for (int i = 1; i < _currentLevels.Count; i++)
        {
            Level level = _currentLevels[i];
            Vector2 size = GetLevelSizeInUnits(level);
            Vector3 position = FindClosestFitPosition(size);
            SpawnLevelAtPosition(newMapIndex, level, position);
            AddRectForLevel(level, position);
            newMapIndex++;
        }

        Spawned?.Invoke(GetOllMapsElements());
        //FocusCameraOnLevels();
    }
    
    public Transform GetItemTransform(int mapIndex, int x, int y)
    {
        foreach (var item in _mapItemsGoups[mapIndex])
        {
            if(item.Position.X == x & item.Position.Y == y)
                return item.transform;
        }
    
        throw new ArgumentException($"Объект по X{x} Y{y} не найден!");
    }

    public void AddChangingItems(int mapIndex, List<MapItem> changingItems)
    {
        if (_changingItemsMap.ContainsKey(mapIndex) == false)
        {
            _changingItemsMap.Add(mapIndex, changingItems);
            return;
        }
        
        _changingItemsMap[mapIndex] = changingItems;
    }

    public void ChangeItem(int mapIndex, Transform item)
    {
        if (item.TryGetComponent(out MapItem mapItem) == false)
            throw new ArgumentNullException(
                $"На объекте остутствует компонент {nameof(MapItem)}! Это может быть вызвано не правильным Transform, или же компонент {nameof(MapItem)} остутствует на тайле.");
        
        if (item.TryGetComponent(out SpriteRenderer spriteRenderer) == false)
            throw new ArgumentNullException(
                $"На объекте остутствует компонент {nameof(SpriteRenderer)}! " +
                $"Это может быть вызвано не правильным Transform, или же компонент {nameof(SpriteRenderer)} остутствует на тайле.");
        
        mapItem.SetType(MapItemType.TailPlayer);
        spriteRenderer.sprite = _currentLevels[mapIndex].Sprites[MapItemType.TailPlayer];
    }

    private Vector2 GetLevelSizeInUnits(Level level)
    {
        int width = level.Map.GridSize.x;
        int height = level.Map.GridSize.y;

        return new Vector2(width * _tileSize.x, height * _tileSize.y);
    }
    
    private void SpawnLevelAtPosition(int newMapIndex, Level level, Vector3 position)
    {
        List<MapItem> mapItems = new();
        Array2DInt map = level.Map;
        _mapData.SetCurrentMap(newMapIndex, map.GetCells());

        int width = map.GridSize.x;
        int height = map.GridSize.y;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int tileType = map.GetCell(x, y);
                Sprite sprite = level.Sprites[(MapItemType)tileType];

                var newTile = Instantiate(_mapItemPrefab, _spawnHub);
                newTile.name = $"Tile_{x}_{y}";

                Vector3 tilePosition = position + new Vector3(x * _tileSize.x, (height - y - 1) * _tileSize.y, 0);
                newTile.transform.position = tilePosition;

                var spriteRenderer = newTile.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;

                MapItem mapItem = newTile.GetComponent<MapItem>();
                
                if (mapItem == null)
                    mapItem = newTile.AddComponent<MapItem>();
                
                mapItem.SetType((MapItemType)tileType);

                PositionInMap positionInMap = new PositionInMap { X = x, Y = y };
                mapItem.SetPositionInMap(positionInMap);

                mapItems.Add(mapItem);
                
                TrySpawnPlayer(tileType, newMapIndex, newTile.transform);
            }
        }
        
        _mapItemsGoups.Add(mapItems);
    }

    private void AddRectForLevel(Level level, Vector3 position)
    {
        int width = level.Map.GridSize.x;
        int height = level.Map.GridSize.y;
        Vector2 size = new Vector2(width * _tileSize.x, height * _tileSize.y);
        
        Rect rect = new Rect(
            position.x - size.x / 2f,
            position.y - size.y / 2f,
            size.x,
            size.y
        );
        
        _placedRects.Add(rect);
    }

    private Vector3 FindClosestFitPosition(Vector2 size)
    {
        float maxRadius = 50f;
        float step = _tileSize.x;
        int minCountPoints = 8;
        
        float radius = 0f;

        while (radius <= maxRadius)
        {
            int pointsCount = Mathf.CeilToInt(2 * Mathf.PI * radius / step);
            pointsCount = Math.Max(pointsCount, minCountPoints);

            for (int i = 0; i < pointsCount; i++)
            {
                float angle = i * 2 * Mathf.PI / pointsCount;
                Vector3 candidatePos = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0);

                Rect candidateRect = new Rect(
                    candidatePos.x - size.x / 2f,
                    candidatePos.y - size.y / 2f,
                    size.x,
                    size.y
                );

                if (IsPositionFree(candidateRect))
                {
                    return candidatePos;
                }
            }

            radius += step;
        }
        
        return Vector3.zero;
    }

    private bool IsPositionFree(Rect rect)
    {
        foreach (var placedRect in _placedRects)
        {
            if (placedRect.Overlaps(rect))
                return false;
        }
        return true;
    }
    
    private void TrySpawnPlayer(int currentType, int mapIndex, Transform playerTransform)
    {
        if(_playersSpawner == null)
            throw new ArgumentNullException(nameof(_playersSpawner));
        
        if (currentType == (int)MapItemType.Player)
            _playersSpawner.Spawn(mapIndex, playerTransform);
    }
    
    private List<Transform> GetOllMapsElements()
    {
        List<Transform> items = new();

        foreach (var mapItems in _mapItemsGoups)
        {
            foreach (var item in mapItems)
            {
                items.Add(item.transform);
            }
        }

        
        return items;
    }
}