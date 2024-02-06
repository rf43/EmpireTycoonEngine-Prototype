using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IvyCreek.EmpireTycoonEngine.Scripts
{
    public class TargetPositioner : MonoBehaviour
    {
        private void OnEnable()
        {
            NavAgentMovement.OnTargetReached += OnTargetReached;
        }
        
        private void OnDisable()
        {
            NavAgentMovement.OnTargetReached -= OnTargetReached;
        }

        private void OnTargetReached()
        {
            StartCoroutine(SetRandomPosition());
        }
        
        private IEnumerator SetRandomPosition()
        {
            yield return new WaitForSeconds(3f);
            var randomPosition = new Vector3(Random.Range(-30, 7), 0, Random.Range(-11, 11));
            transform.position = randomPosition;
            yield return new WaitForSeconds(5);
        }
    }
}