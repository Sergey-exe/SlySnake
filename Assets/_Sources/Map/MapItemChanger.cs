using System;
using System.Collections.Generic;
using UnityEngine;

public class MapItemChanger : MonoBehaviour
{
    [SerializeField] private PlayersMover _playersMover;
    
    private List<Map> _maps;

    public void OnEnable()
    {
        _playersMover.InPoint += ChangeItem;
    }

    public void OnDisable()
    {
        _playersMover.InPoint -= ChangeItem;
    }

    public void SetMaps(List<Map> maps)
    {
        _maps = maps ?? throw new ArgumentNullException(nameof(maps));
    }
    
    public void ChangeItem(int mapIndex, Transform item)
    {
        if (_maps == null)
            return;
        
        if(_maps.Count == 0)
            return;
        
        foreach (var map in _maps)
        {
            if (map.Index == mapIndex)
            {
                map.ChangeItem(item);
                return;
            }
        }
    }
}