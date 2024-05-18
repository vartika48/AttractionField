using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypeD : MonoBehaviour
{
    // List to hold sprite names
    [SerializeField]
    private List<string> spriteNames = new List<string>();

    // List to hold sprites
    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();

    // Method to add sprite with a name to the lists
    public void AddSprite(string name, Sprite sprite)
    {
        spriteNames.Add(name);
        sprites.Add(sprite);
    }

    // Method to assign sprite to the prefab using the name
    public void AssignSpriteToPrefab(string name, GameObject prefab)
    {
        // Find the index of the sprite name in the list
        int index = spriteNames.IndexOf(name);
        
        if (index != -1)
        {
            // Check if the index is valid
            if (index < sprites.Count)
            {
                // Get the sprite from the list using the index
                Sprite sprite = sprites[index];
                
                // Assign the sprite to the prefab
                prefab.GetComponent<SpriteRenderer>().sprite = sprite;
            }
            else
            {
                Debug.LogWarning("Index out of range for sprite: " + name);
            }
        }
        else
        {
            Debug.LogWarning("Sprite with name '" + name + "' not found.");
        }
    }
}
