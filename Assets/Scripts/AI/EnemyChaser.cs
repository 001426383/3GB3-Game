using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : MonoBehaviour {
    private UnityEngine.AI.NavMeshAgent chaserAgent;
    // Use this for initialization
    void Start ()
    {
        chaserAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update ()
    {
        chaserAgent.destination = GameObject.FindWithTag("Player").transform.position;
    }
}
