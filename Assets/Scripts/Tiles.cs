

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
    [SerializeField] bool hasAttachmentPoint;
    [SerializeField] ECustomTileType CustomTileType;
    [SerializeField] GameObject CustomTilePartner;

    //[SerializeField] GameObject tilemanager;
    BoxCollider2D tileCollider;
    Rigidbody2D rb;

    Vector3 adjustedScale;
    [SerializeField] TileData data;
    float distanceThreshold;


    Tiles otherTileRef;
    Tiles TileAvailable;

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

    
}

public enum ETileColliderType
    {
        Box,
        Polygon
    };

public enum ECustomTileType
{
    None,
    BridgeHorizontal
}
