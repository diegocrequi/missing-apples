using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    public float moveSpeed;
    public float maxMoveSpeed;
    public float runSpeedMultiplier;
    public float jumpSpeedMultiplier;
    public float maxRunSpeed;
    public float desaccelerationRate;

    Rigidbody2D rigidBody;
    InputActions inputActions;
    Animator animator;
    JumpController jumpController;
    float movement;
    bool isRunning;

    void Awake()
    {
        transform.position = GameObject.FindWithTag("Respawn").transform.position;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        jumpController = gameObject.GetComponent<JumpController>(); ;
        inputActions = new InputActions();
        inputActions.Player.Enable();
    }

    void Update()
    {
        movement = inputActions.Player.Move.ReadValue<float>();
        isRunning = inputActions.Player.Run.IsPressed();
    }

    void FixedUpdate()
    {
        manageMovement();
        manageMaxSpeed();
        if(!jumpController.getOnWall()) {
            manageAnimations();
        }
    }

    void manageMovement()
    {
        if (inputActions.Player.Move.inProgress)
        {
            Vector2 movementVector = new Vector2(movement * moveSpeed * Time.deltaTime, 0);
            if (isRunning) movementVector *= runSpeedMultiplier;
            if (!jumpController.getOnGround()) movementVector *= jumpSpeedMultiplier;
            rigidBody.AddForce(movementVector, ForceMode2D.Force);
            if (movement > 0) transform.localScale = new Vector3(1, 1, 1);
            if (movement < 0) transform.localScale = new Vector3(-1, 1, 1);
        }
        desaccelerate();
    }

    void desaccelerate()
    {
        if(inputActions.Player.Move.inProgress && jumpController.getOnGround())
        {
            if (movement > 0 && rigidBody.velocity.x < 0 || movement < 0 && rigidBody.velocity.x > 0) 
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        } else
        {
            if (rigidBody.velocity.x != 0 && jumpController.getOnGround())
            {
                float speed = Mathf.Lerp(rigidBody.velocity.x, 0, desaccelerationRate);
                if (Mathf.Abs(rigidBody.velocity.x) < 0.001) speed = 0;
                rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
            }
        }
    }

    void manageMaxSpeed()
    {
        float speedLimit = isRunning ? maxRunSpeed : maxMoveSpeed;
        if(rigidBody.velocity.x > speedLimit)
        {
            rigidBody.velocity = new Vector2(speedLimit, rigidBody.velocity.y);
        }
        if (rigidBody.velocity.x < -speedLimit)
        {
            rigidBody.velocity = new Vector2(-speedLimit, rigidBody.velocity.y);
        }
    }

    public void manageAnimations()
    {
        if (jumpController.getOnGround())
        {
            if (inputActions.Player.Move.inProgress)
            {
                animator.SetInteger("state", (int)PlayerStateEnum.WALKING);
            }
            else
            {
                animator.SetInteger("state", (int)PlayerStateEnum.IDLE);
            }
        }
    }
}
