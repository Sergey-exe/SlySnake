using System;
using System.Collections.Generic;
using _Sources.Map;
using Array2DEditor;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _mapItemPrefab;
    [SerializeField] private Transform _mapsCollector;
    [SerializeField] private PlayersSpawner _playersSpawner;
    [SerializeField] private PlayerWayBuilder _wayBuilder;
    [SerializeField] private MapItemChanger _mapItemChanger;
    [SerializeField] private List<Level> _currentLevels;
    
    private Vector2 _tileSize;
    private List<Map> _maps;
    private List<Rect> _placedRects = new(); 
    private bool _isInit;

    public event Action<List<Transform>> Spawned; 

    public void Init(MapData mapData)
    {
        _maps = new List<Map>();
        _tileSize = _mapItemPrefab.GetComponent<SpriteRenderer>().bounds.size;
        _isInit = true;
    }

    public void SetLevels(List<Level> levels)
    {
        _currentLevels = levels ?? throw new ArgumentNullException(nameof(levels)); 
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
        _wayBuilder.SetMaps(_maps);
        _mapItemChanger.SetMaps(_maps);
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
        Array2DInt levelMap = level.Map;

        MapData mapData = new MapData();
        
        mapData.SetCurrentMap(levelMap.GetCells());

        int width = levelMap.GridSize.x;
        int height = levelMap.GridSize.y;

        Player player = null;

        GameObject mapObject = new GameObject();
        mapObject.name = level.Name;
        mapObject.transform.SetParent(_mapsCollector);
        Map map = mapObject.AddComponent<Map>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int tileType = levelMap.GetCell(x, y);
                Sprite sprite = level.Sprites[(MapItemType)tileType];

                
                var newTile = Instantiate(_mapItemPrefab, map.transform);
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
        
        map.Init(mapItems, _currentLevels[newMapIndex], mapData, newMapIndex);
        _maps.Add(map);
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

        foreach (var map in _maps)
        {
            foreach (var item in map.GetItemTransforms())
            {
                items.Add(item);
            }
        }

        
        return items;
    }
}