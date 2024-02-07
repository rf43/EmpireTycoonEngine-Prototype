using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace EmpireTycoonEngine.Scripts.Core
{
    [AddComponentMenu(menuName: "EmpireTycoonEngine/Scripts/NavAgentMovement")]
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class NavAgentMovement : MonoBehaviour
    {
        private const float XAxisMinRange = -10;
        private const float XAxisMaxRange = 10f;
        private const float ZAxisMinRange = -20f;
        private const float ZAxisMaxRange = 25f;

        [SerializeField] private float _walkSpeed = 1.5f;
        [SerializeField] private float _runSpeed = 1.5f;
        [SerializeField] private float _minDistance = 2f;
        [SerializeField] private float _runDistance = 10f;
        [SerializeField, Range(0, 10)] private int _waitForMinSeconds;
        [SerializeField, Range(1, 10)] private int _waitForMaxSeconds;

        private NavMeshAgent _navMeshAgent;
        private Animator _animator;

        private static readonly int Speed = Animator.StringToHash("Speed");


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
                    var animationSpeed = Random.Range(0f, 0.3f);
                    var walkSpeed = animationSpeed < 0.02f ? 0 : Random.Range(0f, 0.1f);
                    SetAgentSpeedAnimation(walkSpeed, animationSpeed);

                    var randomPosition = GenerateRandomPosition();
                    yield return new WaitForSeconds(Random.Range(_waitForMinSeconds, _waitForMaxSeconds));

                    _navMeshAgent.SetDestination(randomPosition);
                    yield return null;
                }
                else if (_navMeshAgent.remainingDistance > _runDistance)
                {
                    SetAgentSpeedAnimation(_runSpeed, 1f);
                    yield return null;
                }
                else
                {
                    SetAgentSpeedAnimation(_walkSpeed, 0.5f);
                    yield return null;
                }

                yield return null;
            }
        }

        private void SetAgentSpeedAnimation(float speed, float animationSpeed)
        {
            _navMeshAgent.speed = speed;
            _animator.SetFloat(Speed, animationSpeed);
        }

        private static Vector3 GenerateRandomPosition()
        {
            return new Vector3(Random.Range(XAxisMinRange, XAxisMaxRange), 0,
                Random.Range(ZAxisMinRange, ZAxisMaxRange));
        }
    }
}