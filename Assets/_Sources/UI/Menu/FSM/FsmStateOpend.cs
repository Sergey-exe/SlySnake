using System;
using Assets.FSM;
using TMPro;
using UnityEngine;

public class FsmStateOpend : FsmState
{
    private TextMeshProUGUI _infoText;
    private GameObject _openMenu;
    private readonly string _currentText = "Текущий уровень";
    
    public FsmStateOpend(Fsm fsm, GameObject openMenu, TextMeshProUGUI infoText) : base(fsm)
    {
        _openMenu = openMenu ?? throw new ArgumentNullException(nameof(openMenu));
        _infoText = infoText ?? throw new ArgumentNullException(nameof(infoText));
    }

    public override void Enter()
    {
        _openMenu.SetActive(true);
        _infoText.text = _currentText;
    }

    public override void Exit()
    {
        _openMenu.SetActive(false);
    }
}
