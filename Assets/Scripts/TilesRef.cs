using System;
using System.Collections.Generic;
using UnityEngine;

public class TilesRef : MonoBehaviour
{

    [SerializeField]
   List<Sprite> TileSprites = new List<Sprite>();

   [SerializeField]
   List<string> TileNames = new List<string>();

    

    public Sprite getSprite(string spritename)
    {
        int index = TileNames.IndexOf(spritename);

        return TileSprites[index];
    }
   
}



