using System;
using System.Collections.Generic;
using _Sources.Map;
using UnityEngine;

public class Map : MonoBehaviour
{
    private Level _currentLevel;
    private List<MapItem> _items;
    private MapData _mapData;
    private bool _isInit;
    
    public int Index { get; private set; }

    public void Init(List<MapItem> items, Level level, MapData mapData, int index)
    {
        if(index < 0)
            throw new IndexOutOfRangeException(nameof(index));
        
        Index = index;
        _items = items ?? throw new ArgumentNullException(nameof(items));
        _currentLevel = level ?? throw new ArgumentNullException(nameof(level));
        _mapData = mapData ?? throw new ArgumentNullException(nameof(mapData));
        _isInit = true;
    }
    
    public Transform GetItemTransform(int x, int y)
    {
        foreach (var item in _items)
        {
            if(item.Position.X == x & item.Position.Y == y)
                return item.transform;
        }
    
        throw new ArgumentException($"Объект по X{x} Y{y} не найден!");
    }
    
    public void ChangeItem(Transform item)
    {
        if (item.TryGetComponent(out MapItem mapItem) == false)
            throw new ArgumentNullException(
                $"На объекте остутствует компонент {nameof(MapItem)}! Это может быть вызвано не правильным Transform, или же компонент {nameof(MapItem)} остутствует на тайле.");
        
        if (item.TryGetComponent(out SpriteRenderer spriteRenderer) == false)
            throw new ArgumentNullException(
                $"На объекте остутствует компонент {nameof(SpriteRenderer)}! " +
                $"Это может быть вызвано не правильным Transform, или же компонент {nameof(SpriteRenderer)} остутствует на тайле.");
        
        mapItem.SetType(MapItemType.TailPlayer);
        spriteRenderer.sprite = _currentLevel.Sprites[MapItemType.TailPlayer];
    }
    
    public GameMapVector2 SearchPlayer()
    {
        int[,] map = _mapData.GetCurrentMap();
        
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

    public List<Transform> GetItemTransforms()
    {
        List<Transform> items = new List<Transform>();
        
        foreach (var item in _items)
        {
            items.Add(item.GetComponent<Transform>());
        }
        
        return items;
    }

    public int[,] GetCurrentMap()
    {
        return _mapData.GetCurrentMap();
    }

    public void SetCurrentMap(int[,] map)
    {
        _mapData.SetCurrentMap(map);
    }

    public bool HasEmptyItems()
    {
        foreach (var item in _items)
        {
            if(item.MapItemType == MapItemType.Empty)
                return true;
        }
        
        return false;
    }
}
