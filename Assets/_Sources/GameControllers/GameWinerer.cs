using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinerer : MonoBehaviour
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
