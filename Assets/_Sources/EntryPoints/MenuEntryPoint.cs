using _Sources.UI;
using _Sources.UI.Menu;
using UnityEngine;

public class MenuEntryPoint : MonoBehaviour
{
    [SerializeField] private StartLevelUI _startLevelUI;
    [SerializeField] private LevelMenu _levelMenu;
    
    public void Init()
    {
        _levelMenu.Init();
        _levelMenu.gameObject.SetActive(true);
    }
}
