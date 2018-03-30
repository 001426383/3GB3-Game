using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlocker : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent blockerAgent;
    private float minDistance;
    private float turnSpeed;
    
    void Start()
    {
        blockerAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        minDistance = blockerAgent.stoppingDistance;
        turnSpeed = 1f;
    }
    
    void Update()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        blockerAgent.destination = playerObject.transform.position;
        float distance = Vector3.Distance(transform.position, playerObject.transform.position);
        if (distance < minDistance)
        {

            Vector3 direction = (playerObject.transform.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        }
    }
}
