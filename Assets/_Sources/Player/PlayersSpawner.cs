using System;
using UnityEngine;

public class PlayersSpawner : MonoBehaviour
{
    [SerializeField] private Player _prefab;
    [SerializeField] private Player _player;
    
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
        
        _player = Instantiate(_prefab, spawnPoint.position, spawnPoint.rotation);
        
        _playersTransformData.SetTransform(mapIndex, _player.transform);
    }

    [ContextMenu(nameof(Revert))]
    public void Revert()
    {
        _playersTransformData.Revert();
        Destroy(_player.gameObject);
    }
}