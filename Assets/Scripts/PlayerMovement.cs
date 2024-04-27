using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private Vector3 cameraOffset = new Vector3(0,-10,10);

    Vector3 playerPosition;

    [SerializeField]
    public float playerSpeed;

    [SerializeField]
    Camera mainCamera;
    private Vector2 playerDirection;




    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        playerPosition = transform.position;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition += playerPosition + new Vector3(playerSpeed*Time.deltaTime, 0, 0);
    }
   
}
