using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New level", menuName = "Maps/Create new level", order = 51)]
public class Level : ScriptableObject
{
    [SerializeField] private LevelOpeningType  _openingType;
    [SerializeField] private List<Maze> _mazes;
    
    public LevelOpeningType OpeningType => _openingType;

    public List<Maze> GetMazes()
    {
        return _mazes;
    }
}
