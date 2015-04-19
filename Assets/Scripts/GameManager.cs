using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public Text CoinsValue;
    public TextFader TextFader;
    public int CoinsToWin = 100;
    private int coinCount;

    public int CoinCount
    {
        get { return coinCount; }
        set
        {
            if (value == coinCount)
                return;

            if (value == 0)
            {
                TextFader.ShowText("You won! Next you're sure to find an unconventional way for weapons to cure cancer.");
                this.Delay(2, () => Time.timeScale = 0);
            }

            coinCount = value;
            CoinsValue.text = value.ToString();

        }
    }

    public static GameManager Instance { get; set; }
    
    void Start()
    {
        if (Instance != null)
            throw new Exception("Can not have more than one GameManager");

        Instance = this;
        CoinCount = CoinsToWin;

        TextFader.ShowText("Use your unconventional handgun car engine to collect " + CoinCount + " cubes.");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OverlayText(string textToOverlay)
    {
        TextFader.ShowText(textToOverlay);
    }
}
