using System.Collections;
using System.Collections.Generic;
using _Sources.Mechanisms;
using UnityEngine;

public class MechanismsActivator : MonoBehaviour
{
    [SerializeField] private TrapActivator _trapActivator;

    public void ActivateTrap()
    {
        _trapActivator.Activate();
    }
}
