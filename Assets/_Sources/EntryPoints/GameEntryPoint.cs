using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private MenuEntryPoint _menuEntryPoint;
    [SerializeField] private LevelEntryPoint _levelEntryPoint;

    private void Start()
    {
        _menuEntryPoint.Init();
        _levelEntryPoint.Init();
    }
}
