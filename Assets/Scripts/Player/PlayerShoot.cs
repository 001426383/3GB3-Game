using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject[] projectileObject;
    public Material[] headMaterial;

    public AudioSource shootAudioSource;
    public AudioClip shootingSound;

    public Camera cameraSource;
    private Plane groundPlane;

    public float pitchRange;
    private float originalPitch;
    private float newPitch;
    private float soundTimer;

    private float fireRate;
    private float fireTimer;
    private bool evenSpread;
    private bool isAutomatic;
    private float spreadMultiplier; //Projectiles to fire per shot
    private float spreadDeviation; //Arc of spread

    private int equippedGun;

    private void Awake()
    {
        originalPitch = shootAudioSource.pitch;
        fireTimer = 0;
        ChangeGun(0);
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    void Start()
    {
        //groundPlane = new Plane(Vector3.up, Vector3.up *5);
        groundPlane = new Plane(new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 0f));
    }


    void Update()
    {
        //Change Gun
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeGun(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeGun(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeGun(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeGun(3);
        
        //Fire Projectile
        fireTimer -= Time.deltaTime;
        if (fireTimer < 0)
        {
            shootAudioSource.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);

            for (int i = 0; i < spreadMultiplier; i++)
            {
                if (isAutomatic)
                {
                    if (Input.GetButton("Fire1"))
                    { //Fires while button is held
                        Fire();
                        fireTimer = fireRate; //only reset time if fired
                    }
                }
                else
                {
                    if (Input.GetButtonDown("Fire1")) //Fires a single time
                    {
                        Fire();
                        fireTimer = fireRate; //only reset time if fired
                    }
                    //Debug.Log(Input.GetButtonDown("Fire1"));
                }
            }
            //Debug.Log("Mouse Click: X = " + mouseTarget.x + "; Y = " + mouseTarget.y);
        }
    }

    void Fire()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;

        if (groundPlane.Raycast(ray, out enter))
        {
            PlaySound();

            //Slightly move the starting point some distance from player
            Vector3 initPosition = ray.GetPoint(enter) - gameObject.transform.position;
            Vector3 normalizedTarget = initPosition.normalized;
            float offset = 1.5f;
            initPosition = gameObject.transform.position + offset * normalizedTarget;
            initPosition.y = gameObject.transform.position.y;

            //Instantiate projectile
            GameObject newProjectile = Instantiate(projectileObject[equippedGun], initPosition, Quaternion.identity) as GameObject;
            Rigidbody newRB = newProjectile.GetComponent<Rigidbody>();
            newProjectile.transform.parent = GameObject.Find("TemporaryEntities").transform; //Put all projectiles in temporary group
                                                                                             //newRB.velocity = gameObject.GetComponent<Rigidbody>().velocity;

            //Debug.Log(ray.GetPoint(enter));

            //Set the initial direction of the projectile
            Vector3 initDirection;
            initDirection = ray.GetPoint(enter) - gameObject.transform.position;
            initDirection.y = 0f;
            //newRB.AddForce(fireDirection, ForceMode.Impulse);

            //Debug.Log(fireDirection);
            Quaternion fireDirection = Quaternion.LookRotation(initDirection);
            if (evenSpread) //Even spread distribution
            {
                //TODO
            }
            else //Uneven random spread
            {
                Quaternion randRotation = Random.rotation;
                fireDirection = Quaternion.RotateTowards(fireDirection, randRotation, Random.Range(0.0f, spreadDeviation));
            }
            newRB.AddForce(fireDirection * Vector3.forward, ForceMode.Impulse);
            
        }
    }

    private void ChangeGun(int g)
    {
        GameObject head = GameObject.Find("Head");
        head.GetComponent<Renderer>().material = headMaterial[g];

        equippedGun = g;
        if (equippedGun == 0) //Laser Gun
        {
            fireRate = 0.1f;
            evenSpread = false;
            spreadMultiplier = 1f;
            spreadDeviation = 2f;
            isAutomatic = true;
        }
        else if (equippedGun == 1) //Machine Gun
        {
            fireRate = 0.02f;
            evenSpread = false;
            spreadMultiplier = 1f;
            spreadDeviation = 30f;
            isAutomatic = true;
        }
        else if (equippedGun == 2) //Shotgun
        {
            fireRate = 0.5f;
            evenSpread = false;
            spreadMultiplier = 8f;
            spreadDeviation = 20f;
            isAutomatic = false;
        }
        else if (equippedGun == 3) //Laser Launcher
        {
            fireRate = 1f;
            evenSpread = true;
            spreadMultiplier = 16f;
            spreadDeviation = 0f;
            isAutomatic = false;
        }
    }

    private void PlaySound() {

        soundTimer -= Time.deltaTime;
        if (soundTimer < 0)
        {
            if (equippedGun == 0)
            {
                shootAudioSource.PlayOneShot(shootingSound);
                soundTimer = 0f;
            }
            else if (equippedGun == 1)
            {
                shootAudioSource.PlayOneShot(shootingSound);
                soundTimer = 0.03f;
            }
            else if (equippedGun == 2)
            {
                shootAudioSource.PlayOneShot(shootingSound);
                soundTimer = 0.1f;
            }
            else if (equippedGun == 3)
            {
                shootAudioSource.PlayOneShot(shootingSound);
                soundTimer = 0.1f;
            }
        }
    }
}
