using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    Animator anim;
    Rigidbody2D rb;
    bool facingRight = true;
    float horizontalInput;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        if (facingRight && horizontalInput < 0)
        {
            Flip();
        }
        else if (!facingRight && horizontalInput > 0)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
