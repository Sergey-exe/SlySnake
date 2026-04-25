using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Sources.Map
{
    public class EnvironmentSetsData : MonoBehaviour
    {
        [SerializedDictionary("Тип окружения", "Сет окружения")] 
        public SerializedDictionary<EnvironmentSetsType, EnvironmentSet> Environments;
    }
}
