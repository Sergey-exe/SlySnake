
public class MapData 
{
    private int[,] _currentMap;

    public void SetCurrentMap(int[,] map)
    {
        _currentMap = map.Clone() as int[,];
    }

    public int[,] GetCurrentMap()
    {
        return _currentMap.Clone() as int[,];
    }
}
