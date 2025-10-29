using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsData : MonoBehaviour
{
    [SerializeField] private List<Level> _levels;

    public Level GetCurrentLevel()
    {
        return _levels[0];
    }
}
