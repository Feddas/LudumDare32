using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public Text CoinsValue;

    private int coinCount;

    public int CoinCount
    {
        get { return coinCount; }
        set
        {
            if (value == coinCount)
                return;

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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
