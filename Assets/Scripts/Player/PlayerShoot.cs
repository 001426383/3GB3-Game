using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {
    public float fireRate;
    public GameObject projectileObject;

    public AudioSource shootAudioSource;
    public AudioClip shootingSound;

    public Camera cameraSource;
    private Vector3 mouseTarget;
    private float mx, my;
    private Plane groundPlane;

    public float pitchRange;
    private float originalPitch;
    private float newPitch;
    
    private float fireTimer;    

    private void Awake()
    {
        originalPitch = shootAudioSource.pitch;
        fireTimer = 0;
    }

    void Start()
    {
        //groundPlane = new Plane(Vector3.up, Vector3.up *5);
        groundPlane = new Plane(new Vector3 (0f,1f,0f), new Vector3(0f, 0f, 0f));
    }


    void Update () {
        fireTimer -= Time.deltaTime;
        if (fireTimer < 0)
        {
            shootAudioSource.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
            if (Input.GetButton("Fire1")) {
                //mouseTarget = cameraSource.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraSource.nearClipPlane));
                mx = Input.mousePosition.x;
                my = Input.mousePosition.y;

                Fire();
                //Debug.Log("Mouse Click: X = " + mouseTarget.x + "; Y = " + mouseTarget.y);
            }
            fireTimer = fireRate;
        }
    }

    void Fire ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;

        if (groundPlane.Raycast(ray, out enter))
        {
            shootAudioSource.PlayOneShot(shootingSound);
            GameObject newProjectile = Instantiate(projectileObject, gameObject.transform.position, Quaternion.identity) as GameObject;
            Rigidbody newRB = newProjectile.GetComponent<Rigidbody>();
            newProjectile.transform.parent = GameObject.Find("TemporaryEntities").transform; //Put all projectiles in temporary group
            //newRB.velocity = gameObject.GetComponent<Rigidbody>().velocity;

            //Debug.Log(ray.GetPoint(enter));
            
            Vector3 movement;
            movement = ray.GetPoint(enter) - gameObject.transform.position;
            movement.y = 0f;
            newRB.AddForce(movement, ForceMode.Impulse);
        }
        //newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * 10);

    }
}
