using System;
using Assets.FSM;
using UnityEngine;

namespace _Sources.GameControllers.FSM
{
    public class FsmStatePause : FsmState
    {
        private GameObject _pauseWindow;
        
        public FsmStatePause(GameObject pauseWindow, Fsm fsm) : base(fsm)
        {
            _pauseWindow = pauseWindow  ?? throw new ArgumentNullException(nameof(pauseWindow));
        }

        public override void Enter()
        {
            _pauseWindow.SetActive(true);
            Time.timeScale = 0;
        }

        public override void Exit()
        {
            _pauseWindow.SetActive(false);
            Time.timeScale = 1;
        }
    }
}