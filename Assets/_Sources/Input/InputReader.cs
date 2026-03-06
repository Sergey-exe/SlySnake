using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [SerializeField] private PlayersMover _playersMover;
    
    private PlayerInput _playerInput;
    
    private bool _isMobile;
    
    public void Init()
    {
        _playerInput = new PlayerInput();
        _playerInput.Player.Move.performed += OnMove;
    }
    
    public void Activate()
    {
        _playerInput.Enable();
    }

    public void Deactivate()
    {
        _playerInput.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveDirection = context.action.ReadValue<Vector2>();
        _playersMover.TryStartMove(new GameMapVector2(-(int)moveDirection.y, (int)moveDirection.x));
    }
}
