using System.Collections;
using Cinemachine;
using IvyCreek.EmpireTycoonEngine.Input;
using UnityEngine;

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

        private Coroutine _zoomCoroutine;
        private Vector2 _primaryFingerPosition;
        private Vector2 _secondaryFingerPosition;


        private void OnEnable()
        {
            _touchInputReader.PrimaryFingerPosition += OnPrimaryFingerPosition;
            _touchInputReader.SecondaryFingerPosition += OnSecondaryFingerPosition;
            _touchInputReader.SecondaryTouchContact += OnSecondaryTouchContact;
        }

        private void OnDisable()
        {
            _touchInputReader.PrimaryFingerPosition -= OnPrimaryFingerPosition;
            _touchInputReader.SecondaryFingerPosition -= OnSecondaryFingerPosition;
            _touchInputReader.SecondaryTouchContact -= OnSecondaryTouchContact;
        }

        private void OnPrimaryFingerPosition(Vector2 pos)
        {
            _primaryFingerPosition = pos;
        }

        private void OnSecondaryFingerPosition(Vector2 pos)
        {
            _secondaryFingerPosition = pos;
        }

        private void OnSecondaryTouchContact(bool touchStarted)
        {
            if (touchStarted)
            {
                _zoomCoroutine = StartCoroutine(ZoomCamera());
            }
            else
            {
                StopCoroutine(_zoomCoroutine);
            }
        }

        private IEnumerator ZoomCamera()
        {
            float previousDistance = 0;
            var velocity = 0.0F;

            while (true)
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