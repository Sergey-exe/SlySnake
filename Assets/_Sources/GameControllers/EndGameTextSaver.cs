using TMPro;
using UnityEngine;

public class EndGameTextSaver : MonoBehaviour
{
    private string _endText = "End Text(Demo)";
    
    public void SetText(string text)
    {
        _endText = text;
    }

    public string GetText()
    {
        return _endText;
    }
}
