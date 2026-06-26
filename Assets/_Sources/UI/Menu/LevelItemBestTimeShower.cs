using TMPro;
using UnityEngine;

public class LevelItemBestTimeShower : MonoBehaviour
{
    private const float SecondsInMinutes = 60;
    
    [SerializeField] private TextMeshProUGUI _text;
    
    private string _message = "Лучшее время: ";

    public void SetBestTime(float currentTimeSeconds)
    {
        float minutes = Mathf.Floor(currentTimeSeconds / SecondsInMinutes);
        float seconds = currentTimeSeconds % SecondsInMinutes;

        string formattedTime = $"{minutes:0}.{seconds:0}";
        
        _text.text = _message + formattedTime;
    }
}