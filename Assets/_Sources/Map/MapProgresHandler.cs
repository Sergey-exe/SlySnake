using _Sources.Map;

public class MapProgressHandler
{
    private Map _map;

    public MapProgressHandler(Map map)
    {
        _map = map;
    }
    
    public bool HasEmptyItems()
    {
        foreach (var item in _map.GetCurrentMap())
        {
            if(item == (int)MapItemType.Empty)
                return true;
        }
        
        return false;
    }
}