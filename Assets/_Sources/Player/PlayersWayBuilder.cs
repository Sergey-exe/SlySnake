using System;
using System.Collections.Generic;
using System.Linq;
using _Sources.Map;
using UnityEngine;

namespace _Sources.Player
{
    public class PlayersWayBuilder : MonoBehaviour
    {
        [SerializeField] private MechanismsActivator _mechanismsActivator;
    
        private List<global::_Sources.Map.Map> _maps = new();

        public void SetMaps(List<global::_Sources.Map.Map> map)
        {
            _maps = map ?? throw new ArgumentNullException(nameof(map));
        }

        public void Revert()
        {
            _maps.Clear();
        }
    
        public List<Transform> SearchWay(int index, GameMapVector2 direction)
        {
            if (_maps.Count == 0) throw new Exception("Карт не существует!");

            List<Transform> waypoints = new();
            GameMapVector2 playerPosition = _maps[index].SearchPlayer();
            int[,] currentMap = _maps[index].GetCurrentMap();

            int rows = currentMap.GetLength(0);
            int cols = currentMap.GetLength(1);

            int x = playerPosition.X;
            int y = playerPosition.Y;
    
            bool canContinue = true;

            while (canContinue)
            {
                int nextX = x + direction.X;
                int nextY = y + direction.Y;
                
                if (nextX < 0 || nextX >= rows || nextY < 0 || nextY >= cols)
                {
                    canContinue = false;
                }

                else if (currentMap[nextX, nextY] == (int)MapItemType.Wall || 
                         currentMap[nextX, nextY] == (int)MapItemType.TailPlayer)
                {
                    canContinue = false;
                }
                else 
                {
                    x = nextX;
                    y = nextY;
                    
                    if (currentMap[x, y] == (int)MapItemType.Trap)
                    {
                        _mechanismsActivator.ActivateTrap();
                        canContinue = false; 
                    }

                    currentMap[x, y] = (int)MapItemType.TailPlayer;
                    waypoints.Add(_maps[index].GetItemTransform(y, x));
                }
            }
            
            currentMap[playerPosition.X, playerPosition.Y] = (int)MapItemType.TailPlayer;
            currentMap[x, y] = (int)MapItemType.Player;

            _maps[index].SetCurrentMap(currentMap);
            return waypoints;
        }

        public bool HasFreeWays()
        {
            for (int i = 0; i < _maps.Count; i++)
            {
                if(HasFreeWay(i))
                    return true;
            }
        
            return false;
        }
    
        private bool HasFreeWay(int index)
        {
            GameMapVector2 playerPosition = _maps[index].SearchPlayer();
            int[,] map = _maps[index].GetCurrentMap();

            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            (int deltaX, int deltaY)[] dirs =
            {
                (-1, 0), 
                ( 1, 0), 
                ( 0,-1), 
                ( 0, 1)  
            };

            return dirs.Any(d =>
            {
                int x = playerPosition.X + d.deltaX;
                int y = playerPosition.Y + d.deltaY;

                return x >= 0 && x < rows &&
                       y >= 0 && y < cols &&
                       map[x, y] == 0;
            });
        }
    }
}