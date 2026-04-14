using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllLevels", menuName = "Maps/Create AllLevelsCollector", order = 51)]
public class AllLevels : ScriptableObject
{ 
    [SerializeField] private List<Level> _levelsData;
    
    public int CountLevels => _levelsData.Count;

    public Level GetLevel(int index)
    {
        if (index > _levelsData.Count - 1)
            throw new IndexOutOfRangeException();
        
        if (index < 0)
            throw new IndexOutOfRangeException();
        
        return _levelsData[index];
    }

    public LevelOpeningType GetLevelOpeningType(int index)
    {
        return _levelsData[index].OpeningType;
    }
}