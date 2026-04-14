using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Sources.Map
{
    public class SpriteSetsData : MonoBehaviour
    {
        [SerializedDictionary("Тип спрайтсета", "Спрайтсет")] 
        public SerializedDictionary<SpriteSetType, SpriteSet> SpriteSets;
    }
}