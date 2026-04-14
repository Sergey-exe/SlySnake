using System;
using Assets.FSM;
using TMPro;
using UnityEngine;

namespace _Sources.UI.Menu.FSM
{
    public class FsmStateRestart : FsmState
    {
        private GameObject _restartMenu;
        private TextMeshProUGUI _restartText;
        private string _restartMessage = "Лучшее время:";
    
        public FsmStateRestart(Fsm fsm, GameObject restartMenu, TextMeshProUGUI infoText) : base(fsm)
        {
            _restartMenu = restartMenu ?? throw new ArgumentNullException(nameof(restartMenu));
            _restartText = infoText ?? throw new ArgumentNullException(nameof(infoText));
        }

        public override void Enter()
        {
            _restartMenu.SetActive(true);
            _restartText.text = _restartMessage;
        }

        public override void Exit()
        {
        
        }
    }
}
