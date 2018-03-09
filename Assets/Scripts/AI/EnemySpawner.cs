using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject spawnerEntity;
    public float spawnFrequency;
    public float spawnerHealth;
    public int maxAmount;
    private int currentAmount;
    private float spawnTimer;
    //private GameObject[] childEntity;

    // Use this for initialization
    void Start () {
        currentAmount = 0;
     spawnTimer = spawnFrequency;
    }
	
	// Update is called once per frame
	void Update ()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0)
        {
            if (currentAmount < maxAmount)
            {
                GameObject newEntity = Instantiate(spawnerEntity, transform.position, Quaternion.identity);
                newEntity.transform.parent = gameObject.transform;
                currentAmount++;
            }
            spawnTimer = spawnFrequency;
        }
    }
}
