using _Sources.Map;
using Array2DEditor;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "Mew level", menuName = "Maps/Crete new level", order = 51)]
public class Level : ScriptableObject
{
    [SerializedDictionary("Тип элемента", "Спрайт элемента")] 
    public SerializedDictionary<MapItemType, Sprite> Sprites;

    [field: SerializeField] public Array2DInt Map { get; private set; }
}