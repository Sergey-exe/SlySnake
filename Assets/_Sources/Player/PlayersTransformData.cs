using System.Collections.Generic;
using UnityEngine;

public class PlayersTransformData
{
    private Dictionary<int, Transform> _players = new();
    
    public int PlayersCount => _players.Count;

    public void SetTransform(int index, Transform transform)
    {
        _players.Add(index, transform);
    }

    public Transform GetTransform(int index)
    {
        return _players[index];
    }
}