using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameManager gameManager = GameManager.instance;

    [SerializeField] private TextMeshProUGUI playerPolarityText;

    private void UpdatePlayerPolarityText(EPolarity newPolarity)
    {
        playerPolarityText.text = "Player Polarity : "+newPolarity.ToString();
    }

    private void OnEnable()
    {
        GameManager.instance.OnPolarityChanged += UpdatePlayerPolarityText;
    }

    private void Awake() 
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
