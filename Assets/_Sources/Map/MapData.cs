using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    private Dictionary<int, int[,]> _currentMaps = new();

    public void SetCurrentMap(int index, int[,] map)
    {
        if (_currentMaps.ContainsKey(index))
        {
            _currentMaps[index] = map;
            return;
        }
        
        _currentMaps.Add(index, map.Clone() as int[,]);
    }

    public int[,] GetCurrentMap(int index)
    {
        return _currentMaps[index].Clone() as int[,];
    }
}
