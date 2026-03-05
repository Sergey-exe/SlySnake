using TMPro;
using UnityEngine;

public class LevelTimeViewer : MonoBehaviour
{
    private const float SecondsInMinutes = 60;
    
    [SerializeField] private TextMeshProUGUI[] _timeTexts;
    
    private bool _iaActivate;
    
    public void Activate()
    {
        _iaActivate = true;
    }
    
    public void ShowTime(float currentTimeSeconds)
    {
        float minutes = Mathf.Floor(currentTimeSeconds / SecondsInMinutes);
        float seconds = currentTimeSeconds % SecondsInMinutes;
        
        string formattedTime = $"{minutes:0}.{seconds:0}";
        
        foreach (var text in _timeTexts)
        {
            text.text = formattedTime;
        }
    }
}