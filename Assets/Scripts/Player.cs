

using System.Collections;
using Unity.VisualScripting;

using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] float playerSpeed;         //player speed
    [SerializeField] float jumpPower;           //jump power
    [SerializeField] Transform groundCheck;     //ground check point on Player 
    [SerializeField] LayerMask groundLayer;     //Layermask ref for ground layer
    [SerializeField] Transform grabPoint;       //transform point where player will grab the tiles
    [SerializeField] Transform rayPoint;        //Raycast Emittor point
    [SerializeField] float polarityResetTime;  //Player polarity reset time
    [SerializeField] float minInclusiveRepel;   //minimum inclusive repel distance
    [SerializeField] float maxExclusiveRepel;   //maximum exclusive repel distance

    [SerializeField] float searchRadius;

    float rayDistance;
    private GameObject grabbedTile;             // reference for grabbed 
    
    //private GameObject tileToBridge;
    private int layerIndex;
    bool isPolarityTimerActive;                 // bool to check if reset polarity timer is active               
    Vector3 randomRepelPoint; 

    float disCheck;                  

    bool isRepelled;                            //is tile has reached the random repel point

    RaycastHit2D hitInfo;

    private float horizontal;
    private bool isFacingRight = true;          // bool to check if player facing right

    EPolarity playerPolarity;                   // player polarity 

    Rigidbody2D rb;

    Tiles HitTileRef;

    //Tiles TileToAttachRef;

    Vector3 collisionHandle = new Vector3(0.1f,0f,0f);

    Tiles tempClosestTile = null;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() 
    {
        isPolarityTimerActive = false;
        isRepelled = false;
        layerIndex = LayerMask.NameToLayer("Ground");
        Debug.Log(layerIndex);
        playerPolarity = EPolarity.Negative;
        disCheck = 0f;
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

        HandleTiles();

        HandleMovement();
        
        GrabTiles();
        

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(playerPolarity==EPolarity.Positive || playerPolarity==EPolarity.Neutral)
            {
                setPolarity(EPolarity.Negative);
            }
            else if(playerPolarity==EPolarity.Negative)
            {
                setPolarity(EPolarity.Positive);
            }
            Debug.Log("Player Polarity = "+playerPolarity);
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

    // Get Tile position for calculating direction of tile in context to player position
    Vector3 getTouchPosition()
    {
        
        Vector3 touchPostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchPostion.z=0f;

        return touchPostion;
    }

    // calculate edge offset of the tile to attach to player
    Vector3 placementOffset(Vector3 objectTransform)
    {
        return new Vector3((objectTransform.x/2),0f,0f);
    }

    // Attract tile to player functionality 
    void attractTile(GameObject tileToMove, Vector3 placementOffsetAdjusted, Tiles tileCompRef)
    {
        tileToMove.GetComponent<Rigidbody2D>().isKinematic = true;
        if(transform.position.x < grabPoint.position.x)
        {
            if(tileCompRef.getTileColliderType()==ETileColliderType.Box)
            tileToMove.GetComponent<BoxCollider2D>().isTrigger = true;
            else
            tileToMove.GetComponent<PolygonCollider2D>().isTrigger = true;

            tileToMove.transform.position = Vector3.MoveTowards(tileToMove.transform.position, 
                                                                grabPoint.position+placementOffset(placementOffsetAdjusted)+collisionHandle,
                                                                0.05f);
            //grabbedTile.transform.position = grabPoint.position+placementOffset(grabbedTile.transform)+new Vector3(0.1f,0f,0f);
        }
        else
        {
            if(tileCompRef.getTileColliderType()==ETileColliderType.Box)
            tileToMove.GetComponent<BoxCollider2D>().isTrigger = true;
            else
            tileToMove.GetComponent<PolygonCollider2D>().isTrigger = true;

            tileToMove.transform.position = Vector3.MoveTowards(tileToMove.transform.position, 
                                                                grabPoint.position-placementOffset(placementOffsetAdjusted)-collisionHandle,
                                                                0.05f);
            // if(grabbedTile.transform.position == grabPoint.position+placementOffset(grabbedTile.transform)-new Vector3(0.1f,0f,0f))
            //grabbedTile.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    void attractTileToStatic(GameObject tileToMove, Vector3 placementOffsetAdjusted,GameObject otherTile, Tiles tileToMoveCompRef)
    {
        tileToMove.GetComponent<Rigidbody2D>().isKinematic = true;
        if(tileToMove.transform.position.x < otherTile.gameObject.transform.position.x)
        {
            if(tileToMoveCompRef.getTileColliderType()==ETileColliderType.Box)
            tileToMove.GetComponent<BoxCollider2D>().isTrigger = true;
            else
            tileToMove.GetComponent<PolygonCollider2D>().isTrigger = true;

            tileToMove.transform.position = Vector3.MoveTowards(tileToMove.transform.position, 
                                                                otherTile.GetComponent<Tiles>().getTileAttachmentPoint().position-placementOffset(placementOffsetAdjusted)+collisionHandle,
                                                                0.05f);
            //grabbedTile.transform.position = grabPoint.position+placementOffset(grabbedTile.transform)+new Vector3(0.1f,0f,0f);
        }
        else
        {
            if(tileToMoveCompRef.getTileColliderType()==ETileColliderType.Box)
            tileToMove.GetComponent<BoxCollider2D>().isTrigger = true;
            else
            tileToMove.GetComponent<PolygonCollider2D>().isTrigger = true;

            tileToMove.transform.position = Vector3.MoveTowards(tileToMove.transform.position, 
                                                                otherTile.GetComponent<Tiles>().getTileAttachmentPoint().position+collisionHandle,
                                                                0.05f);
            // if(grabbedTile.transform.position == grabPoint.position+placementOffset(grabbedTile.transform)-new Vector3(0.1f,0f,0f))
            //grabbedTile.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    // Repel tile from player functionality
    void repelTile(GameObject tileToMove, Vector3 pointToMove)
    {
        tileToMove.GetComponent<Rigidbody2D>().isKinematic = true;
        if(tileToMove.transform.position != (transform.position+pointToMove))
        {
            isRepelled=true;
            tileToMove.transform.position = Vector3.MoveTowards(grabbedTile.transform.position, 
                                                                transform.position+pointToMove,
                                                                0.05f);
            HitTileRef.setIsRepelled(true);
        }
        else
        {
            HitTileRef.setIsRepelled(false);
            grabbedTile = null;
            isRepelled = false;
            HitTileRef = null;
        }
    }

    // reset player polarity to neutral
    void ResetPolarity()
    {
        isPolarityTimerActive = false;
        setPolarity(EPolarity.Neutral);
        Debug.Log("Value reset to Neutral");
    }

    //Coroutine to activate reset player polarity timer and assign polarity to player
    IEnumerator ActivatePolarity(float resetdelay)
    {
        //setPolarity(polarity);
        isPolarityTimerActive=true;
        yield return new WaitForSeconds(resetdelay);
        ResetPolarity(); //Work later if needed

    }

    void HandleTiles()
    {
        if(grabbedTile !=null)
        {
            if(playerPolarity!=EPolarity.Neutral)
            {
                if(HitTileRef.getTilePolarity()!=playerPolarity)
                {
                    attractTile(grabbedTile, grabbedTile.GetComponent<Tiles>().getAdjustedScale(), HitTileRef);
                }
                else if(HitTileRef.getTilePolarity()==playerPolarity)
                {
                    Debug.LogWarning("Repel + "+tempClosestTile);
                    if(tempClosestTile == null && grabbedTile)
                    {   
                        Debug.LogWarning("Temp Closest tile is null");
                        tempClosestTile = findClosestOppositePolarityStaticTile();
                    }
                    if(tempClosestTile)
                    {
                        Debug.Log("Temp Closed Tile Not Null"+tempClosestTile);
                        attractTileToStatic(grabbedTile,tempClosestTile.getAdjustedScale(),tempClosestTile.GameObject(),HitTileRef);

                        Vector3 tempGrabbedTilePos = (grabbedTile.transform.position+placementOffset(HitTileRef.getAdjustedScale()));

                        Vector3 tempStaticTilePos = (tempClosestTile.gameObject.transform.position-placementOffset(tempClosestTile.getAdjustedScale()));

                        //Debug.LogWarning("Grabbed Tile Position = "+tempGrabbedTilePos);
                        //Debug.Log("Static Tile Position = "+tempStaticTilePos);
                        


                        disCheck = Vector3.Distance(tempGrabbedTilePos, tempStaticTilePos);

                        Debug.Log("DisCHECK = "+disCheck);
                        if(disCheck < 1)
                        {
                            Debug.Log("Positions are Equal");

                            if(HitTileRef.getTileColliderType()==ETileColliderType.Box)
                            grabbedTile.GetComponent<BoxCollider2D>().isTrigger = false;
                            else
                            grabbedTile.GetComponent<PolygonCollider2D>().isTrigger = false;

                            grabbedTile=null;
                            Debug.LogWarning("grabbedTile Nulled");
                            tempClosestTile =null;
                            HitTileRef = null;
                            
                        }
                    }
                    else
                    {
                        if (!isRepelled)
                        {
                            randomRepelPoint = new(Random.Range(3f,6f), Random.Range(3f,6f), 0f);
                            Debug.Log("Random Location of Repel : "+randomRepelPoint);
                        }
                        repelTile(grabbedTile, randomRepelPoint);
                    }
                }
            }
            else
            {

            }
        }
    }

    void HandleMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();

        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }
    void GrabTiles()
    {
        if(Input.GetMouseButtonDown(0))
        {   
            Vector3 touchPos = getTouchPosition();

            Debug.Log(touchPos);

            if(!grabbedTile)
            {

                Debug.Log("Grabbed Tile is NULL");
                hitInfo = Physics2D.Raycast(rayPoint.position, touchPos-rayPoint.transform.position, 10f, groundLayer);
                


                HitTileRef = hitInfo.collider.gameObject.GetComponent<Tiles>();

                Debug.Log(HitTileRef.gameObject);

                if(hitInfo)
                {
                    if(hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex && !HitTileRef.getIsStatic())  
                    {

                        if (HitTileRef.getTilePolarity() != EPolarity.Neutral)
                        {
                            if(HitTileRef.getTilePolarity() != playerPolarity)
                            {
                                grabbedTile = hitInfo.collider.gameObject;
                                HitTileRef.setIsGrabbed(true);
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("No Tile Found !!");
                }
                

            }
            else
            Debug.Log("Grabbed Tile is Not Null");
        }
    }

    Tiles findClosestOppositePolarityStaticTile()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(grabbedTile.transform.position, searchRadius);
        Tiles closestTile = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            if(collider.gameObject.layer == layerIndex)
            {
                Tiles tile = collider.GetComponent<Tiles>();
                Debug.Log("Tiles Found = "+tile.gameObject);

                if(tile.getTilePolarity() != EPolarity.Neutral)
                {
                    if (tile != null && tile.getIsStatic() && HitTileRef.getTilePolarity() != tile.getTilePolarity() )
                    {
                        float distance = Vector2.Distance(grabbedTile.transform.position, tile.transform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestTile = tile;
                        }
                    }
                }
                
            }
            
        }
        Debug.Log("Closest Tile Returned = "+closestTile);
        return closestTile;
    }

    

}
public enum EPolarity
    {
        Positive,
        Negative,
        Neutral
    };
