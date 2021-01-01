using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEFORE YOU USE THE SCRIPT:
// How to set this up:
// 1. Create an empty object and give it the CharacterController Component
// 2. Within the empty object put the body and camera in.
// 3. Link the Camera to playerCamera
// 4. Link the empty object to the playerController.

// The Changeable values are what i've tested to be good, but obviously they can be changed

// If you get stuck on walls, make a physics material with zero friction on your character body

public class PlayerMovement : MonoBehaviour {

    // References the Camera and Controller of the Player
    public Transform playerCamera;
    public CharacterController playerController;

    // Changeable Values
    public float playerSpeed = 6.68f;
    public float sensitivity = 100f;
    public float gravity = -12f;
    public float jumpHeight = 1f;
    public float runMultiplier = 1.5f;

    // Private Variables
    private float lookX;
    private float lookY;
    private Vector3 velocity;
    private bool jumpState = false;
    private bool runState = false;

    void Start() {

        // Hides Cursor
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update () {

        // Updates the Rotation of the Camera and the Player
        UpdateLook();

        // Updates the Direction/Movement of the player 
        UpdateDirection();

    }

    // Turns the Body and Camera to look around
    void UpdateLook() {

        // Gets Mouse X and Y Axes
        float mouseHorizontal = Input.GetAxis("Mouse X");
        float mouseVertical = Input.GetAxis("Mouse Y");

        // Adds the Axes to respective variables
        // (Mouse Axes reset to 0 or else your camera will be locked to zero)
        lookX -= mouseVertical * sensitivity * Time.deltaTime;
        lookY += mouseHorizontal * sensitivity * Time.deltaTime;

        // Clamps the looking up direction so you can look backwards
        lookX = Mathf.Clamp(lookX, -90f, 90f);

        // Turns the camera and the controller separately
        // Stops the controller from turning up when looking up
        playerCamera.transform.localRotation = Quaternion.Euler(lookX, 0, 0);
        playerController.transform.localRotation = Quaternion.Euler(0, lookY, 0);

    }

    void UpdateDirection() {

        // Walking Motion

        // Gets horizontal and vertical inputs
        float strafeMovement = Input.GetAxis("Horizontal");
        float walkMovement = Input.GetAxis("Vertical");

        float speed = playerSpeed;

        if (runState) {

            speed = playerSpeed * runMultiplier;

        }else {

            speed = playerSpeed;

        }

        Vector3 move = transform.right * strafeMovement + transform.forward * walkMovement;

        playerController.Move(move * speed * Time.deltaTime);

        // Jumping Motion

        if (Input.GetKey("space")) {

            if (jumpState == false) {

                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                jumpState = true;

            }

        }

        // Running Toggle

        if (Input.GetKeyDown("left shift"))
        {

            runState = true;

        }

        if (Input.GetKeyUp("left shift"))
        {

            runState = false;

        }

        // Falling Motion

        velocity.y += gravity * Time.deltaTime;
        Debug.Log(velocity.y);
        playerController.Move(velocity * Time.deltaTime);


        // Landing Script

        if (playerController.isGrounded)
        {

            velocity.y = 0;
            jumpState = false;

        }


    }

}
