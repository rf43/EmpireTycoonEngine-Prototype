using System;
using System.Collections;
using Cinemachine;
using IvyCreek.EmpireTycoonEngine.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IvyCreek.EmpireTycoonEngine.Scripts
{
    [AddComponentMenu(menuName: "EmpireTycoonEngine/Scripts/CameraManager")]
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private TouchInputReader _touchInputReader;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _zoomSpeed = 1f;
        [SerializeField] private int _minZoomLevel = 10;
        [SerializeField] private int _maxZoomLevel = 20;

        [SerializeField,
         Tooltip("Adjusts how smooth the transition is. " +
                 "A higher value will result in a slower but smoother transition.")]
        private float _smoothTime = 0.3f;

        private bool _isZooming;
        private Coroutine _zoomCoroutine;

        private bool _isTrackingPrimaryFingerPosition;
        private Coroutine _trackPrimaryFingerPositionCoroutine;

        private Vector2 _primaryFingerPosition;
        private Vector2 _secondaryFingerPosition;


        private void OnEnable()
        {
            _touchInputReader.PrimaryFingerPosition += OnPrimaryFingerPosition;
            _touchInputReader.SecondaryFingerPosition += OnSecondaryFingerPosition;
            _touchInputReader.SecondaryTouchContact += OnSecondaryTouchContact;
            _touchInputReader.PrimaryFingerTap += OnPrimaryFingerTap;
        }

        private void OnDisable()
        {
            _touchInputReader.PrimaryFingerPosition -= OnPrimaryFingerPosition;
            _touchInputReader.SecondaryFingerPosition -= OnSecondaryFingerPosition;
            _touchInputReader.SecondaryTouchContact -= OnSecondaryTouchContact;
            _touchInputReader.PrimaryFingerTap -= OnPrimaryFingerTap;
        }

        private void OnPrimaryFingerPosition(Vector2 pos)
        {
            _primaryFingerPosition = pos;
        }

        private void OnSecondaryFingerPosition(Vector2 pos)
        {
            _secondaryFingerPosition = pos;
        }

        private void OnSecondaryTouchContact(bool started)
        {
            if (started)
            {
                _isZooming = true;
                _zoomCoroutine = StartCoroutine(ZoomCamera());
            }
            else
            {
                _isZooming = false;
                StopCoroutine(_zoomCoroutine);
            }
        }
        
        private void OnPrimaryFingerTap(InputAction action)
        {
            switch (action.phase)
            {
                case InputActionPhase.Performed:
                    if (!_isTrackingPrimaryFingerPosition)
                    {
                        _isTrackingPrimaryFingerPosition = true;
                        _trackPrimaryFingerPositionCoroutine = StartCoroutine(TrackPrimaryFingerPosition());
                    }
                    break;
                case InputActionPhase.Canceled:
                    _isTrackingPrimaryFingerPosition = false;
                    StopCoroutine(_trackPrimaryFingerPositionCoroutine);
                    break;
                
                // Not needed at this time
                case InputActionPhase.Started:
                case InputActionPhase.Waiting:
                case InputActionPhase.Disabled:
                default:
                    break;
            }
        }
        
        private IEnumerator TrackPrimaryFingerPosition()
        {
            while (_isTrackingPrimaryFingerPosition)
            {
                Debug.Log("RF43: Tracking primary finger position");
                Debug.Log("RF43: Primary finger position: " + _primaryFingerPosition);
                yield return new WaitForSeconds(1f);
            }
        }

        private IEnumerator ZoomCamera()
        {
            float previousDistance = 0;
            var velocity = 0.0F;

            while (_isZooming)
            {
                var distance = Vector2.Distance(_primaryFingerPosition, _secondaryFingerPosition);

                if (distance > previousDistance)
                {
                    var oSize = _camera.m_Lens.OrthographicSize - _zoomSpeed * Time.deltaTime;
                    SetOrthographicSize(oSize, ref velocity);
                }
                else if (distance < previousDistance)
                {
                    var oSize = _camera.m_Lens.OrthographicSize + _zoomSpeed * Time.deltaTime;
                    SetOrthographicSize(oSize, ref velocity);
                }

                previousDistance = distance;
                yield return null;
            }
        }

        private void SetOrthographicSize(float oSize, ref float velocity)
        {
            var targetZoom = Mathf.Clamp(oSize, _minZoomLevel, _maxZoomLevel);
            _camera.m_Lens.OrthographicSize =
                Mathf.SmoothDamp(_camera.m_Lens.OrthographicSize, targetZoom, ref velocity, _smoothTime);
        }
    }
}