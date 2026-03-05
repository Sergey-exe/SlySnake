using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanism : MonoBehaviour
{
    [SerializeField] EndGameTextSaver _endGameTextSaver;
    [SerializeField] private string _endText = "Активировал механизм проигрыша!";
    
    public bool IsActivate { get; private set; }
    
    public void Activate()
    {
        _endGameTextSaver.SetText(_endText);
        IsActivate = true;
    }
}
