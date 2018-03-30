using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent chaserAgent;


    void Start()
    {
        chaserAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
    
    void Update()
    {
        chaserAgent.destination = GameObject.FindWithTag("Player").transform.position;
    }
}
