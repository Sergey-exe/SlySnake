using System;
using Assets.FSM;
using UnityEngine;
using YG;

namespace _Sources.UI.Menu.FSM
{
    public class FsmStateRestart : FsmState
    {
        private GameObject _restartMenu;
    
        public FsmStateRestart(Fsm fsm, GameObject restartMenu) : base(fsm)
        {
            _restartMenu = restartMenu ?? throw new ArgumentNullException(nameof(restartMenu));
        }

        public override void Enter()
        {
            _restartMenu.SetActive(true);
        }

        public override void Exit()
        {
            _restartMenu.SetActive(false);
        }
    }
}