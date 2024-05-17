

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour
{

   // [SerializeField] string TileSpriteName;

    [SerializeField] EPolarity tilePolarity;
    [SerializeField] bool isStatic;
    [SerializeField] Transform playerAttachmentPoint;
    [SerializeField] Transform tileAttachmentPoint;
    
    [SerializeField] Vector3 collisionOffset;

    [SerializeField] float adjustedXValue;

    //[SerializeField] GameObject tilemanager;

    BoxCollider2D tileCollider;
    Rigidbody2D rb;

    Vector3 adjustedScale;

    [SerializeField] TileData data;

    float distanceThreshold;

    //SpriteRenderer sr;

    Tiles otherTileRef;
    Tiles TileAvailable;

    CircleCollider2D MegneticField;

    bool isAttachedSomewhere;

    bool isRepelled;

    bool isGrabbed;

    
    //bool isGrabbed;

    bool availableForAttachment;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tileCollider = GetComponent<BoxCollider2D>();
        // sr = GetComponent<SpriteRenderer>();
        // sr.sprite=tilemanager.GetComponent<TilesRef> ().getSprite(TileSpriteName);
        MegneticField = GetComponent<CircleCollider2D>();

    }

    void Start()
    {
        if(!isStatic)
        isAttachedSomewhere = false;
        else
        isAttachedSomewhere = true;
        collisionOffset = new Vector3(0.1f,0.1f,0);

        adjustedScale = new Vector3(adjustedXValue,0f,0f);

        distanceThreshold = 1f;
        isRepelled = false;
    }

    public EPolarity getTilePolarity()
    {
        return tilePolarity;
    }

    public void setTilePolarity(EPolarity newPolarity)
    {
        tilePolarity = newPolarity;
    }

    public bool getIsStatic()
    {
        return isStatic;
    }

    public bool getIsRepelled()
    {
        return isRepelled;
    }

    public void setIsRepelled(bool newValue)
    {
        isRepelled = newValue;
    }

    public Vector3 getAdjustedScale()
    {
        return adjustedScale;
    }

    // public void setIsAvailableForAttachment(bool available)
    // {
    //     availableForAttachment = available;
    // }

    public bool gettIsAvailableForAttachment()
    {
        return availableForAttachment;
    }

    public void setIsGrabbed(bool newValue)
    {
        isGrabbed = newValue;
    }

    public Transform getTileAttachmentPoint()
    {
        return tileAttachmentPoint;
    }

    // Attract tile to player functionality 
    public void AttractTile(Transform grabPoint, Vector3 collisionHandle)
    {
        rb.isKinematic = true;
        if (transform.position.x < grabPoint.position.x)
        {
            tileCollider.isTrigger = true;
            transform.position = Vector3.MoveTowards(transform.position,
                                                      grabPoint.position + PlacementOffset(adjustedScale) + collisionHandle,
                                                      0.05f);
        }
        else
        {
            tileCollider.isTrigger = true;
            transform.position = Vector3.MoveTowards(transform.position,
                                                      grabPoint.position - PlacementOffset(adjustedScale) - collisionHandle,
                                                      0.05f);
        }
    }

    // Repel tile from player functionality
    public void RepelTile(Vector3 pointToMove)
    {
        rb.isKinematic = true;
        if (transform.position != (transform.position + pointToMove))
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                                      transform.position + pointToMove,
                                                      0.05f);
        }
    }

    // Calculate edge offset of the tile to attach to player
    private Vector3 PlacementOffset(Vector3 objectTransform)
    {
        return new Vector3((objectTransform.x / 2), 0f, 0f);
    }

    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // if(IsCollisionWithCircleCollider(other) && isGrabbed)
        // {   
        //     Debug.LogWarning("Other Collider is Circle");
        //     Collider2D temp = null;
        //     if (other == temp || temp != null)
        //     {
        //         Debug.LogWarning("Everything is Shit");
        //     }
        //     else
        //     {
        //         temp = other;
        //         Debug.Log(temp);
        //         otherTileRef = other.gameObject.GetComponent<Tiles>();
        //         Debug.Log(other.gameObject);  
        //         if(otherTileRef.getTilePolarity() != tilePolarity && !otherTileRef.getIsStatic() && !isAttachedSomewhere && isRepelled)
        //         {
        //             availableForAttachment = true;
        //             TileAvailable = otherTileRef;

        //             Debug.LogWarning(TileAvailable);
            
        //             Transform moveTo = otherTileRef.getTileAttachmentPoint();
        //             Transform tempTransform;
        //                 if(transform.position.x < moveTo.position.x )
        //                 {
        //                     tempTransform = transform;
        //                     tempTransform.position = transform.position + PlacementOffset(adjustedScale);
        //                 }
        //                 else
        //                 {
        //                     tempTransform = transform;
        //                     tempTransform.position = transform.position - PlacementOffset(adjustedScale);
        //                 }
        //                 float tempDistance = Vector3.Distance(moveTo.position,tempTransform.position);
        //                 while(tempDistance>distanceThreshold)
        //                 {
        //                 AttractTile(moveTo,collisionOffset);
        //                 tempDistance = Vector3.Distance(moveTo.position,tempTransform.position);

        //                 }
            

        //         }
        //     }
        // }
        
               
    }


    private void OnTriggerExit2D(Collider2D other) 
    {
        if(IsCollisionWithCircleCollider(other))
        {
            TileAvailable = null;
            Debug.LogWarning(TileAvailable);
        }
        
    }
    private bool IsCollisionWithCircleCollider(Collider2D other)
    {
        // Create a temporary object to simulate the collision from the CircleCollider2D
        GameObject temp = new GameObject();
        Collider2D tempCollider = temp.AddComponent<CircleCollider2D>();

        // Move the temporary object to the position of the other collider
        temp.transform.position = other.transform.position;

        // Check if there is any overlap between the CircleCollider2D and the other collider
        bool isCollisionWithCircle = Physics2D.IsTouching(tempCollider, MegneticField);

        // Destroy the temporary object to clean up
        Destroy(temp);

        return isCollisionWithCircle;
    }

    
}
