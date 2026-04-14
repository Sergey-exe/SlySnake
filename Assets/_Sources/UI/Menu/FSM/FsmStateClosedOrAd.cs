using System;
using Assets.FSM;
using TMPro;
using UnityEngine;

public class FsmStateClosedOrAd : FsmState
{
    private TextMeshProUGUI _infoText;
    private GameObject _adMenu;
    private readonly string _currentText = "Открыть за рекламу";
    
    public FsmStateClosedOrAd(Fsm fsm, GameObject closedMenu, TextMeshProUGUI infoText) : base(fsm)
    {
        _adMenu = closedMenu ?? throw new ArgumentNullException(nameof(closedMenu));
        _infoText = infoText ?? throw new ArgumentNullException(nameof(infoText));
    }

    public override void Enter()
    {
        _adMenu.SetActive(true);
        _infoText.text = _currentText;
    }

    public override void Exit()
    {
        _adMenu.SetActive(false);
    }
}