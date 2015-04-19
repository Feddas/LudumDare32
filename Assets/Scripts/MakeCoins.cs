using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MakeCoins : MonoBehaviour
{
    public float delay = 0.1f;
    public GameObject coin;

    private List<GameObject> coins = new List<GameObject>();

    void Start()
    {
        InvokeRepeating("Spawn", delay, delay);
    }

    void Spawn()
    {
        if (coins.Count > 1000)
            return;

        var position = new Vector3(Random.Range(6, 16), 20, Random.Range(4, 14));
        var newCoin = Instantiate(coin, position, Quaternion.identity) as GameObject;
        coins.Add(newCoin);
    }
}
