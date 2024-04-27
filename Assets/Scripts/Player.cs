using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    private float horizontal;
    private bool isFacingRight = true;

    EPolarity playerPolarity;

    Rigidbody2D rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //Function to Check if Player is Ground
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);
    }
    //Function to Flip the player
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight=!isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }   

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        Flip();

        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }

    private void FixedUpdate() 
    {
        rb.velocity = new Vector2(horizontal * playerSpeed, rb.velocity.y);
       
    }

    //Function for Fetching Player Polarity
    public EPolarity GetEPolarity()
    {
        return playerPolarity;
    }

    //Function for Setting/Updating Player Polarity
    public void setPolarity(EPolarity newPolarity)
    {
        playerPolarity = newPolarity;
    }
}

public enum EPolarity
    {
        Positive,
        Negative,
        Neutral
    };
