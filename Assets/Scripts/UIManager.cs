using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    GameManager gameManager = GameManager.instance;

    [SerializeField] TextMeshProUGUI playerPolarityText;
    //[SerializeField] RectTransform LevelCompleteScreen;

    private void UpdatePlayerPolarityText(EPolarity newPolarity)
    {
        playerPolarityText.text = "Player Polarity : "+newPolarity.ToString();
    }

    private void OnEnable()
    {
        GameManager.instance.OnPolarityChanged += UpdatePlayerPolarityText;
        //GameManager.instance.OnLevelCompleted += LevelCompleted;
    }

    // private void LevelCompleted()
    // {
    //     LevelCompleteScreen.gameObject.SetActive(true);
    // }

    private void Awake() 
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //LevelCompleteScreen.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
