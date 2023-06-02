using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    public float movementSpeed = 10f;
    public float tiltAmount = 10f;

    private Rigidbody rb;
    private Vector3 movementVector = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Get the horizontal and vertical input from the joystick
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        // Create a movement vector based on the joystick input
        Vector3 joystickInput = new Vector3(horizontalInput, 0f, verticalInput);
        movementVector = joystickInput * movementSpeed;

        // Tilt the Beyblade based on the joystick input
        float tiltAngleX = verticalInput * tiltAmount;
        float tiltAngleZ = -horizontalInput * tiltAmount;
        Quaternion tiltRotation = Quaternion.Euler(tiltAngleX, 0f, tiltAngleZ);
        transform.rotation = tiltRotation * transform.rotation;
    }

    private void FixedUpdate()
    {
        // Move the Beyblade based on the movement vector
        rb.AddForce(movementVector, ForceMode.Force);
    }
}
