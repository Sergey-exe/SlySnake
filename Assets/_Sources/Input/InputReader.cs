using System.Linq;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] private KeyCode[] _keysUp;
    [SerializeField] private KeyCode[] _keysDown;
    [SerializeField] private KeyCode[] _keysRight;
    [SerializeField] private KeyCode[] _keysLeft;
    
    [SerializeField] private PlayersMover _playersMover;
    
    private int _step = 1;
    private bool _isMobile;
    private bool _isActive;

    private void Update()
    {
        if(_isActive == false)
            return;

        if(DownButtonUp())
            _playersMover.TryStartMove(new GameMapVector2(-_step, 0));
        else if(DownButtonDown())
            _playersMover.TryStartMove(new GameMapVector2(_step, 0));
        else if(DownButtonRight())
            _playersMover.TryStartMove(new GameMapVector2(0, _step));
        else if(DownButtonLeft())
            _playersMover.TryStartMove(new GameMapVector2(0, -_step));
    }
    
    
    public void Activate()
    {
        _isActive = true;
    }
    
    public bool DownButtonUp()
    {
        return DownButton(_keysUp);
    }

    public bool DownButtonDown()
    {
        return DownButton(_keysDown);
    }

    public bool DownButtonRight()
    {
        return DownButton(_keysRight);
    }

    public bool DownButtonLeft()
    {
        return DownButton(_keysLeft);
    }
    
    private bool DownButton(KeyCode[] keyCodes)
    {
        return keyCodes.Any(keyCode => Input.GetKeyDown(keyCode));
    }
}
