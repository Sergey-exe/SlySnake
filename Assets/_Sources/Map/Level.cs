using _Sources.Map;
using Array2DEditor;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "New level", menuName = "Maps/Create new level", order = 51)]
public class Level : ScriptableObject
{
    [field: SerializeField] public SpriteSetType Type { get; private set; }

    [field: SerializeField] public string Name { get; private set; }
    
    [field: SerializeField] public Array2DInt Map { get; private set; }
}