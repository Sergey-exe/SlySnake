using System;
using System.Collections.Generic;
using System.Linq;
using _Sources.Map;
using UnityEngine;

public class PlayersWayBuilder : MonoBehaviour
{
    [SerializeField] private MechanismsActivator _mechanismsActivator;
    
    private List<Map> _maps = new();

    public void SetMaps(List<Map> map)
    {
        _maps = map ?? throw new ArgumentNullException(nameof(map));
    }

    public void Revert()
    {
        _maps.Clear();
    }
    
    public List<Transform> SearchWay(int index, GameMapVector2 direction)
    {
        bool stopWay = false;
        
        if(_maps.Count == 0)
            throw new Exception("Карт для нахождения пути не существует!");
        
        List<Transform> waypoints = new();
        GameMapVector2 playerPosition = _maps[index].SearchPlayer();
        
        int[,] currentMap = _maps[index].GetCurrentMap();

        int x = playerPosition.X;
        int y = playerPosition.Y;
        
        
        int wayLength;
        
        if (direction.Y != 0 & direction.X == 0)
            wayLength = currentMap.GetLength(0) - (playerPosition.Y + 1) * direction.Y;
        else if (direction.X != 0 & direction.Y == 0)
            wayLength = currentMap.GetLength(1) - (playerPosition.X + 1) * direction.X;
        else
            return null;
        
        for (int i = 0; i < wayLength; i++)
        {
            x += direction.X;
            y += direction.Y;
            
            if (currentMap[x, y] == (int)MapItemType.Wall || currentMap[x, y] == (int)MapItemType.TailPlayer)
            {
                currentMap[playerPosition.X, playerPosition.Y] = (int)MapItemType.TailPlayer;
                currentMap[x - direction.X, y - direction.Y] = (int)MapItemType.Player;
                break;
            }
            else if(currentMap[x, y] == (int)MapItemType.Trap)
            {
                _mechanismsActivator.ActivateTrap();
                stopWay = true;
            }
            
            currentMap[x, y] = (int)MapItemType.TailPlayer;
            waypoints.Add(_maps[index].GetItemTransform(y, x));
            
            if(stopWay)
                break;
        }

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