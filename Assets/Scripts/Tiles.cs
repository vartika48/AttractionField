

using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour
{

   // [SerializeField] string TileSpriteName;

    [SerializeField] EPolarity tilePolarity = EPolarity.Neutral;
    [SerializeField] bool isStatic;
    [SerializeField] ETileColliderType eTileColliderType;
    [SerializeField] Transform playerAttachmentPoint;
    [SerializeField] Transform tileAttachmentPoint;
    
    //[SerializeField] Vector3 collisionOffset;
    [SerializeField] float adjustedXValue;
    [SerializeField] float adjustedYValue;
    [SerializeField] bool hasAttachmentPoint;
    [SerializeField] ECustomTileType CustomTileType;
    [SerializeField] GameObject CustomTilePartner;

    //[SerializeField] GameObject tilemanager;
    BoxCollider2D tileCollider;
    Rigidbody2D rb;

    Vector3 adjustedScale;
    [SerializeField] TileData data;
    float distanceThreshold;

    Vector3 targetPosition; 

    Vector3 MoveTileAPos;
    Vector3 MoveTileBPos;


    
    Tiles otherTileRef;
    Tiles TileAvailable;

    bool isAttachedSomewhere;

    bool isRepelled;

    bool isGrabbed;
    
    bool movingToB = false;

    

    bool initTileMove = false;

    ECustomTileType otherTileType;
    
    //bool isGrabbed;

    bool availableForAttachment;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tileCollider = GetComponent<BoxCollider2D>();
        // sr = GetComponent<SpriteRenderer>();
        // sr.sprite=tilemanager.GetComponent<TilesRef> ().getSprite(TileSpriteName);

    }

    void Update()
    {
        if(initTileMove)
        {
            TileMover(otherTileType);
        }
    }

    void Start()
    {
        if(!isStatic)
        isAttachedSomewhere = false;
        else
        isAttachedSomewhere = true;

        adjustedScale = new Vector3(adjustedXValue,0f,0f);

        distanceThreshold = 1f;
        isRepelled = false;
        if(CustomTileType != ECustomTileType.None)
        {
            targetPosition = CustomTilePartner.transform.position-new Vector3(CustomTilePartner.GetComponent<Tiles>().getAdjustedXValue()/2,0,0);

        }
        
    }

    public void setInitTileMove(bool newValue, ECustomTileType newValueTileType, Transform TileA, Transform TileB)
    {
        Debug.Log("InitTileMove");
        if(!initTileMove)
        {
            initTileMove = newValue;
            otherTileType = newValueTileType;
            if(getTileColliderType()==ETileColliderType.Box)
            GetComponent<BoxCollider2D>().isTrigger = false;
            else
            GetComponent<PolygonCollider2D>().isTrigger = false;

            if(newValueTileType == ECustomTileType.BridgeHorizontal)
            {
                targetPosition = TileA.position;
                MoveTileAPos = targetPosition;
                MoveTileBPos = TileB.position;
            }
        
            else
            {
                targetPosition = TileA.position;
                MoveTileAPos = targetPosition;
                MoveTileBPos = TileB.position;
            }
        }
        
        
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

    public bool getHasAttachmentPoint()
    {
        return hasAttachmentPoint;
    }

    public bool gettIsAvailableForAttachment()
    {
        return availableForAttachment;
    }

    public void setIsStatic(bool newValue)
    {
        isStatic=newValue;
    }

    public void setIsGrabbed(bool newValue)
    {
        isGrabbed = newValue;
    }

    public Transform getTileAttachmentPoint()
    {
        return tileAttachmentPoint;
    }

    public GameObject getCustomTilePartner()
    {
        return CustomTilePartner;
;
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
    public void TileMover(ECustomTileType tileMoveType)
    {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.01f);
            Debug.Log("Current Position: " + transform.position);

            if (movingToB)
            {
                Debug.LogWarning("Moving to B");
                if (Vector3.Distance(transform.position, targetPosition) < 0.03f)
                {
                    Debug.Log("Reached B");
                    targetPosition = MoveTileAPos;
                    movingToB = false;
                }
            }
            else
            {
                Debug.LogWarning("Moving to A");
                if (Vector3.Distance(transform.position, targetPosition) < 0.03f)
                {
                    Debug.Log("Reached A");
                    targetPosition = MoveTileBPos;
                    movingToB = true;
                }
            }
    }

    // Calculate edge offset of the tile to attach to player
    private Vector3 PlacementOffset(Vector3 objectTransform)
    {
        return new Vector3((objectTransform.x / 2), 0f, 0f);
    }

    public ETileColliderType getTileColliderType()
    {
        return eTileColliderType;
    }

    public ECustomTileType getCustomTileType()
    {
        return CustomTileType;
    }

    float getAdjustedXValue()
    {
        return adjustedXValue;
    }
}

public enum ETileColliderType
    {
        Box,
        Polygon
    };

public enum ECustomTileType
{
    None,
    BridgeHorizontal,
    BridgeVertical

}
