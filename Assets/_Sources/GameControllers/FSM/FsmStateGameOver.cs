using System;
using _Sources.UI;
using Assets.FSM;
using UnityEngine;

public class FsmStateGameOver : FsmState
{
    private GameObject _endWindow;
    private MiniMapPainter _miniMapPainter;
    
    public FsmStateGameOver(GameObject endWindow, MiniMapPainter miniMapPainter, Fsm fsm) : base(fsm)
    {
        _endWindow = endWindow ?? throw new ArgumentNullException(nameof(endWindow));
        _miniMapPainter = miniMapPainter  ?? throw new ArgumentNullException(nameof(miniMapPainter));
    }
    
    public override void Enter()
    {
        _miniMapPainter.Paint();
        _endWindow.SetActive(true);
    }

    public override void Exit()
    {
        _endWindow.SetActive(false);
    }
}