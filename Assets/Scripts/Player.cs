

using System.Collections;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform grabPoint;
    [SerializeField] Transform rayPoint;
    [SerializeField] float polarityResetTimer;
    float rayDistance;
    private GameObject grabbedTile;
    private int layerIndex;
    bool isActve;
    


    private float horizontal;
    private bool isFacingRight = true;

    EPolarity playerPolarity;

    Rigidbody2D rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() 
    {
        isActve = false;
        layerIndex = LayerMask.NameToLayer("Ground");
        Debug.Log(layerIndex);
        playerPolarity = EPolarity.Negative;
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
        if(grabbedTile !=null)
        {
            if(grabbedTile.gameObject.GetComponent<Tiles>().getTilePolarity()!=playerPolarity)
            {
                attractTile(grabbedTile);
            }
            if(grabbedTile.gameObject.GetComponent<Tiles>().getTilePolarity()==playerPolarity)
            {
                repelTile(grabbedTile);
            }
        }
        

        
        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();

        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(!isActve)
            {
                StartCoroutine(ActivatePolarity(polarityResetTimer, EPolarity.Positive));
                Debug.Log("xyz");
            }
            //playerPolarity = EPolarity.Positive;
            //ActivatePolarity(EPolarity.Positive);
            
            Debug.Log("Polarity Updated "+playerPolarity);
            Debug.Log("Mouse Button Down Left");
            Vector3 touchPos = getTouchPosition();
            Debug.Log("Touch Pos = "+touchPos);

            Debug.Log(" Ray Distance "+Vector3.Distance(rayPoint.position, touchPos));

            Debug.Log("Start Position "+rayPoint.position);
            Debug.Log("End Position"+(touchPos));

            RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, touchPos-rayPoint.transform.position, Mathf.Infinity, groundLayer);
            Debug.DrawRay(rayPoint.position, touchPos-rayPoint.transform.position, Color.red, 500f);
            

            Debug.Log(hitInfo.collider);

            if(hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)  
                {
                Debug.Log("Collider Layer : "+hitInfo.collider.gameObject.layer.ToString());
                
                if (hitInfo.collider.gameObject.GetComponent<Tiles>().getTilePolarity() != EPolarity.Neutral)
                {
                    if(hitInfo.collider.gameObject.GetComponent<Tiles>().getTilePolarity() != playerPolarity)
                    {
                        grabbedTile = hitInfo.collider.gameObject;
                        //moveTile(grabbedTile);                       
                    }
                }
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("Mouse Button Down Left");
            if(!isActve)
            {
                ActivatePolarity(polarityResetTimer, EPolarity.Negative);
            }
            Debug.Log("Polarity Updated "+playerPolarity);
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

    Vector3 getTouchPosition()
    {
        //     Touch touch = Input.GetTouch(0);
        //     Vector3 touchPostion = Camera.main.ScreenToWorldPoint(touch.position);
        //     touchPostion.z=0f;
        //     return touchPostion;

        //Touch touch = Input.GetTouch(0);
        Vector3 touchPostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchPostion.z=0f;

        return touchPostion;
    }

    Vector3 placementOffset(Transform objectTransform)
    {
        return new Vector3((objectTransform.localScale.x/2),0f,0f);
    }

    void attractTile(GameObject tileToMove)
    {
        tileToMove.GetComponent<Rigidbody2D>().isKinematic = true;
        if(transform.position.x < grabPoint.position.x)
        {
            //grabbedTile.GetComponent<BoxCollider2D>().isTrigger = true;
            grabbedTile.transform.position = Vector3.MoveTowards(grabbedTile.transform.position, 
                                                                grabPoint.position+placementOffset(grabbedTile.transform)+new Vector3(0.1f,0f,0f),
                                                                0.05f);
            //grabbedTile.transform.position = grabPoint.position+placementOffset(grabbedTile.transform)+new Vector3(0.1f,0f,0f);
        }
        else
        {
            grabbedTile.GetComponent<BoxCollider2D>().isTrigger = true;
            grabbedTile.transform.position = Vector3.MoveTowards(grabbedTile.transform.position, 
                                                                grabPoint.position-placementOffset(grabbedTile.transform)-new Vector3(0.1f,0f,0f),
                                                                0.05f);
            // if(grabbedTile.transform.position == grabPoint.position+placementOffset(grabbedTile.transform)-new Vector3(0.1f,0f,0f))
            //grabbedTile.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    void repelTile(GameObject tileToMove)
    {
        tileToMove.GetComponent<Rigidbody2D>().isKinematic = true;
        

    }

    void ResetPolarity()
    {
        isActve = false;
        setPolarity(EPolarity.Neutral);
        Debug.Log("Value reset to Neutral");
    }

    IEnumerator ActivatePolarity(float resetdelay, EPolarity polarity)
    {
        setPolarity(polarity);
        isActve=true;
        yield return new WaitForSeconds(resetdelay);
        ResetPolarity();
    }

}

public enum EPolarity
    {
        Positive,
        Negative,
        Neutral
    };
