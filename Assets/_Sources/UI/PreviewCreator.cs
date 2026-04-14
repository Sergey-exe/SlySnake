using _Sources.Map;
using UnityEngine;

namespace _Sources.UI
{
    public class PreviewCreator : MonoBehaviour
    {
        [SerializeField] private MapSpawner _spawner;
        [SerializeField] private Transform _spawnPoint;
        
        [ContextMenu(nameof(CreatePreview))]
        public void CreatePreview()
        {
            //_spawner.SpawnLite(2, _spawnPoint);
        }
    }
}
