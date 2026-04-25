using System.Collections;
using System.Collections.Generic;
using _Sources.Map;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpriteSet", menuName = "SpriteSet/Create new SpriteSet", order = 51)]
public class SpriteSet : ScriptableObject
{
    [SerializedDictionary("Тип элемента", "Спрайт элемента")] 
    public SerializedDictionary<MapItemType, Sprite> Sprites;
}
