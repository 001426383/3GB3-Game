using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float acceleration;
    public float turnspeed;

    public AudioSource movementAudio;
    public AudioClip movingSound;
    public float pitchRange;
    private float originalPitch;
    private float newPitch;

    private Rigidbody playerRigidbody;
    private string forwardMovementAxisRef; //Used in the movement of player, have snap so change in movement is smoother.
    private string sidewaysMovementAxisRef;
    private string forwardAxisRef; //Used in the animation of player, doesn't have snap so transition is smoother
    private string sidewaysAxisRef;
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

    void Start()
    {
        forwardMovementAxisRef = "VerticalMove";
        sidewaysMovementAxisRef = "HorizontalMove";
        forwardAxisRef = "Vertical";
        sidewaysAxisRef = "Horizontal";
        speedupMovementRef = "SpeedUp";
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        forwardMovementValue = Input.GetAxis(forwardMovementAxisRef);
        sidewaysMovementValue = Input.GetAxis(sidewaysMovementAxisRef);
        isSpeeding = Input.GetAxis(speedupMovementRef);
        movementValue = Mathf.Abs(forwardMovementValue) + Mathf.Abs(sidewaysMovementValue); //For checking movement
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
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
        //GainScore();
        MoveHead();
        RotateBody();
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

        //Make Turning Sharper/Faster
        turnspeed = Mathf.Clamp(turnspeed, 0, 1); //Recommended 0.95
        if (playerRigidbody.velocity.x > 0 && sidewaysMovementValue < 0 || playerRigidbody.velocity.x < 0 && sidewaysMovementValue > 0)
        {
            rbVelocity = playerRigidbody.velocity;
            rbVelocity.x *= turnspeed;
            playerRigidbody.velocity = rbVelocity;
        }
        if (playerRigidbody.velocity.z > 0 && forwardMovementValue < 0 || playerRigidbody.velocity.z < 0 && forwardMovementValue > 0)
        {
            rbVelocity = playerRigidbody.velocity;
            rbVelocity.z *= turnspeed;
            playerRigidbody.velocity = rbVelocity;
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

    private void Dash() { }
    private void GainScore()
    {
        if (isMoving)
        {
            GameObject manager = GameObject.Find("GameManager");
            GameManager managerScript = manager.GetComponent<GameManager>();
            managerScript.score += 1;
        }
    }

    private void MoveHead()
    {
        if (isMoving)
        {
            float sidewaysInputDirection = Input.GetAxis(sidewaysAxisRef);
            float forwardInputDirection = Input.GetAxis(forwardAxisRef);
            GameObject head = GameObject.Find("Head");
            //Vector3 headMovement = new Vector3(0.0f, head.transform.localPosition.y, 0.0f);
            Vector3 headMovement = head.transform.localPosition;
            headMovement.x = 0.5f * sidewaysInputDirection;
            headMovement.z = 0.5f * forwardInputDirection;
            head.transform.localPosition = headMovement;
        }
    }

    private void RotateBody()
    {
        if (isMoving)
        {
            float sidewaysInputDirection = Input.GetAxis(sidewaysAxisRef);
            float forwardInputDirection = Input.GetAxis(forwardAxisRef);
            GameObject body = GameObject.Find("Body");
            Quaternion bodyRotation = body.transform.localRotation;
            //Debug.Log(bodyRotation);

            float xRotation = 0.0f;
            float yRotation = 0.0f;

            if (forwardInputDirection >= 0) //Tilt Forwards
                xRotation = 90.0f + 20.0f * Mathf.Max(Mathf.Abs(sidewaysInputDirection), Mathf.Abs(forwardInputDirection));
            else //Tilt Backwards
                xRotation = 90.0f - 20.0f * Mathf.Max(Mathf.Abs(sidewaysInputDirection), Mathf.Abs(forwardInputDirection));

            if (forwardInputDirection == 0.0f)
                yRotation = sidewaysInputDirection * 90; //Rotate by +/- 90 degrees (Prevent dividing by zero)
            else
                yRotation = Mathf.Atan(sidewaysInputDirection / forwardInputDirection) * Mathf.Rad2Deg; //Rotation after tilting    
            bodyRotation = Quaternion.Euler(xRotation, yRotation, 0);
            body.transform.localRotation = bodyRotation;
        }
    }
}
