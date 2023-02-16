using UnityEngine;
using UnityEngine.InputSystem;

namespace Arcade3D
{
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Properties
        public Vector2 MovementInput { get; private set; }
        public bool DashInput { get; private set; }
        public bool JumpInput { get; private set; }

        #endregion

        #region Public API
        public void GetMovementInput(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void GetDashInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                DashInput = true;
                print("dashed");
            }
            if (context.canceled)
                DashInput = false;
        }

        public void GetJumpInput(InputAction.CallbackContext context)
        {
            if (context.performed)
                JumpInput = true;
            if (context.canceled)
                JumpInput = false;
        }

        public void GetReturnToMenuInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                UIManager.Instance.QuitGame();
            }
        }

        #endregion
    }
}
