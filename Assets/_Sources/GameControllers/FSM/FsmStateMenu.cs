using System;
using System.Collections;
using System.Collections.Generic;
using Assets.FSM;
using UnityEngine;

public class FsmStateMenu : FsmState
{
    private GameObject _menuUI;
    
    public FsmStateMenu(GameObject menuUI, Fsm fsm) : base(fsm)
    {
        _menuUI = menuUI ?? throw new ArgumentNullException(nameof(menuUI));
    }

    public override void Enter()
    {
        _menuUI.SetActive(true);
    }

    public override void Exit()
    {
        _menuUI.SetActive(false);
    }
}
