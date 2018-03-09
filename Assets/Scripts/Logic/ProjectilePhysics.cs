using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePhysics : MonoBehaviour {

    public float projectileDamage;
    public float projectileSpeed;
    public float projectileTimeOut;
    public float projectileAcceleration;

    private Rigidbody pRigidbody;
    Vector3 direction;


    public AudioSource collisionAudioSource;
    public AudioClip particleEnvironmentSound;
    

    void Start () {
        //gameObject.timeoutDestructor = projectileTimeOut;
        pRigidbody = GetComponent<Rigidbody>();
    }
	

	void Update () {
        projectileTimeOut -= Time.deltaTime;
        if (projectileTimeOut < 0)
        {
            Destroy(gameObject);
        }
        else
            Move();
    }

    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject.transform.parent.gameObject);
            collisionAudioSource.PlayOneShot(particleEnvironmentSound);
            Destroy(gameObject);

            GameObject manager = GameObject.Find("GameManager");
            GameManager managerScript = manager.GetComponent<GameManager>();
            managerScript.score += 100;
            /*
            if (collision.gameObject.name == "ChaserEnemy")
                managerScript.score += 100;
            else if (collision.gameObject.name == "FastChaserEnemy")
                managerScript.score += 300;
            */

        }

        //if (collision.gameObject.tag == "Environment")
        //Debug.Log(gameObject.name + " has collided with " + collision.gameObject.name);
    }
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject.name + " was collided with " + other.gameObject.name);
    }

    void Move()
    {


        //gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        direction = Vector3.Normalize(pRigidbody.velocity);
        //pRigidbody.AddForce(direction * projectileSpeed * projectileAcceleration);

        pRigidbody.velocity = direction * projectileSpeed;

        /*
        float maxVelocity = projectileSpeed;
        float maxVelocitySqr = maxVelocity * maxVelocity;
        Vector3 rbVelocity = pRigidbody.velocity;
        if (rbVelocity.sqrMagnitude > maxVelocitySqr)
        {
            pRigidbody.velocity = rbVelocity.normalized * maxVelocity; //Limits max velocity
        }
        */
    }
}
