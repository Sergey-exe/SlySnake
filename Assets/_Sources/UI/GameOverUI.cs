using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject _endWindow;
    [SerializeField] private MiniMapPainter _miniMapPainter;
    
    public void ShowLose()
    {
        _miniMapPainter.Paint();
        _endWindow.SetActive(true);
    }

    public void CloseLose()
    {
        _endWindow.SetActive(false);
    }
}