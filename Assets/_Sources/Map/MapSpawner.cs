using System;
using System.Collections.Generic;
using _Sources.Player;
using Array2DEditor;
using UnityEngine;

namespace _Sources.Map
{
    public class MapSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _mapItemPrefab;
        [SerializeField] private Transform _mapsCollector;
        [SerializeField] private PlayersSpawner _playersSpawner;
        [SerializeField] private PlayersWayBuilder _wayBuilder;
        [SerializeField] private MapItemChanger _mapItemChanger;
        [SerializeField] private MapsProgressCollection _mapsProgressCollection;
        [SerializeField] private AllLevels _levels;
        [SerializeField] private SpriteSetsData _spriteSetsData;
        [SerializeField] private EnvironmentSetsData _environmentSetsData;
        [SerializeField] private Camera _camera;
        [SerializeField] private SoundChanger _soundChanger;

        private Vector2 _tileSize;
        private List<global::_Sources.Map.Map> _maps;
        private List<Rect> _placedRects = new();
        private bool _isInit;

        public int CurrentLevelIndex { get; private set; } = 0;

        public event Action<List<Transform>> Spawned;
        public event Action<int> OnNextLevel;
        public event Action<int, LevelOpeningType> IsGotToAd;

        public void Init(MapData mapData)
        {
            _maps = new List<global::_Sources.Map.Map>();
            _tileSize = _mapItemPrefab.GetComponent<SpriteRenderer>().bounds.size;
            _isInit = true;
        }

        public void SetCurrentLevelIndex(int index)
        {
            if (index < 0 || index >= _levels.CountLevels)
                throw new ArgumentOutOfRangeException(nameof(index));

            CurrentLevelIndex = index;
        }

        public void RestartLevel()
        {
            Revert(false);
            SpawnMap();
        }

        public void NextLevel()
        {
            int step = 1;
            int nextIndex = CurrentLevelIndex + step;

            if (_levels.GetLevelOpeningType(nextIndex) == LevelOpeningType.ClosedOrAD)
            {
                step++;
                IsGotToAd?.Invoke(nextIndex, LevelOpeningType.ClosedOrAD);
            }

            CurrentLevelIndex = (CurrentLevelIndex + step) % _levels.CountLevels;

            RestartLevel();
            OnNextLevel?.Invoke(step);
        }

        [ContextMenu(nameof(SpawnMap))]
        public void SpawnMap()
        {
            if (!_isInit)
            {
                Debug.LogError("Класс не инициализирован!");
                return;
            }

            _placedRects.Clear();
            Level currentLevel = _levels.GetLevel(CurrentLevelIndex);
            List<Maze> mazes = currentLevel.GetMazes();

            int index = 0;

            Maze firstMaze = mazes[0];

            EnvironmentSetsType environmentSetType = currentLevel.EnvironmentSetType;
            SetBackground(environmentSetType);
            SpawnLevelAtPosition(index, firstMaze, environmentSetType, Vector3.zero);
            AddRectForLevel(firstMaze, Vector3.zero);
            index++;

            for (int i = 1; i < mazes.Count; i++)
            {
                Maze maze = mazes[i];

                Vector2 size = GetLevelSizeInUnits(maze);
                Vector3 pos = FindClosestFitPosition(size);

                SpawnLevelAtPosition(index, maze, environmentSetType, pos);
                AddRectForLevel(maze, pos);

                index++;
            }

            Spawned?.Invoke(GetOllMapsElements());
            _wayBuilder.SetMaps(_maps);
            _mapItemChanger.SetMaps(_maps);
        }
        
        public void Revert(bool setDefault)
        {
            foreach (Transform child in _mapsCollector)
                Destroy(child.gameObject);

            _mapItemChanger.Revert();
            _wayBuilder.Revert();
            
            if(setDefault)
                SetBackground(EnvironmentSetsType.Default);
        }

        private Vector2 GetLevelSizeInUnits(Maze maze)
        {
            return new Vector2(
                maze.Map.GridSize.x * _tileSize.x,
                maze.Map.GridSize.y * _tileSize.y
            );
        }

        private void SetBackground(EnvironmentSetsType backgroundType)
        {
            _camera.backgroundColor = _environmentSetsData.Environments[backgroundType].BackgroundColor;
            _soundChanger.ChangeSound(_environmentSetsData.Environments[backgroundType].BackgroundSound);

            foreach (var gameObject in _environmentSetsData.Environments[backgroundType].SpecialObjects)
            {
                if (gameObject == null)
                    continue;
                    
                Instantiate(gameObject, _mapsCollector);
            }
        }

        private void SpawnLevelAtPosition(int newMapIndex, Maze maze, EnvironmentSetsType backgroundType, Vector3 position)
        {
            List<MapItem> mapItems = new();
            Array2DInt levelMap = maze.Map;

            MapData mapData = new MapData();
            mapData.SetCurrentMap(levelMap.GetCells());

            int width = levelMap.GridSize.x;
            int height = levelMap.GridSize.y;

            GameObject mapObject = new GameObject();
            mapObject.transform.SetParent(_mapsCollector);

            global::_Sources.Map.Map map = new global::_Sources.Map.Map();
            MapProgressHandler mapProgressHandler = new(map);
            _mapsProgressCollection.AddHandler(mapProgressHandler);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int tileType = levelMap.GetCell(x, y);
                    
                    MapItemType spriteType = (MapItemType)tileType;
                    
                    Sprite sprite = _spriteSetsData.SpriteSets[maze.Type]
                        .Sprites[spriteType];

                    var tile = Instantiate(_mapItemPrefab, mapObject.transform);
                    tile.name = $"Tile_{x}_{y}";

                    Vector3 tilePosition = position + new Vector3(
                        x * _tileSize.x,
                        (height - y - 1) * _tileSize.y,
                        0);

                    tile.transform.position = tilePosition;

                    var spriteRenderer = tile.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprite;

                    MapItem mapItem = tile.GetComponent<MapItem>();
                    
                    if (mapItem == null)
                        mapItem = tile.AddComponent<MapItem>();

                    mapItem.SetType((MapItemType)tileType);
                    mapItem.SetPositionInMap(new PositionInMap { X = x, Y = y });

                    mapItems.Add(mapItem);

                    if (tileType != (int)MapItemType.Player)
                        continue;
                    
                    spriteType = MapItemType.TailPlayer;
                        
                    TrySpawnPlayer(tileType, newMapIndex, sprite, tile.transform, _environmentSetsData.Environments[backgroundType].ImpactSound);
                        
                    sprite = _spriteSetsData.SpriteSets[maze.Type]
                        .Sprites[spriteType];
                        
                    spriteRenderer.sprite = sprite;
                }
            }

            map.Init(mapItems, maze.Type, mapData, _spriteSetsData, newMapIndex);
            _maps.Add(map);
        }

        private void AddRectForLevel(Maze maze, Vector3 position)
        {
            Vector2 size = GetLevelSizeInUnits(maze);

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
            int minPoints = 8;

            float radius = 0f;

            while (radius <= maxRadius)
            {
                int points = Mathf.CeilToInt(2 * Mathf.PI * radius / step);
                points = Math.Max(points, minPoints);

                for (int i = 0; i < points; i++)
                {
                    float angle = i * 2 * Mathf.PI / points;

                    Vector3 candidate = new Vector3(
                        radius * Mathf.Cos(angle),
                        radius * Mathf.Sin(angle),
                        0);

                    Rect rect = new Rect(
                        candidate.x - size.x / 2f,
                        candidate.y - size.y / 2f,
                        size.x,
                        size.y
                    );

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
                if (r.Overlaps(rect))
                    return false;

            return true;
        }

        private void TrySpawnPlayer(int tileType, int mapIndex, Sprite sprite, Transform transform, AudioClip impactSound)
        {
            if (_playersSpawner == null)
                throw new ArgumentNullException(nameof(_playersSpawner));

            if (tileType == (int)MapItemType.Player)
                _playersSpawner.Spawn(mapIndex, sprite, transform, impactSound);
        }

        private List<Transform> GetOllMapsElements()
        {
            List<Transform> items = new();

            foreach (var map in _maps)
                items.AddRange(map.GetItemTransforms());

            return items;
        }
    }
}