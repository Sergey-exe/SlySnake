using System.Collections.Generic;
using UnityEngine;

namespace _Sources.Player
{
    public class PlayersTransformData
    {
        private Dictionary<int, Transform> _players = new();
    
        public int PlayersCount => _players.Count;

        public void SetTransform(int index, Transform transform)
        {
            if(_players.ContainsKey(index))
                return;
        
            _players.Add(index, transform);
        }

        public Transform GetTransform(int index)
        {
            return _players[index];
        }

        public void Revert()
        {
            _players.Clear();
        }
    }
}