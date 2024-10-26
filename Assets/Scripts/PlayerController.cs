using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Groundcheck and movement variables.
    public float groundCheckDistance;
    public LayerMask groundLayer;
    public LayerMask stairLayer;
    bool isGrounded;
    bool facingRight = true;
    public float moveSpeed = 10f;
    float horizontalInput;
    float decelForce = 120f;

    // Jump variables.
    public float jumpForce = 20f;
    bool isJumping = false;

    // References.
    Animator anim;
    Rigidbody2D rb;
    BoxCollider2D collider;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    void Jump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            isJumping = true;
            anim.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }



    void Update()
    {
        isGrounded = GroundCheck();

        // Get horizontal input from player.
        horizontalInput = Input.GetAxis("Horizontal");

        // Set walk speed parameter to match the velocity of player.
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (facingRight && horizontalInput < 0)
        {
            Flip();
        }
        else if (!facingRight && horizontalInput > 0)
        {
            Flip();
        }
    }

    bool GroundCheck()
    {
        Vector2 origin = new Vector2(collider.bounds.center.x, collider.bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);

        return hit.collider != null;
    }

    void StairCheck()
    {
        Vector2 origin = new Vector2(collider.bounds.center.x, collider.bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Stairs"))
            {
                if (rb.velocity.y > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                }
                else if (rb.velocity.y <= 0)
                {
                    rb.AddForce(Vector2.down * decelForce, ForceMode2D.Force);
                }
            }
        }
        
    }

    void FixedUpdate()
    {
        float verticalVelocity = rb.velocity.y;
        StairCheck();
        rb.velocity = new Vector2(horizontalInput * moveSpeed, verticalVelocity);

        if (isGrounded && rb.velocity.y <= 0)
        {
            isJumping = false;
            anim.SetBool("isJumping", false);
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
