using System;
using System.Collections.Generic;
using _Sources.Map;
using UnityEngine;

public class PlayerWayBuilder : MonoBehaviour
{
    private List<Map> _maps = new();

    public void SetMaps(List<Map> map)
    {
        _maps = map ?? throw new ArgumentNullException(nameof(map));
    }
    
    public List<Transform> SearchWay(int index, GameMapVector2 direction)
    {
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
            throw new InvalidOperationException("Движение по двум осям одновременно недопустимо");
        
        for (int i = 0; i < wayLength; i++)
        {
            x += direction.X;
            y += direction.Y;
            
            if (currentMap[x, y] == (int)MapItemType.Empty)
            {
                currentMap[x, y] = (int)MapItemType.TailPlayer;
                waypoints.Add(_maps[index].GetItemTransform(y, x));
            }
            else
            {
                currentMap[playerPosition.X, playerPosition.Y] = (int)MapItemType.TailPlayer;
                currentMap[x - direction.X, y - direction.Y] = (int)MapItemType.Player;
                break;
            }
        }

        _maps[index].SetCurrentMap(currentMap);
        
        return waypoints;
    }
}