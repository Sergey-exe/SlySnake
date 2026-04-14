using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Sources.Map
{
    public class MapsProgressCollection : MonoBehaviour
    {
        private List<MapProgressHandler> _mapsProgressHandlers = new();

        public void AddHandler(MapProgressHandler mapProgressHandler)
        {
            if(mapProgressHandler == null)
                throw new ArgumentNullException(nameof(mapProgressHandler));
        
            _mapsProgressHandlers.Add(mapProgressHandler);
        }

        public bool HasEmptyItems()
        {
            foreach (var progressHandler in _mapsProgressHandlers)
            {
                if(progressHandler.HasEmptyItems())
                    return true;
            }
        
            return false;
        }

        public void Revert()
        {
            _mapsProgressHandlers.Clear();
        }
    }
}
