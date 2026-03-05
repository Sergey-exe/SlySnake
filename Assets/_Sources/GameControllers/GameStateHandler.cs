using System;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    [SerializeField] private PlayersMover _playersMover;
    [SerializeField] private MapsProgressCollection _mapsProgressCollection;
    [SerializeField] private PlayersWayBuilder _playersWayBuilder;
    [SerializeField] private MechanismsProgressHandler _mechanismsProgressHandler;

    public event Action<bool> IsVin; 
    
    private void OnEnable()
    {
        _playersMover.PlayersFinished += TryInvokeState;
    }

    private void OnDisable()
    {
        _playersMover.PlayersFinished -= TryInvokeState;
    }

    private void TryInvokeState()
    {
        bool isVin = true;
        
        if (_mechanismsProgressHandler.IsCorrectActiveOll())
        {
            if(_playersWayBuilder.HasFreeWays())
                return;
            
            if(_mapsProgressCollection.HasEmptyItems())
                isVin = false;
        }
        else
        {
            isVin = false;
        }
        
        IsVin?.Invoke(isVin);
    }
}
