using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;
using _Sources.Map;

public class MapFactory : MonoBehaviour
{
    [SerializeField] private GameObject _mapItemPrefab;
    [SerializeField] private Transform _mapsCollector;
    [SerializeField] private SpriteSetsData _spriteSetsData;

    public Vector2 GetTileSize() => _mapItemPrefab.GetComponent<SpriteRenderer>().bounds.size;

    public void ClearCollector()
    {
        foreach (Transform child in _mapsCollector)
            Destroy(child.gameObject);
    }

    public Map SpawnLevelAtPosition(
        int newMapIndex, 
        Maze maze, 
        Vector3 originPosition, 
        Vector2 tileSize,
        out List<PlayerSpawnPoint> playerSpawnPoints)
    {
        playerSpawnPoints = new List<PlayerSpawnPoint>();
        List<MapItem> mapItems = new();
        Array2DInt levelMap = maze.Map;

        MapData mapData = new MapData();
        mapData.SetCurrentMap(levelMap.GetCells());

        int width = levelMap.GridSize.x;
        int height = levelMap.GridSize.y;

        GameObject mapObject = new GameObject($"Maze_{newMapIndex}");
        mapObject.transform.SetParent(_mapsCollector);

        Map map = new();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int tileType = levelMap.GetCell(x, y);
                MapItemType spriteType = (MapItemType)tileType;
                
                Sprite sprite = _spriteSetsData.SpriteSets[maze.Type].Sprites[spriteType];

                var tile = Instantiate(_mapItemPrefab, mapObject.transform);
                tile.name = $"Tile_{x}_{y}";

                Vector3 tilePosition = originPosition + new Vector3(
                    x * tileSize.x,
                    (height - y - 1) * tileSize.y,
                    0);

                tile.transform.position = tilePosition;

                var spriteRenderer = tile.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;

                MapItem mapItem = tile.GetComponent<MapItem>() ?? tile.AddComponent<MapItem>();
                mapItem.SetType(spriteType);
                mapItem.SetPositionInMap(new PositionInMap { X = x, Y = y });
                mapItems.Add(mapItem);

                if (tileType == (int)MapItemType.Player) 
                {
                    playerSpawnPoints.Add(new PlayerSpawnPoint {
                        MapIndex = newMapIndex,
                        DefaultSprite = sprite,
                        Transform = tile.transform,
                        TailSprite = _spriteSetsData.SpriteSets[maze.Type].Sprites[MapItemType.TailPlayer]
                    });
                }
            }
        }

        map.Init(mapItems, maze.Type, mapData, _spriteSetsData, newMapIndex);
        return map;
    }
}

public struct PlayerSpawnPoint
{
    public int MapIndex;
    public Sprite DefaultSprite;
    public Sprite TailSprite;
    public Transform Transform;
}
