using _Sources.Map;
using Array2DEditor;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "New maze", menuName = "Maps/Create new maze", order = 51)]
public class Maze : ScriptableObject
{
    [field: SerializeField] public SpriteSetType Type { get; private set; }

    [field: SerializeField] public string Name { get; private set; }
    
    [field: SerializeField] public Array2DInt Map { get; private set; }
}