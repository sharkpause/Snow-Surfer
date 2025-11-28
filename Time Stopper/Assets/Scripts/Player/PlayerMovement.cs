using NUnit.Framework.Interfaces;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float moveAcceleration = 30f;

    [SerializeField] float jumpForce = 5f;
    [SerializeField] float normalGravityScale = 3f;
    [SerializeField] float fallGravityScale = 4f;
    [SerializeField] float apexBonus = 2f;
    
    [SerializeField] float jumpBufferTime = 0.1f;
    float jumpBufferTimer;

    [SerializeField] float dashSpeed = 20f;
    [SerializeField] float dashDuration = 0.15f;
    bool isDashing = false;
    float dashTimeCounter;
    float dashDirection = 1f;

    [SerializeField] float dashCooldown = 1f;
    private float dashCooldownTimer = 0f;

    Rigidbody2D rb;

    float moveDirection = 0;

    bool isGrounded = false;
    int groundLayer;

    float coyoteTime = 0.1f;
    float coyoteTimeCounter = 0f;
    float apexPoint;

    private void Awake()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleJumpBufferTimer();

        HandleDashCooldownTimer();
    }

    void HandleJumpBufferTimer()
    {
        if (jumpBufferTimer > 0)
        {
            jumpBufferTimer -= Time.deltaTime;
        }
    }

    void HandleDashCooldownTimer()
    {
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        HandleCoyoteTime();

        HandleFallGravityScale();

        HandleMove();

        HandleApexPoint();

        HandleDash();
    }

    void HandleCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }
    }

    void HandleFallGravityScale()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = fallGravityScale;
        }
        else
        {
            rb.gravityScale = normalGravityScale;
        }
    }

    void HandleApexPoint()
    {
        apexPoint = Mathf.Clamp01(
            1f - Mathf.Abs(rb.linearVelocity.y) / jumpForce
        );
    }

    void HandleDash()
    {
        if (isDashing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);
            dashTimeCounter -= Time.fixedDeltaTime;

            if (dashTimeCounter <= 0f)
            {
                rb.gravityScale = normalGravityScale;
                isDashing = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.IsTouchingLayers(groundLayer))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.IsTouchingLayers(groundLayer))
        {
            isGrounded = false;
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<float>();

        if(moveDirection != 0)
        {
            dashDirection = Mathf.Sign(moveDirection);
        }
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if(ctx.started && !isDashing && dashCooldownTimer <= 0f)
        {
            isDashing = true;
            dashTimeCounter = dashDuration;
            dashCooldownTimer = dashCooldown;
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            jumpBufferTimer = jumpBufferTime;
            if ((jumpBufferTimer > 0f) && (isGrounded || coyoteTimeCounter > 0f))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpBufferTimer = 0f;
            }
        } else if(ctx.canceled)
        {
            if(rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }
    }

    void HandleMove()
    {
        float apexAccel = 1f + (apexPoint * apexBonus);
        float effectiveAcceleration = moveAcceleration * apexAccel;

        if (moveDirection == 1 || moveDirection == -1) {
            rb.linearVelocity = new Vector2(
                Mathf.Lerp(rb.linearVelocity.x, moveDirection * moveSpeed, effectiveAcceleration * Time.fixedDeltaTime),
                rb.linearVelocity.y
            );
        } else
        {
            rb.linearVelocity = new Vector2(
                Mathf.Lerp(rb.linearVelocity.x, 0, effectiveAcceleration * Time.fixedDeltaTime),
                rb.linearVelocity.y
            );
        }
    }
}
