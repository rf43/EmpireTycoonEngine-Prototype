using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static TouchInput;

namespace IvyCreek.EmpireTycoonEngine.Input
{
    [CreateAssetMenu(fileName = "TouchInputReader", menuName = "EmpireTycoonEngine/Input/TouchInputReader")]
    public class TouchInputReader : ScriptableObject, IZoomActions, ITouchActions
    {
        public event UnityAction<Vector2> PrimaryFingerPosition = delegate { }; 
        public event UnityAction<Vector2> SecondaryFingerPosition = delegate { };
        public event UnityAction<bool> SecondaryTouchContact = delegate { };
        public event UnityAction<bool> TouchInput = delegate { };
        public event UnityAction<bool> TouchAndHold = delegate { };
        
        private TouchInput _touchInputActions;
        
        private void OnEnable()
        {
            if (_touchInputActions != null) return;
            _touchInputActions = new TouchInput();
            _touchInputActions.Zoom.SetCallbacks(this);
            _touchInputActions.Touch.SetCallbacks(this);
            _touchInputActions.Enable();
        }

        public void OnPrimaryFingerPosition(InputAction.CallbackContext context)
        {
            PrimaryFingerPosition.Invoke(context.ReadValue<Vector2>());
        }

        public void OnSecondaryFingerPosition(InputAction.CallbackContext context)
        {
            SecondaryFingerPosition.Invoke(context.ReadValue<Vector2>());
        }

        public void OnSecondaryTouchContact(InputAction.CallbackContext context)
        {
            SecondaryTouchContact.Invoke(context.ReadValueAsButton());
        }

        public void OnTouchInput(InputAction.CallbackContext context)
        {
            TouchInput.Invoke(context.ReadValueAsButton());
        }

        public void OnTouchAndHold(InputAction.CallbackContext context)
        {
            TouchAndHold.Invoke(context.ReadValueAsButton());
        }
    }
}