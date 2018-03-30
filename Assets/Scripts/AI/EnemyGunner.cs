using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunner : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent gunnerAgent;

    public float fireRate;
    private float fireTimer;
    public GameObject projectileObject;
    public float projectileSpeed;
    private float minDistance;

    // Use this for initialization
    void Start()
    {
        gunnerAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        minDistance = gunnerAgent.stoppingDistance;
        fireTimer = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        gunnerAgent.destination = playerObject.transform.position;
        float distance = Vector3.Distance(transform.position, playerObject.transform.position);
        if (distance < minDistance)
        {
            Vector3 direction = (playerObject.transform.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

            fireTimer -= Time.deltaTime;

            if (fireTimer < 0)
            {
                //shootAudioSource.PlayOneShot(shootingSound);
                
                Vector3 initPosition = playerObject.transform.position - gameObject.transform.position;
                Vector3 normalizedTarget = initPosition.normalized;
                float offset = 1.5f;
                initPosition = gameObject.transform.position + offset * normalizedTarget;
                initPosition.y = gameObject.transform.position.y;
                
                GameObject newProjectile = Instantiate(projectileObject, initPosition, Quaternion.identity) as GameObject;
                Rigidbody newRB = newProjectile.GetComponent<Rigidbody>();
                newProjectile.transform.parent = GameObject.Find("TemporaryEntities").transform; //Put all projectiles in temporary group
                                                                                                 //newRB.velocity = gameObject.GetComponent<Rigidbody>().velocity;
                newRB.AddForce(lookRotation * Vector3.forward * projectileSpeed, ForceMode.Impulse);
                fireTimer = fireRate;
            }
        }
    }
}
