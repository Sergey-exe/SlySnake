using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
    [SerializeField] private PlayersMover _playersMover;
    [SerializeField] private PlayersWayBuilder _wayBuilder;

    private void OnEnable()
    {
        _playersMover.PlayersFinished += TryChangeState;
    }

    private void OnDisable()
    {
        _playersMover.PlayersFinished -= TryChangeState;
    }

    private void TryChangeState()
    {
        if(_wayBuilder.HasFreeWays() == true)
            return;
        
        //if()
    }
}
