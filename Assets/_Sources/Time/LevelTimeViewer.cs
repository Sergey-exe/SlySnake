using TMPro;
using UnityEngine;

public class LevelTimeViewer : MonoBehaviour
{
    private const float SecondsInMinutes = 60;
    
    [SerializeField] private TextMeshProUGUI[] _timeTexts;
    [SerializeField] private GameObject[] _timers;
    
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

    public void ShowTimers()
    {
        foreach (var timer in _timers)
            timer.SetActive(true);
    }

    public void HideTimers()
    {
        foreach (var timer in _timers)
            timer.SetActive(false);
    }
}