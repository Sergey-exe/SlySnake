using UnityEngine;

public class GameWineUI : MonoBehaviour
{
    [SerializeField] private GameObject _endWindow;
    
    public void ShowWine()
    {
        _endWindow.SetActive(true);
    }

    public void CloseWine()
    {
        _endWindow.SetActive(false);
    }
}
