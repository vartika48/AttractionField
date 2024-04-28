using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour, ITiles
{

    [SerializeField] EPolarity tilePolarity;

    // Start is called before the first frame update

    public EPolarity getTilePolarity()
    {
        return tilePolarity;
    }

    public void setTilePolarity(EPolarity newPolarity)
    {
        tilePolarity = newPolarity;
    }

    public void attractTile()
    {

    }
   public void repelTile()
   {
    
   }
}

interface ITiles 
{
    public void attractTile();

    public void repelTile();
}
