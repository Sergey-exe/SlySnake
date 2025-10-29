using System.Collections.Generic;
using UnityEngine;

public class PlayersSpawner : MonoBehaviour
{
    [SerializeField] private Player _prefab;
    
    public int PlayersCount => _players.Count;
    
    private Dictionary<int, Transform> _players = new();

    public void Spawn(int mapIndex, Transform spawnPoint)
    {
        Player player = Instantiate(_prefab, spawnPoint.position, spawnPoint.rotation);
        
        _players.Add(mapIndex, player.transform);
    }

    public Transform GetPlayerTransform(int mapIndex)
    {
        return _players[mapIndex];
    }
}
