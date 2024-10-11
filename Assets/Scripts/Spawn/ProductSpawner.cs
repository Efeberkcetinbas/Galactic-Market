using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField] private float minX,maxX,minZ,maxZ,posY;
    [SerializeField] private Ease ease,shootingEase;
    [SerializeField] private Transform shootingMechanism,timer,shootingPoint1,shootingPoint2;
    [SerializeField] private ParticleSystem shootingDust;


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
        
        timer.DOLocalMoveY(0.25f,1f);
        yield return shootingMechanism.DOLocalMoveY(-0.4f, 1).WaitForCompletion();
        
        shootingMechanism.DOLookAt(gameData.TargetPos.position,0.1f);
        shootingDust.Play();
        for (int i = 0; i < amount; i++)
        {
            //Add ShootingMechanic a Shooting Animation
            GameObject product = pool.Get();
            // Use better solution
            product.GetComponent<ProductVariety>().RandomizeVariety();
            product.transform.position = GetRandomSpawnPosition();
            shootingMechanism.DOShakeScale(.05f,.1f);
            EventManager.Broadcast(GameEvent.OnSpawnProduct);
            Debug.Log("TATAK");
            //StartCoroutine(MoveToTarget(product, gameData.TargetPos.position));
            MoveToTargetWithDotween(product, gameData.TargetPos.position);

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

    public void MoveToTargetWithDotween(GameObject product, Vector3 targetPosition)
    {
        float speed = 5f;

        

        // Calculate the duration based on speed and distance
        float distance = Vector3.Distance(product.transform.position, targetPosition);
        float duration = distance / speed;

        product.transform.localScale=Vector3.zero;
        DOTween.Sequence()
            .Append(product.transform.DOScale(Vector3.one,duration*.5f).SetEase(Ease.OutBounce))
            .Append(product.transform.DOScale(Vector3.zero, duration / 2).SetEase(Ease.InQuad));
        
        //product.transform.DOScale(Vector3.one,0.2f).SetEase(Ease.OutBounce);
        // Move the product using DOTween
        product.transform.DOMove(targetPosition, duration)
            .SetEase(ease) // Constant speed (Linear motion)
            .OnComplete(() => 
            {
                // Once the product reaches the target, return it to the pool
                ReturnProductToPool(product);
                EventManager.Broadcast(GameEvent.OnProductHit);

                // Decrement the products in movement and check if all products have arrived
                productsInMovement--;
                if (productsInMovement <= 0)
                {
                    // All products have arrived, invoke the callback
                    onAllProductsArrived?.Invoke();
                    //StopShootingAnimation
                    shootingDust.Stop();
                    EventManager.Broadcast(GameEvent.OnShootingEnded);
                    shootingMechanism.DOLocalMoveY(-1,1).SetEase(shootingEase);
                    timer.DOLocalMoveY(.5f,.5f);
                }
            });
    }

    public void ReturnProductToPool(GameObject product)
    {
        product.SetActive(false);
    }
    private Vector3 GetRandomSpawnPosition()
    {
        // Logic to determine random spawn positions within the scene
        return new Vector3(Random.Range(shootingPoint1.position.x,shootingPoint2.position.x), posY, Random.Range(minZ, maxZ));
    }
}
