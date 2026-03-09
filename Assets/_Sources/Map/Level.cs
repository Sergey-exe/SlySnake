using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New level", menuName = "Maps/Create new level", order = 51)]
public class Level : ScriptableObject
{
    [SerializeField] private LevelOpeningType  _openingType;
    [SerializeField] private List<Maze> _levels;

    public List<Maze> GetLevels()
    {
        return _levels;
    }
}
