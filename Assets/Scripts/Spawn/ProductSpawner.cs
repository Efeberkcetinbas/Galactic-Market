using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ProductPool
    {
        public ProductTypes productType;
        public GameObject productPrefab;
        public int poolSize;
    }

    public ProductPool[] productPools; // Array to hold different product pools
    private Dictionary<ProductTypes, ObjectPool> productPoolDictionary;

    public float spawnInterval = 1f; // Time interval between spawns
    [SerializeField] private GameData gameData;
    [SerializeField] private float minX,maxX,minZ,maxZ;

    private int productsInMovement; // Count the products in movement
    private System.Action onAllProductsArrived; // Store the callback

    private void Start()
    {
        productPoolDictionary = new Dictionary<ProductTypes, ObjectPool>();

        foreach (var pool in productPools)
        {
            if (pool.productPrefab == null)
            {
                Debug.LogError("Product prefab missing for: " + pool.productType);
                continue;
            }

            ObjectPool newPool = new ObjectPool(pool.productPrefab, pool.poolSize, transform);
            productPoolDictionary.Add(pool.productType, newPool);

            Debug.Log("Initialized pool for: " + pool.productType + " with size: " + pool.poolSize);
        }
    }

    public IEnumerator SpawnAndMoveProducts(ProductTypes productType, int amount, System.Action onSpawnComplete)
    {
        if (!productPoolDictionary.ContainsKey(productType))
        {
            Debug.LogWarning("No pool found for product type: " + productType);
            yield break;
        }

        ObjectPool pool = productPoolDictionary[productType];
        productsInMovement = amount; // Track the number of products to move
        onAllProductsArrived = onSpawnComplete; // Store the callback

        for (int i = 0; i < amount; i++)
        {
            GameObject product = pool.Get();
            // Use better solution
            product.GetComponent<ProductVariety>().RandomizeVariety();
            product.transform.position = GetRandomSpawnPosition();
            Debug.Log("TATAK");
            StartCoroutine(MoveToTarget(product, gameData.TargetPos.position));

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Coroutine to move the product to the target
    private IEnumerator MoveToTarget(GameObject product, Vector3 targetPosition)
    {
        float speed = 5f;

        while (Vector3.Distance(product.transform.position, targetPosition) > 0.1f)
        {
            product.transform.position = Vector3.MoveTowards(product.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        // Once the product reaches the target, return it to the pool
        ReturnProductToPool(product);

        // Decrement the products in movement and check if all products have arrived
        productsInMovement--;
        if (productsInMovement <= 0)
        {
            // All products have arrived, invoke the callback
            onAllProductsArrived?.Invoke();
        }
    }

    public void ReturnProductToPool(GameObject product)
    {
        product.SetActive(false);
    }
    private Vector3 GetRandomSpawnPosition()
    {
        // Logic to determine random spawn positions within the scene
        return new Vector3(Random.Range(minX, maxX), 1, Random.Range(minZ, maxZ));
    }
}
