using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Sources.Map
{
    public class Map 
    {
        private SpriteSetType _spriteSetType;
        private List<MapItem> _items;
        private MapData _mapData;
        private SpriteSetsData _spriteSetsData;
    
        public int Index { get; private set; }

        public void Init(List<MapItem> items, SpriteSetType spriteSetType, MapData mapData, SpriteSetsData spriteSetsData, int index)
        {
            if(index < 0)
                throw new IndexOutOfRangeException(nameof(index));
        
            Index = index;
            _spriteSetType = spriteSetType;
            _items = items ?? throw new ArgumentNullException(nameof(items));
            _mapData = mapData ?? throw new ArgumentNullException(nameof(mapData));
            _spriteSetsData = spriteSetsData ?? throw new ArgumentNullException(nameof(spriteSetsData));
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
            spriteRenderer.sprite = _spriteSetsData.SpriteSets[_spriteSetType].Sprites[MapItemType.TailPlayer];
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
    }
}