using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag=="Player")
        {
            Debug.Log("Level Finished");
            GameManager.instance.LevelCompleted();
            
            bool LoadSceneinit = false;

            if(!LoadSceneinit)
            {
                SceneManager.LoadScene(GameManager.instance.getSceneIndex()+1);
                LoadSceneinit = true;
            }
            

            

        }
    }
}
