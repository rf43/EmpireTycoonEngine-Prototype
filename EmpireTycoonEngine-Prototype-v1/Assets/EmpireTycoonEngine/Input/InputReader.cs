using UnityEngine;
using UnityEngine.InputSystem;

namespace IvyCreek.EmpireTycoonEngine.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "EmpireTycoonEngine/Input/InputReader")]
    public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
    {
        public void OnMove(InputAction.CallbackContext context)
        {
            // no-op
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            // no-op
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            // no-op
        }

        public void OnMouseControlCamera(InputAction.CallbackContext context)
        {
            // no-op
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            // no-op
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            // no-op
        }
    }
}
