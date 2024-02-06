using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static TouchInput;

namespace IvyCreek.EmpireTycoonEngine.Input
{
    [CreateAssetMenu(fileName = "TouchInputReader", menuName = "EmpireTycoonEngine/Input/TouchInputReader")]
    public class TouchInputReader : ScriptableObject, ITouchScreenGamePlayActions
    {
        public event UnityAction<Vector2> PrimaryFingerPosition; 
        public event UnityAction<Vector2> SecondaryFingerPosition;
        public event UnityAction<bool> SecondaryTouchContact;
        public event UnityAction<InputAction> PrimaryFingerTap;
        
        private TouchInput _touchInputActions;
        
        private void OnEnable()
        {
            if (_touchInputActions != null) return;
            _touchInputActions = new TouchInput();
            _touchInputActions.Enable();
            _touchInputActions.TouchScreenGamePlay.SetCallbacks(this);
        }
        
        private void OnDisable()
        {
            _touchInputActions.TouchScreenGamePlay.RemoveCallbacks(this);
            _touchInputActions.Disable();
        }

        public void OnPrimaryFingerPosition(InputAction.CallbackContext context)
        {
            PrimaryFingerPosition?.Invoke(context.ReadValue<Vector2>());
        }
        
        public void OnSecondaryFingerPosition(InputAction.CallbackContext context)
        {
            SecondaryFingerPosition?.Invoke(context.ReadValue<Vector2>());
        }
        
        public void OnSecondaryTouchContact(InputAction.CallbackContext context)
        {
            SecondaryTouchContact?.Invoke(context.ReadValueAsButton());
        }

        public void OnPrimaryFingerTap(InputAction.CallbackContext context)
        {
            PrimaryFingerTap?.Invoke(context.action);
        }
    }
}