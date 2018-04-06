using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    public float projectileMinTimeOut;
    public float projectileMaxTimeOut;
    public float projectileDamage;
    bool isColliding;



    void Start()
    {
        //gameObject.timeoutDestructor = projectileTimeOut;
        projectileMaxTimeOut = Random.Range(projectileMinTimeOut, projectileMaxTimeOut);
    }

    void Update()
    {
        projectileMaxTimeOut -= Time.deltaTime;
        if (projectileMaxTimeOut < 0)
        {
            Destroy(gameObject);
        }
        isColliding = false;
    }

    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (isColliding) return;
        isColliding = true;
        if (collision.gameObject.name == "PlayerGroup")
        {
            //Debug.Log(gameObject.name + " has collided with " + collision.gameObject.name);

            GameObject manager = GameObject.Find("GameManager");
            GameManager managerScript = manager.GetComponent<GameManager>();
            managerScript.playerHealth -= projectileDamage;

            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Environment" || collision.gameObject.tag == "Boundary")
        {
            EnvironmentManager environmentScript = collision.gameObject.GetComponent<EnvironmentManager>();
            LoseEnergy(environmentScript.elasticity);
        }
    }
    void LoseEnergy(float reduction)
    {
        projectileMaxTimeOut *= reduction;
        projectileDamage *= reduction;
    }
}
