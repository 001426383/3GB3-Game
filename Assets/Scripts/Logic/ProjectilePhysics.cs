using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectilePhysics : MonoBehaviour
{
    public float projectileDamage;
    public float projectileSpeed;
    public float projectileMinTimeOut;
    public float projectileMaxTimeOut;
    public float projectileAcceleration;

    private Rigidbody pRigidbody;
    Vector3 direction;


    public AudioSource collisionAudioSource;
    public AudioClip particleEnvironmentSound;

    bool isColliding;
    void Start()
    {
        //gameObject.timeoutDestructor = projectileTimeOut;
        pRigidbody = GetComponent<Rigidbody>();
        projectileMaxTimeOut = Random.Range(projectileMinTimeOut, projectileMaxTimeOut);
    }


    void Update()
    {
        projectileMaxTimeOut -= Time.deltaTime;
        if (projectileMaxTimeOut < 0)
        {
            Destroy(gameObject);
        }
        else
            Move();
        isColliding = false;
    }

    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (isColliding) return;
        isColliding = true;
        //Debug.Log(gameObject.name + " has collided with " + collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy")
        {
            LoseEnergy(0.5f);

            GameObject enemyEntity = collision.gameObject.transform.parent.gameObject;

            EnemyProperties propertiesScript = enemyEntity.GetComponent<EnemyProperties>();
            if (collision.gameObject.name == "BlockerBody")
                propertiesScript = enemyEntity.transform.parent.gameObject.transform.GetComponent<EnemyProperties>();
            propertiesScript.health -= projectileDamage;
            if (propertiesScript.health <= 0)
            {
                GameObject spawnSource = enemyEntity.transform.parent.gameObject;
                if (collision.gameObject.name == "BlockerBody")
                    spawnSource = enemyEntity.transform.parent.gameObject.transform.parent.gameObject;
                EnemySpawner spawnScript = spawnSource.GetComponent<EnemySpawner>();
                spawnScript.currentAmount -= 1;
                Destroy(enemyEntity);

                GameObject manager = GameObject.Find("GameManager");
                GameManager managerScript = manager.GetComponent<GameManager>();
                managerScript.score += 100;

                collisionAudioSource.PlayOneShot(particleEnvironmentSound);
            }
            //Destroy(gameObject);

            /*
            if (collision.gameObject.name == "ChaserEnemy")
                managerScript.score += 100;
            else if (collision.gameObject.name == "FastChaserEnemy")
                managerScript.score += 300;
            */
        }
        else if (collision.gameObject.tag == "Turret")
        {
            LoseEnergy(0.3f);
            GameObject turretEntity = collision.gameObject.transform.parent.gameObject;
            EnemyProperties propertiesScript = turretEntity.GetComponent<EnemyProperties>();
            propertiesScript.health -= projectileDamage;
            if (propertiesScript.health <= 0)
            {
                Destroy(turretEntity);

                GameObject manager = GameObject.Find("GameManager");
                GameManager managerScript = manager.GetComponent<GameManager>();
                managerScript.score += 1000;
            }
        }
        else if (collision.gameObject.tag == "Spawner")
        {
            LoseEnergy(0.3f);
            GameObject spawnerEntity = collision.gameObject.transform.parent.gameObject;
            EnemySpawner spawnScript = spawnerEntity.GetComponent<EnemySpawner>();
            spawnScript.spawnerHealth -= projectileDamage;
            if (spawnScript.spawnerHealth <= 0)
            {
                Destroy(spawnerEntity);

                GameObject manager = GameObject.Find("GameManager");
                GameManager managerScript = manager.GetComponent<GameManager>();
                managerScript.score += 1000;
            }
        }
        else if (collision.gameObject.tag == "Environment" || collision.gameObject.tag == "Boundary")
        {
            EnvironmentManager environmentScript = collision.gameObject.GetComponent<EnvironmentManager>();
            LoseEnergy(environmentScript.elasticity);
        }
        else if (collision.gameObject.tag == "Health Orb") {
            HealthPack healingScript = collision.gameObject.GetComponent<HealthPack>();
            healingScript.durability -= projectileDamage;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject.name + " was collided with " + other.gameObject.name);
    }

    void Move() //Move Projectile base on direction vector
    {
        //gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        direction = Vector3.Normalize(pRigidbody.velocity);
        //pRigidbody.AddForce(direction * projectileSpeed * projectileAcceleration);

        //Rigidbody playerRigidbody = GameObject.Find("PlayerGroup").transform.GetComponent<Rigidbody>();
        pRigidbody.velocity = direction * projectileSpeed; // + direction * playerRigidbody.velocity.magnitude;

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

    void LoseEnergy(float reduction) 
    {
        projectileSpeed *= reduction;
        projectileMaxTimeOut *= reduction;
        projectileDamage *= reduction;

    }
}
