using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameObject player;
    public GameObject wallObjects;
    public GameObject boundaryObjects;
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
        if (collision.gameObject.Equals(boundaryObjects))
        {
            collisionAudioSource.PlayOneShot(playerBoundarySound);
        }
        else if (collision.gameObject.Equals(wallObjects))
        {
            //collision.gameObject.transform.rotation.y
            collisionAudioSource.PlayOneShot(playerWallSound);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            collisionAudioSource.PlayOneShot(playerEnemySound);
            Destroy(collision.gameObject.transform.parent.gameObject);
            GameObject manager = GameObject.Find("GameManager");
            GameManager managerScript = manager.GetComponent<GameManager>();
            managerScript.score -= 500;

            //GameObject.Destroy(gameObject);
        }
        //Debug.Log(gameObject.name + " has collided with " + collision.gameObject.name);
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject.name + " was collided with " + other.gameObject.name);
    }
}
