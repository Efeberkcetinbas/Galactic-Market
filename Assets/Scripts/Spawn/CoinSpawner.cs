using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CoinSpawner : MonoBehaviour
{
    public CoinPool coinPool;            // Reference to the CoinPool script
    public Transform spawnPoint;         // Where the coins will spawn
    public Transform coinGunTarget;      // Target position (coin gun)
    public float pullDuration = 1f;      // Duration for the coin to reach the target
    public ParticleSystem teleportEffectPrefab; // Particle system prefab for the teleport effect
    public LineRenderer teleportBeam;    // LineRenderer for the teleport beam
    private ParticleSystem currentTeleportEffect; // Reference to the current teleport effect

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCustomerLeaved,OnCustomerLeaved);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCustomerLeaved,OnCustomerLeaved);

    }

    private void OnCustomerLeaved()
    {
        SpawnCoin();
    }

    

    // Method to spawn a coin and pull it to the gun with a teleport effect
    private void SpawnCoin()
    {
        // Get a coin from the pool
        GameObject coin = coinPool.GetCoin();
        coin.transform.position=Vector3.zero;
        coin.transform.DOJump(spawnPoint.position,1,2,0.25f);
        DOTween.Sequence()
            .Append(coin.transform.DOScale(Vector3.one,pullDuration*0.5f).SetEase(Ease.OutBounce))
            .Append(coin.transform.DOScale(Vector3.zero, pullDuration*0.5f).SetEase(Ease.InQuad));
        

        // Start teleport beam and particles between the gun and the coin
        StartTeleportBeam(coin.transform.position);

        // Move the coin to the gun target (using DOTween)
        PullCoinToGun(coin);
    }

    // Method to pull the coin towards the coin gun using DOTween
    private void PullCoinToGun(GameObject coin)
    {
        // Use DOTween to move the coin to the coin gun's position
        coin.transform.DOMove(coinGunTarget.position, pullDuration)
            .SetEase(Ease.InQuad)
            .OnUpdate(() => {
                // Update the beam's end position as the coin moves
                teleportBeam.SetPosition(1, coin.transform.position);
            })
            .OnComplete(() => 
            {
                // Stop the beam and return the coin to the pool
                StopTeleportBeam();
                coinPool.ReturnCoin(coin);
                EventManager.Broadcast(GameEvent.OnCoinPulled);
            });
    }

    // Method to start teleport beam and particles between the gun and coin
    private void StartTeleportBeam(Vector3 coinPosition)
    {
        // Start the particle effect at the coin gun position
        if (currentTeleportEffect != null)
        {
            Destroy(currentTeleportEffect.gameObject); // Destroy the previous effect if it exists
        }

        currentTeleportEffect = Instantiate(teleportEffectPrefab, coinGunTarget.position, Quaternion.identity);
        currentTeleportEffect.Play();

        // Configure the LineRenderer to connect the gun and the coin
        teleportBeam.enabled = true;
        teleportBeam.SetPosition(0, coinGunTarget.position);  // Start point (gun)
        teleportBeam.SetPosition(1, coinPosition);            // End point (coin)
    }

    // Method to stop the teleport beam
    private void StopTeleportBeam()
    {
        teleportBeam.enabled = false;

        // Optionally, destroy the particle effect when finished
        if (currentTeleportEffect != null)
        {
            Destroy(currentTeleportEffect.gameObject); // Clean up the particle effect
            currentTeleportEffect = null;
        }
    }
}
