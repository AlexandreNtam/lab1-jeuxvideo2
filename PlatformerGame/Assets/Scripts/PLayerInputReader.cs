using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputReader : MonoBehaviour
    {
        private InputAction moveAction;
        private InputAction jumpAction;

        private Vector2 moveValue;
        private bool jumpTriggered;
        private bool jumpHeld;

        public Vector2 MoveInput => moveValue;
        public bool JumpTriggered => jumpTriggered;
        public bool JumpHeld => jumpHeld;

        private void Awake()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            jumpAction = InputSystem.actions.FindAction("Jump");
        }

        private void Update()
        {
            if (moveAction != null) moveValue = moveAction.ReadValue<Vector2>();
            if (jumpAction != null && jumpAction.triggered) jumpTriggered = true;
            if (jumpAction != null) jumpHeld = jumpAction.IsPressed();
        }

        public void ResetJumpTrigger() => jumpTriggered = false;
    }
}