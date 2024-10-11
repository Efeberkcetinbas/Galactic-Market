using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    public GameObject coinPrefab;       // Prefab for the coin
    public int poolSize = 20;           // Initial size of the pool
    private Queue<GameObject> coinPool; // Queue for object pooling

    private void Awake()
    {
        InitializePool();
    }

    // Initialize the pool with a certain number of coin objects
    private void InitializePool()
    {
        coinPool = new Queue<GameObject>();

        // Create initial coin objects and disable them
        for (int i = 0; i < poolSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.SetActive(false);
            coinPool.Enqueue(coin);  // Add coin to the pool
        }
    }

    // Method to get a coin from the pool
    public GameObject GetCoin()
    {
        if (coinPool.Count > 0)
        {
            GameObject coin = coinPool.Dequeue();
            coin.SetActive(true);  // Enable the coin
            return coin;
        }
        else
        {
            // Optionally instantiate a new coin if the pool is empty
            GameObject newCoin = Instantiate(coinPrefab);
            return newCoin;
        }
    }

    // Method to return a coin to the pool
    public void ReturnCoin(GameObject coin)
    {
        coin.SetActive(false);   // Disable the coin
        coinPool.Enqueue(coin);  // Add it back to the pool
    }
}
