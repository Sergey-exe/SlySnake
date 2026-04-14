using System;
using Assets.FSM;
using UnityEngine;

public class FsmStateClosed : FsmState
{
    private GameObject _closedMenu;
    private GameObject[] _hiddenElements;
    
    public FsmStateClosed(Fsm fsm, GameObject closedMenu, GameObject[] hiddenElements) : base(fsm)
    {
        _closedMenu = closedMenu ?? throw new ArgumentNullException(nameof(closedMenu));
        _hiddenElements = hiddenElements;
    }

    public override void Enter()
    {
        _closedMenu.SetActive(true);
        
        if(_hiddenElements == null)
            return;
        
        foreach(var element in _hiddenElements)
            element.SetActive(false);
    }

    public override void Exit()
    {
        _closedMenu.SetActive(false);
        
        if(_hiddenElements == null)
            return;
        
        foreach(var element in _hiddenElements)
            element.SetActive(true);
    }
}