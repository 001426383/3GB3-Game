using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public float speed = 180f;
    public float healAmount = 10;
    public float durability;

    void Start()
    {
    }
    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
        if (durability <= 0) {
            GameObject manager = GameObject.Find("GameManager");
            GameManager managerScript = manager.GetComponent<GameManager>();
            managerScript.playerHealth += healAmount;
            Destroy(gameObject);
        }
    }
}