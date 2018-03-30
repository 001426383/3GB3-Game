using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    public float fireRate;
    private float fireTimer;
    public GameObject projectileObject;
    public float projectileSpeed;
    public float minDistance;

    // Use this for initialization
    void Start()
    {
        fireTimer = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
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
                float offset = 3f;
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
