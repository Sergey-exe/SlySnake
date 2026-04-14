using System;
using _Sources.Input;
using _Sources.TimeManagement;
using Assets.FSM;
using UnityEngine;

namespace _Sources.GameControllers.FSM
{
    public class FsmStateGame : FsmState
    {
        private GameObject _gameWindow;
        private InputReader _inputReader;
        private LevelTimeCounter _levelTimeCounter;

        public FsmStateGame(GameObject gameWindow, InputReader inputReader, LevelTimeCounter levelTimeCounter, Fsm fsm) : base(fsm)
        {
            _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
            _levelTimeCounter = levelTimeCounter ?? throw new ArgumentNullException(nameof(levelTimeCounter));
            _gameWindow = gameWindow ?? throw new ArgumentNullException(nameof(gameWindow));
        }

        public override void Enter()
        {
            _inputReader.Activate();
            _levelTimeCounter.StartCounting();
            _gameWindow.SetActive(true);
        }

        public override void Exit()
        {
            _inputReader.Deactivate();
            _levelTimeCounter.StopCounting();
            _gameWindow.SetActive(false);
        }
    }
}