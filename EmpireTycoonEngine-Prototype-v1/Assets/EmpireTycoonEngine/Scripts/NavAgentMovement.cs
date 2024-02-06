using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace IvyCreek.EmpireTycoonEngine.Scripts
{
    [AddComponentMenu(menuName: "EmpireTycoonEngine/Scripts/NavAgentMovement")]
    public class NavAgentMovement : MonoBehaviour
    {
        [SerializeField] private TargetPositioner _target;
        [SerializeField] private float _walkSpeed = 1.5f;
        [SerializeField] private float _runSpeed = 1.5f;
        [SerializeField] private float _minDistance = 2f;
        [SerializeField] private float _runDistance = 10f;
        [SerializeField, Range(1, 10)] private int _waitForMinSeconds;
        [SerializeField, Range(1, 10)] private int _waitForMaxSeconds;

        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        
        static readonly int Speed = Animator.StringToHash("Speed");
        
        public static event UnityAction OnTargetReached = delegate { }; 

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            StartCoroutine(TargetWanderAround());
        }

        private IEnumerator TargetWanderAround()
        {
            while (true)
            {
                if (_navMeshAgent.remainingDistance < _minDistance)
                {
                    Debug.Log($"Distance => {_navMeshAgent.remainingDistance}");
                    Debug.Log("IDLE");
                    _animator.SetFloat(Speed, 0);
                    OnTargetReached?.Invoke();
                    yield return new WaitForSeconds(Random.Range(_waitForMinSeconds, _waitForMaxSeconds));
                    _navMeshAgent.SetDestination(_target.transform.position);
                    yield return null;
                }
                else if(_navMeshAgent.remainingDistance > _runDistance)
                {
                    Debug.Log($"Distance => {_navMeshAgent.remainingDistance}");
                    Debug.Log("RUNNING");
                    _navMeshAgent.speed = _runSpeed;
                    _animator.SetFloat(Speed, 1);
                    yield return null;
                }
                else
                {
                    Debug.Log($"Distance => {_navMeshAgent.remainingDistance}");
                    Debug.Log("WALKING");
                    _navMeshAgent.speed = _walkSpeed;
                    _animator.SetFloat(Speed, 0.5f);
                    yield return null;
                }
                
                yield return null;
            }
        }
    }
}