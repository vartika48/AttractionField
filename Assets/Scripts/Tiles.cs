using UnityEngine;

public class Tiles : MonoBehaviour
{
    [SerializeField] EPolarity tilePolarity;
    [SerializeField] bool isStatic;
    [SerializeField] Transform playerAttachmentPoint;
    [SerializeField] Transform tileAttachmentPoint;

    BoxCollider2D tileCollider;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tileCollider = GetComponent<BoxCollider2D>();
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

    // Attract tile to player functionality 
    public void AttractTile(Transform grabPoint, Vector3 collisionHandle)
    {
        rb.isKinematic = true;
        if (transform.position.x < grabPoint.position.x)
        {
            tileCollider.isTrigger = true;
            transform.position = Vector3.MoveTowards(transform.position,
                                                      grabPoint.position + PlacementOffset(transform) + collisionHandle,
                                                      0.05f);
        }
        else
        {
            tileCollider.isTrigger = true;
            transform.position = Vector3.MoveTowards(transform.position,
                                                      grabPoint.position - PlacementOffset(transform) - collisionHandle,
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
    private Vector3 PlacementOffset(Transform objectTransform)
    {
        return new Vector3((objectTransform.localScale.x / 2), 0f, 0f);
    }
}
