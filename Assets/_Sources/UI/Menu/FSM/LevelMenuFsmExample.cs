using TMPro;
using UnityEngine;

namespace _Sources.UI.Menu.FSM
{
    public class LevelMenuFsmExample : MonoBehaviour
    {
        [SerializeField] private GameObject _closedMenu;
        [SerializeField] private GameObject[] _hiddenElements;
        [SerializeField] private GameObject _openMenu;
        [SerializeField] private GameObject _restartMenu;
        [SerializeField] private GameObject _adMenu;
        [SerializeField] private TextMeshProUGUI _infoText;
    
        private Fsm _fsm;

        public void Init()
        {
            _fsm = new LevelMenuFsm();
        
            _fsm.AddState(new FsmStateOpend(_fsm, _openMenu, _infoText));
            _fsm.AddState(new FsmStateClosed(_fsm,  _closedMenu, _hiddenElements));
            _fsm.AddState(new FsmStateRestart(_fsm, _restartMenu, _infoText));
            _fsm.AddState(new FsmStateClosedOrAd(_fsm, _adMenu, _infoText));
        
            _fsm.SetState<FsmStateClosed>();
        }
    
        public void ChangeState(LevelOpeningType levelOpeningType)
        {
            if(_fsm == null)
                return;
        
            switch (levelOpeningType)
            {
                case LevelOpeningType.Open:
                    _fsm.SetState<FsmStateOpend>();
                    break;
            
                case LevelOpeningType.Closed:
                    _fsm.SetState<FsmStateClosed>();
                    break;
            
                case LevelOpeningType.Restart:
                    _fsm.SetState<FsmStateRestart>();
                    break;
            
                case LevelOpeningType.ClosedOrAD:
                    _fsm.SetState<FsmStateClosedOrAd>();
                    break;
            }
        }
    }
}