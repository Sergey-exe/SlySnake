using _Sources.UI;
using Assets.FSM;
using UnityEngine;

public class FsmStateGameWin : FsmState
{
    private GameObject _gameWineUI;
    
    public FsmStateGameWin(GameObject gameWineUI, Fsm fsm) : base(fsm)
    {
        _gameWineUI = gameWineUI;
    }

    public override void Enter()
    {
        Debug.Log("Победа!"); 
        _gameWineUI.SetActive(true);
    }

    public override void Exit()
    {
        _gameWineUI.SetActive(false);
    }
}