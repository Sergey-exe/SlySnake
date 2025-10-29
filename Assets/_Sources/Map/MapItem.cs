using System;
using _Sources.Map;
using UnityEngine;

namespace _Sources.Map
{
    public class MapItem : MonoBehaviour
    {
        [field: SerializeField] public MapItemType MapItemType { get; private set;}
        
        public PositionInMap Position { get; private set; }

        public void SetType(MapItemType mapItemType)
        {
            MapItemType = mapItemType;
        }

        public void SetPositionInMap(PositionInMap positionInMap)
        {
            Position = positionInMap ?? throw new ArgumentNullException(nameof(positionInMap));
        }
    }
}

public class PositionInMap
{
    public int X { get;  set; }
    
    public int Y { get;  set; }
}