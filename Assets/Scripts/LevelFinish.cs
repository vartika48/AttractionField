using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag=="Player")
        {
            Debug.Log("Level Finished");
            GameManager.instance.LevelCompleted();
        }
    }
}
