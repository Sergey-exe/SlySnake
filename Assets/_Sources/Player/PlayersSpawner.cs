using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Sources.Player
{
    public class PlayersSpawner : MonoBehaviour
    {
        [SerializeField] private global::Player _prefab;
        [SerializeField] private List<global::Player> _players;
    
        private PlayersTransformData _playersTransformData;

        private bool _isInit;

        public void Init(PlayersTransformData playersTransformData)
        {
            _playersTransformData = playersTransformData ?? throw new ArgumentNullException(nameof(playersTransformData));
        
            _isInit = true;
        }

        public void Spawn(int mapIndex, Sprite sprite, Transform spawnPoint)
        {
            if (!_isInit)
                throw new Exception($"Класс {nameof(PlayersSpawner)} не инициализирован!");
        
            global::Player player = Instantiate(_prefab, spawnPoint.position, spawnPoint.rotation);
            
            player.GetComponent<SpriteRenderer>().sprite = sprite;
        
            _playersTransformData.SetTransform(mapIndex, player.transform);
        
            _players.Add(player);
        }
        
        public void Revert()
        {
            _playersTransformData.Revert();
        
            foreach (var player in _players)
                Destroy(player.gameObject);
        
            _players.Clear();
        }
    }
}