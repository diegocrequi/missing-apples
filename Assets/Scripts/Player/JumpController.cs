using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    public float jumpForce;
    public float wallJumpXForce;
    public float wallJumpYForce;
    public float coyoteTime;
    public float coyoteTimeWall;
    public float speedFrictionOnWall;

    Rigidbody2D rigidBody;
    Animator animator;
    InputActions inputActions;
    [SerializeField]
    bool canJump;
    [SerializeField]
    bool jumpFromWall;
    [SerializeField]
    bool onGround;
    [SerializeField]
    bool onWall;
    [SerializeField]
    bool pendingOnWall;
    float coyoteTimeCounter;
    float coyoteTimeWallCounter;
    MovementController movementController;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        movementController = gameObject.GetComponent<MovementController>();
        inputActions = new InputActions();
        inputActions.Player.Enable();
        coyoteTimeCounter = coyoteTime;
        coyoteTimeWallCounter = coyoteTimeWall;
    }

    void Update()
    {
        manageCoyoteTime();
        manageCoyoteWallTime();
        manageHangingWall();
        if ((canJump || jumpFromWall) && inputActions.Player.Jump.WasPressedThisFrame())
        {
            performJump();
        }

        if(!canJump && !onWall && rigidBody.velocity.y > 0)
        {
            animator.SetInteger("state", (int)PlayerStateEnum.JUMPING);
        }
        if(!canJump && !onWall && rigidBody.velocity.y < 0)
        {
            animator.SetInteger("state", (int)PlayerStateEnum.FALLING);
        }
        if(pendingOnWall && rigidBody.velocity.y < -0.5)
        {
            onTouchingRightWall();
            rigidBody.AddForce(new Vector2(3 * transform.localScale.x, 0), ForceMode2D.Impulse);
        }
    }

    void performJump()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        float direction = onWall ? -transform.localScale.x : transform.localScale.x;
        float xForce = jumpFromWall ? wallJumpXForce * direction : 0;
        float yForce = jumpFromWall ? wallJumpYForce : jumpForce;
        rigidBody.AddForce(new Vector2(xForce, yForce), ForceMode2D.Force);
        canJump = false;
        jumpFromWall = false;
        onWall = false;
    }

    void manageCoyoteTime()
    {
        if(canJump && !onGround && !onWall)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if(coyoteTimeCounter < 0)
        {
            canJump = false;
        }
    }

    void manageCoyoteWallTime()
    {
        if (canJump && !onGround && !onWall)
        {
            coyoteTimeWallCounter -= Time.deltaTime;
        }
        if (coyoteTimeWallCounter < 0)
        {
            jumpFromWall = false;
        }
    }

    void manageHangingWall()
    {
        if(onWall)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, speedFrictionOnWall * -1);
        }
    }

    public void onLanding()
    {
        onGround = true;
        canJump = true;
        jumpFromWall = false;
        coyoteTimeCounter = coyoteTime;
        coyoteTimeWallCounter = coyoteTimeWall;
        onLeavingRightWall();
        movementController.manageAnimations();
    }

    public void onJumping()
    {
        onGround = false;
    }

    public void onTouchingRightWall() 
    {
        if(!onGround) {
            onWall = true;
            canJump = true;
            jumpFromWall = true;
            coyoteTimeWallCounter = coyoteTimeWall;
            animator.SetInteger("state", (int)PlayerStateEnum.HANGING_WALL);
            transform.GetChild(1).GetChild(0).GetComponent<BoxCollider2D>().gameObject.SetActive(false);
            transform.GetChild(1).GetChild(1).GetComponent<BoxCollider2D>().gameObject.SetActive(true);
        }
        pendingOnWall = true;
    }

    public void onLeavingRightWall()
    {
        if(onWall) {
            onWall = false;
            coyoteTimeCounter = coyoteTime;
            transform.GetChild(1).GetChild(0).GetComponent<BoxCollider2D>().gameObject.SetActive(true);
            transform.GetChild(1).GetChild(1).GetComponent<BoxCollider2D>().gameObject.SetActive(false);
        }
    }

    public void desactivatingOnPendingWall()
    {
        pendingOnWall = false;
    }

    public bool getOnGround()
    {
        return onGround;
    }

    public bool getOnWall()
    {
        return onWall;
    }
}
