using System;
using System.Collections.Generic;
using _Sources.Map;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New level", menuName = "Maps/Create new level", order = 51)]
public class Level : ScriptableObject
{
    [SerializeField] private LevelOpeningType  _openingType;
    [SerializeField] private EnvironmentSetsType _environmentSetType;
    [SerializeField] private List<Maze> _mazes;
    
    public LevelOpeningType OpeningType => _openingType;
    public EnvironmentSetsType EnvironmentSetType => _environmentSetType;

    public List<Maze> GetMazes()
    {
        return _mazes;
    }
}
