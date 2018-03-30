using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameObject player;
    //public GameObject wallObjects;
    //public GameObject boundaryObjects;
    //public GameObject enemyObjects;

    public AudioSource collisionAudioSource;
    public AudioClip playerWallSound;
    public AudioClip playerBoundarySound;
    public AudioClip playerEnemySound;
    public float pitchRange;
    private float originalPitch;


    private void Awake()
    {
        originalPitch = collisionAudioSource.pitch;
    }

    void Start()
    {
    }


    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        collisionAudioSource.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
        if (collision.gameObject.tag == "Boundary")
        {
            collisionAudioSource.PlayOneShot(playerBoundarySound);
        }
        else if (collision.gameObject.tag == "Environment")
        {
            //collision.gameObject.transform.rotation.y
            collisionAudioSource.PlayOneShot(playerWallSound);
            EnvironmentManager environmentScript = collision.gameObject.GetComponent<EnvironmentManager>();

            Rigidbody playerRigidbody = GetComponent<Rigidbody>();
            playerRigidbody.velocity *= Mathf.Clamp(environmentScript.elasticity - 0.2f, 0.0f, 1.0f);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            GameObject enemyEntity = collision.gameObject.transform.parent.gameObject;
            GameObject spawnSource = enemyEntity.transform.parent.gameObject;
            EnemySpawner spawnScript = spawnSource.GetComponent<EnemySpawner>();
            spawnScript.currentAmount -= 1;

            collisionAudioSource.PlayOneShot(playerEnemySound);
            Destroy(enemyEntity);


            GameObject manager = GameObject.Find("GameManager");
            GameManager managerScript = manager.GetComponent<GameManager>();
            EnemyProperties propertiesScript = enemyEntity.GetComponent<EnemyProperties>();

            managerScript.playerHealth -= propertiesScript.collisionDamage;
            /*
            GameObject manager = GameObject.Find("GameManager");
            GameManager managerScript = manager.GetComponent<GameManager>();
            managerScript.score -= 500;
            */

            //GameObject.Destroy(gameObject);
        }
        //Debug.Log(gameObject.name + " has collided with " + collision.gameObject.name);
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject.name + " was collided with " + other.gameObject.name);
    }
}
