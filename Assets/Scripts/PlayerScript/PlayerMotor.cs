using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System;

// drag in inspector

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed = 5f;
    public float gravity = -15.8f;
    public float jumpHeight = 15f;

    
    void Start() => controller = GetComponent<CharacterController>();
    //touching the ground during the last movement
    private void Update() => isGrounded = controller.isGrounded;

    //receive the inputs for our InputManager.cs and apply them to our character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        moveDirection = moveDirection * speed;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);
     //   Debug.Log(playerVelocity.y);
    }

    //Jump
    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

    }
  
}
