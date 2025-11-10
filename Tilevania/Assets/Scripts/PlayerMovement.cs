using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rigidbody;
    BoxCollider2D collider;
    Animator animator;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float maxVelocity = 10f;

    float gravityScaleAtStart;

    bool isMoving = false;
    bool isClimbing = false;
    bool isTouchingLadder = false;
    bool isJumpHeld = false;

    [SerializeField] Transform groundCheck;
    bool isGrounded;

    [SerializeField] float coyoteTime = 0.1f;
    float coyoteTimeCounter;

    float cutOffMultiplier = 0.5f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rigidbody.gravityScale;
    }

    private void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, LayerMask.GetMask("Ground"));

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        } else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        
        if(isJumpHeld && !Keyboard.current.wKey.isPressed)
        {
            isJumpHeld = false;
            if(rigidbody.linearVelocity.y > Mathf.Epsilon)
            {
                Vector2 v = rigidbody.linearVelocity;
                v.y *= cutOffMultiplier;
                rigidbody.linearVelocity = v;
            }

        }

        if(rigidbody.linearVelocity.y > maxVelocity)
        {
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, maxVelocity);
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if ((isGrounded || coyoteTimeCounter > Mathf.Epsilon) && value.isPressed)
        {
            Vector2 v = rigidbody.linearVelocity;
            v.y = jumpSpeed;
            rigidbody.linearVelocity = v;
            isJumpHeld = true;
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rigidbody.linearVelocity.y);
        rigidbody.linearVelocity = playerVelocity;
        animator.SetBool("isRunning", isMoving);
    }

    void FlipSprite()
    {
        isMoving = Mathf.Abs(rigidbody.linearVelocity.x) > Mathf.Epsilon;
        if (isMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody.linearVelocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        isClimbing = Mathf.Abs(rigidbody.linearVelocity.y) > Mathf.Epsilon;
        isTouchingLadder = collider.IsTouchingLayers(LayerMask.GetMask("Climbing"));

        animator.SetBool("isClimbing", isClimbing && isTouchingLadder);
        if (isTouchingLadder)
        {
            Vector2 climbVelocity = new Vector2(rigidbody.linearVelocity.x, moveInput.y * moveSpeed);
            rigidbody.linearVelocity = climbVelocity;
            rigidbody.gravityScale = 0f;
        } else
        {
            rigidbody.gravityScale = gravityScaleAtStart;
        }
    }
}
