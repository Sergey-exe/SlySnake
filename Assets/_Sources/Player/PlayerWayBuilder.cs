using System;
using System.Collections.Generic;
using _Sources.Map;
using UnityEngine;

public class PlayerWayBuilder : MonoBehaviour
{
    [SerializeField] MapData _mapData;
    [SerializeField] MapSpawner _mapSpawner;
    
    public List<Transform> SearchWay(int mapIndex, GameMapVector2 direction, GameMapVector2 playerPosition)
    {
        List<Transform> waypoints = new();
        int[,] currentMap = _mapData.GetCurrentMap(mapIndex);

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
                waypoints.Add(_mapSpawner.GetItemTransform(mapIndex, y, x));
            }
            else
            {
                currentMap[playerPosition.X, playerPosition.Y] = (int)MapItemType.TailPlayer;
                currentMap[x - direction.X, y - direction.Y] = (int)MapItemType.Player;
                break;
            }
        }

        _mapData.SetCurrentMap(mapIndex, currentMap);
        
        return waypoints;
    }
}