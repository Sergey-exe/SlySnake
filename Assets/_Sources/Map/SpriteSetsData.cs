using AYellowpaper.SerializedCollections;
using UnityEngine;

public class SpriteSetsData : MonoBehaviour
{
    [SerializedDictionary("Тип спрайтсета", "Спрайтсет")] 
    public SerializedDictionary<SpriteSetType, SpriteSet> SpriteSets;
}