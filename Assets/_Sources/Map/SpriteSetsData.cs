using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Sources.Map
{
    public class SpriteSetsData : MonoBehaviour
    {
        public SpriteSetsType spriteSetsType;
        
        [SerializedDictionary("Тип окружения", "Спрайтсет")] 
        public SerializedDictionary<SpriteSetsType, SpriteSet> SpriteSets;
    }
}