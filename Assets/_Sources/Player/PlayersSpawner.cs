using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayersSpawner : MonoBehaviour
{
    [SerializeField] private Player _prefab;
    [SerializeField] private List<Player> _players;
    
    private PlayersTransformData _playersTransformData;

    private bool _isInit;

    public void Init(PlayersTransformData playersTransformData)
    {
        _playersTransformData = playersTransformData ?? throw new ArgumentNullException(nameof(playersTransformData));
        
        _isInit = true;
    }

    public void Spawn(int mapIndex, Transform spawnPoint)
    {
        if (!_isInit)
            throw new Exception($"Класс {nameof(PlayersSpawner)} не инициализирован!");
        
        Player player = Instantiate(_prefab, spawnPoint.position, spawnPoint.rotation);
        
        _playersTransformData.SetTransform(mapIndex, player.transform);
        
        _players.Add(player);
    }

    [ContextMenu(nameof(Revert))]
    public void Revert()
    {
        _playersTransformData.Revert();
        
        foreach (var player in _players)
            Destroy(player.gameObject);
        
        _players.Clear();
    }
}