using System.Collections;
using Cinemachine;
using IvyCreek.Tools.Core.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EmpireTycoonEngine.Scripts.Core
{
    [DefaultExecutionOrder(-1000)]
    [AddComponentMenu(menuName: "EmpireTycoonEngine/Scripts/CameraManager")]
    public class CameraManager : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private TouchInputReader _touchInputReader;

        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        [Header("Zoom Settings")] [SerializeField]
        private float _zoomSpeed = 1f;

        [SerializeField] private int _minZoomLevel = 10;
        [SerializeField] private int _maxZoomLevel = 20;

        [SerializeField,
         Tooltip("Adjusts how smooth the transition is. " +
                 "A higher value will result in a slower but smoother transition.")]
        private float _smoothZoomTime = 0.3f;

        [Header("Pan Settings")] [SerializeField]
        private float _panSpeed = 1f;

        private bool _isZooming;
        private Coroutine _zoomCoroutine;

        private bool _isPanningCamera;
        private Vector3 _panTouchStart;
        private Coroutine _panCoroutine;

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
                StartZoomingCamera();
            }
            else
            {
                StopZoomingCamera();
            }
        }

        private void OnPrimaryFingerTap(InputAction action)
        {
            switch (action.phase)
            {
                case InputActionPhase.Started:
                    // We will assume that we are panning the camera when the primary finger is pressed
                    // and let the zoom functionality take over when the secondary finger is pressed
                    _panTouchStart = GetWorldPoint(_primaryFingerPosition);
                    break;
                case InputActionPhase.Performed:
                    StartPanningCamera();
                    break;
                case InputActionPhase.Canceled:
                    StopPanningCamera();
                    break;

                // Not needed at this time
                case InputActionPhase.Waiting:
                case InputActionPhase.Disabled:
                default:
                    break;
            }
        }

        private IEnumerator PanCameraCoroutine()
        {
            while (_isPanningCamera)
            {
                Debug.Log("RF43: Panning camera");
                yield return null;
            }
        }

        private IEnumerator ZoomCameraCoroutine()
        {
            float previousDistance = 0;
            var velocity = 0.0F;

            while (_isZooming)
            {
                var distance = Vector2.Distance(_primaryFingerPosition, _secondaryFingerPosition);

                if (distance > previousDistance)
                {
                    var oSize = _virtualCamera.m_Lens.OrthographicSize - _zoomSpeed * Time.deltaTime;
                    SetOrthographicSize(oSize, ref velocity);
                }
                else if (distance < previousDistance)
                {
                    var oSize = _virtualCamera.m_Lens.OrthographicSize + _zoomSpeed * Time.deltaTime;
                    SetOrthographicSize(oSize, ref velocity);
                }

                previousDistance = distance;
                yield return null;
            }
        }
        
        private void StartPanningCamera()
        {
            if (_isPanningCamera || _isZooming || _panCoroutine != null) return;
            _isPanningCamera = true;
            _panCoroutine = StartCoroutine(PanCameraCoroutine());
        }
        
        private void StopPanningCamera()
        {
            if (!_isPanningCamera) return;
            _isPanningCamera = false;
            StopCoroutine(_panCoroutine);
            _panCoroutine = null;
            _panTouchStart = Vector3.zero;
        }
        
        private void StartZoomingCamera()
        {
            if (_isZooming) return;
            StopPanningCamera();
            _isZooming = true;
            _zoomCoroutine = StartCoroutine(ZoomCameraCoroutine());
        }
        
        private void StopZoomingCamera()
        {
            if (!_isZooming) return;
            _isZooming = false;
            StopCoroutine(_zoomCoroutine);
            _zoomCoroutine = null;
        }

        private void SetOrthographicSize(float oSize, ref float velocity)
        {
            var targetZoom = Mathf.Clamp(oSize, _minZoomLevel, _maxZoomLevel);
            _virtualCamera.m_Lens.OrthographicSize =
                Mathf.SmoothDamp(_virtualCamera.m_Lens.OrthographicSize, targetZoom, ref velocity, _smoothZoomTime);
        }
        
        private Vector3 GetWorldPoint(Vector2 screenPoint)
        {
            Ray ray = _virtualCamera.VirtualCameraGameObject.GetComponent<Camera>().ScreenPointToRay(screenPoint);
            new Plane(Vector3.up, Vector3.zero).Raycast(ray, out var dist);
            return ray.GetPoint(dist);
        }
    }
}