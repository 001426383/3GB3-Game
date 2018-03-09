using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed;
    public float acceleration;

    public AudioSource movementAudio;
    public AudioClip movingSound;
    public float pitchRange;
    private float originalPitch;
    private float newPitch;

    private Rigidbody playerRigidbody;
    private string forwardMovementAxisRef;
    private string sidewaysMovementAxisRef;
    private string speedupMovementRef;
    private float movementValue = 0f;
    private float forwardMovementValue = 0f;
    private float sidewaysMovementValue = 0f;
    private float isSpeeding = 0f;
    private float originalSpeed;
    private bool isMoving;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        originalSpeed = speed;
        movementAudio.clip = movingSound;
        originalPitch = movementAudio.pitch;
    }

    void Start () {
        forwardMovementAxisRef = "Vertical";
        sidewaysMovementAxisRef = "Horizontal";
        speedupMovementRef = "SpeedUp";
        isMoving = false;
    }
	
	// Update is called once per frame
	void Update () {
        forwardMovementValue = Input.GetAxis(forwardMovementAxisRef);
        sidewaysMovementValue = Input.GetAxis(sidewaysMovementAxisRef);
        isSpeeding = Input.GetAxis(speedupMovementRef);
        movementValue = Mathf.Abs(forwardMovementValue) + Mathf.Abs(sidewaysMovementValue); //For checking movement
	}

    private void OnEnable ()
    {

    }
		
	private void OnDisable ()
	{
        //player_Rigidbody.isKinematic = true;
	}

    private void MovementAudio()
    {

        if (!isMoving && Mathf.Abs(movementValue) > 0.0f) // Start Sound
        {
            movementAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
            newPitch = movementAudio.pitch;
            movementAudio.time = 0f;
            movementAudio.Play();
            isMoving = true;
        }
        else if (Mathf.Abs(movementValue) >= 0.01f) //Continuous Sound
        {
            if (movementAudio.time > 3.5f)
            {
                //movementAudio.pitch = 0 - movementAudio.pitch;
                movementAudio.time = 2f;
            }
            //else if (movementAudio.time < 2.5f)
            //{
            //    movementAudio.pitch = Mathf.Abs(movementAudio.pitch);
            //}

            if (isSpeeding > 0)
            {
                if (movementAudio.pitch < newPitch + 0.1f)
                    movementAudio.pitch += 0.01f;
            }
            else
            {
                if (movementAudio.pitch > newPitch)
                    movementAudio.pitch -= 0.01f;
            }
            //Debug.Log("AudioTime: " + movementAudio.time);

        }
        else //Idle Sound
        {
            if (movementAudio.time > 1f && movementAudio.time < 3f)
            {
                movementAudio.pitch = Mathf.Abs(movementAudio.pitch);
                movementAudio.time = 3f;
            }
            if (movementAudio.time >= 7f)
                movementAudio.Stop();
            isMoving = false;
        }
    }

    // Runs at a fixed time (could be none or more than once per frame).
    private void FixedUpdate()
    {
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        Move();
        MovementAudio();
    }

    private void Move()
    {
        if (isSpeeding > 0)
            speed = originalSpeed + 30 * isSpeeding;
        else
            speed = originalSpeed;
        /*
        Vector3 movement;
        movement = transform.forward * forwardMovementValue + transform.right * sidewaysMovementValue;

        movement = movement * speed * Time.deltaTime ;

        // Apply this movement to the rigidbody's position.
        playerRigidbody.MovePosition(playerRigidbody.position + movement);
        */

       
        float maxVelocity = speed;
        float maxVelocitySqr = maxVelocity * maxVelocity;
        Vector3 rbVelocity = playerRigidbody.velocity;
        if (rbVelocity.sqrMagnitude > maxVelocitySqr)
        {
            playerRigidbody.velocity = rbVelocity.normalized * maxVelocity; //Limits max velocity
        }

        /*
        rbVelocity = playerRigidbody.velocity;
        //Offset Vertical Velocity if travelling in oppoisite direction
        if ((sidewaysMovementValue > 0 && rbVelocity.x < 0) || (sidewaysMovementValue < 0 && rbVelocity.x > 0))
        {
            if (rbVelocity.z > 0)
                playerRigidbody.AddForce(new Vector3(0.0f,0.0f,-1.0f) * speed * acceleration);
            else
                playerRigidbody.AddForce(new Vector3(0.0f,0.0f,1.0f) * speed * acceleration);
        }
        //Offset Horizontal Velocity if travelling in oppoisite direction
        if ((forwardMovementValue > 0 && rbVelocity.z < 0) || (forwardMovementValue < 0 && rbVelocity.z > 0))
        {
            if (rbVelocity.z > 0)
                playerRigidbody.AddForce(new Vector3(-1.0f,0.0f,0.0f) * speed * acceleration);
            else
                playerRigidbody.AddForce(new Vector3(1.0f,0.0f,0.0f) * speed * acceleration);
        }
        */
        Vector3 movement = new Vector3(sidewaysMovementValue, 0.0f, forwardMovementValue);

        movement = movement * speed * acceleration;
        playerRigidbody.AddForce(movement, ForceMode.Acceleration);
        //Debug.Log(movement);
        //Debug.Log(playerRigidbody.velocity);
    }
}
