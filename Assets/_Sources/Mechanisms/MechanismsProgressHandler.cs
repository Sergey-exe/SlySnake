using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MechanismsProgressHandler : MonoBehaviour
{
    [SerializeField] private List<Mechanism> _mechanisms;

    public bool IsCorrectActiveOll()
    {
        foreach (var mechanism in _mechanisms)
        {
            if(mechanism.IsActivate)
                return false;
        }
        
        return true;
    }
}
