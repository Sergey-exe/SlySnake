using System;
using _Sources.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Sources.Input
{
    public class InputReader : MonoBehaviour
    {
        private PlayerInput _playerInput;
        
        public event Action<GameMapVector2> OnMove;

        public void Init()
        {
            _playerInput = new PlayerInput();
            
            _playerInput.Player.Move.performed += OnMovePerformed;
        }

        public void Activate()
        {
            _playerInput.Enable();
        }

        public void Deactivate()
        {
            _playerInput.Disable();
        }
        
        public void SendMoveCommand(GameMapVector2 direction)
        {
            OnMove?.Invoke(direction);
        }
        
        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            Vector2 moveDirection = context.action.ReadValue<Vector2>();
            
            GameMapVector2 gridDirection = new GameMapVector2(
                -(int)moveDirection.y,
                (int)moveDirection.x
            );
            
            OnMove?.Invoke(gridDirection);
        }

        private void OnDestroy()
        {
            if (_playerInput != null)
            {
                _playerInput.Player.Move.performed -= OnMovePerformed;
                _playerInput.Dispose();
            }
        }
    }
}