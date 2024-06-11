using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalTileMovement : MonoBehaviour
{
    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
        private Vector3 localScale;
    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        moveSpeed = 3f;

    }
    public void OnTrigger2D(Collider2D collision)
    {
        if (collision.GetComponent<Tiles>())
        {
            dirX*= -1f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);        
    }
}
