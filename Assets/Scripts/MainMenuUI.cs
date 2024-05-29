using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    // Start is called before the first frame update
    public void startPlay()
    {   
        Debug.Log("StartPlay");
        SceneManager.LoadScene(GameManager.instance.getSceneIndex()+1);
    }
    void Start()
    {
        playButton.onClick.AddListener(startPlay);
    }
   
    
    
}
