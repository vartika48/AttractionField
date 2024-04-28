using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour
{
    [SerializeField] SpawnableTiles tileData;

    EPolarity tilePolarity;

    // Start is called before the first frame update
    void Start()
    {
        tilePolarity = tileData.eCharge;
    }

    public EPolarity getTilePolarity()
    {
        return tilePolarity;
    }

    public void setTilePolarity(EPolarity newPolarity)
    {
        tilePolarity = newPolarity;
    }
   
}
