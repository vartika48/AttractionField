using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] TextMeshProUGUI playerPolarityText;
    [SerializeField] RectTransform LevelCompleteScreen;



    private void Awake() 
    {
        gameManager = GameManager.instance;
        if (gameManager != null)
        {
            gameManager.OnPolarityChanged += UpdatePlayerPolarityText;
            gameManager.OnLevelCompleted += LevelCompleted;
        }
        else
        {
            Debug.LogError("GameManager is not assigned in OnEnable.");
        }
    }
    private void UpdatePlayerPolarityText(EPolarity newPolarity)
    {
        Debug.Log("Player Polarity Change");
        playerPolarityText.text = "Player Polarity : "+newPolarity.ToString();
    }

    private void OnEnable()
    {
        if(gameManager == null)
        {
            gameManager.OnPolarityChanged += UpdatePlayerPolarityText;
            gameManager.OnLevelCompleted += LevelCompleted;
        }
        else
        {
            Debug.Log("Game Manager is Null");
        }
        
    }

    // private void OnDisable() 
    // {
    //     if(gameManager == null)
    //     {
    //         gameManager.OnPolarityChanged -= UpdatePlayerPolarityText;
    //         gameManager.OnLevelCompleted -= LevelCompleted;
    //     }
    //     else
    //     {
    //         Debug.Log("Game Manager is Null");
    //     }
    // }

    private void LevelCompleted()
    {
        LevelCompleteScreen.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        LevelCompleteScreen.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
