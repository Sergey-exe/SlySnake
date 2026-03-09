using UnityEngine;

public class MenuEntryPoint : MonoBehaviour
{
    [SerializeField] private StartLevelUI _startLevelUI;
    
    public void Init()
    {
        _startLevelUI.OpenMenu();
    }
}
