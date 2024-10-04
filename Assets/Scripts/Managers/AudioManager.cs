using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip GameLoop,BuffMusic;
    public AudioClip SuccessSound,SuccessUISound,RestartSound ,NextLevelSound,StartSound,FailUISound;

    //Gameplay Sounds
    [SerializeField] private AudioClip spawnProductSound,customerLeavingPointSound,stopTimerSound,customerSpawnSound,customerSatisfySound,
    customerLeavesSound,pressStopTimer;

    AudioSource musicSource,effectSource;


    private void Start() 
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = GameLoop;
        //musicSource.Play();
        effectSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable() 
    {
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.AddHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnGameStart,OnGameStart);
        //Gameplay Events

        EventManager.AddHandler(GameEvent.OnCustomerLeavingPoint,OnCustomerLeavingPoint);
        EventManager.AddHandler(GameEvent.OnCustomerLeaves,OnCustomerLeaves);
        EventManager.AddHandler(GameEvent.OnCustomerSatisfy,OnCustomerSatisfy);
        EventManager.AddHandler(GameEvent.OnCustomerSpawn,OnCustomerSpawn);
        EventManager.AddHandler(GameEvent.OnSpawnProduct,OnSpawnProduct);
        EventManager.AddHandler(GameEvent.OnStopTimer,OnStopTimer);
        EventManager.AddHandler(GameEvent.OnPressStopTimer,OnPressStopTimer);
        

    }
    private void OnDisable() 
    {
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.RemoveHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnGameStart,OnGameStart);
        //Gameplay Events

        EventManager.RemoveHandler(GameEvent.OnCustomerLeavingPoint,OnCustomerLeavingPoint);
        EventManager.RemoveHandler(GameEvent.OnCustomerLeaves,OnCustomerLeaves);
        EventManager.RemoveHandler(GameEvent.OnCustomerSatisfy,OnCustomerSatisfy);
        EventManager.RemoveHandler(GameEvent.OnCustomerSpawn,OnCustomerSpawn);
        EventManager.RemoveHandler(GameEvent.OnSpawnProduct,OnSpawnProduct);
        EventManager.RemoveHandler(GameEvent.OnStopTimer,OnStopTimer);
        EventManager.RemoveHandler(GameEvent.OnPressStopTimer,OnPressStopTimer);

    }

    
    #region GameManagement

    private void OnSuccess()
    {
        effectSource.PlayOneShot(SuccessSound);
    }

    private void OnSuccessUI()
    {
        effectSource.PlayOneShot(SuccessUISound);
    }


    private void OnRestartLevel()
    {
        effectSource.PlayOneShot(RestartSound);
    }

    private void OnNextLevel()
    {
        effectSource.PlayOneShot(NextLevelSound);
    }

    private void OnGameStart()
    {
        effectSource.PlayOneShot(StartSound);
    }

    private void OnFailUI()
    {
        effectSource.PlayOneShot(FailUISound);
    }

    #endregion
  

    #region Gameplay
    
    private void OnCustomerSatisfy()
    {
        effectSource.PlayOneShot(customerSatisfySound);
    }

    private void OnSpawnProduct()
    {
        effectSource.PlayOneShot(spawnProductSound);
    }

    private void OnStopTimer()
    {
        effectSource.PlayOneShot(stopTimerSound);
    }

    private void OnPressStopTimer()
    {
        effectSource.PlayOneShot(pressStopTimer);
    }

    private void OnCustomerLeavingPoint()
    {
        effectSource.PlayOneShot(customerLeavingPointSound);
    }

    private void OnCustomerLeaves()
    {
        effectSource.PlayOneShot(customerLeavesSound);
    }

    private void OnCustomerSpawn()
    {   
        effectSource.PlayOneShot(customerSpawnSound);
    }
    


    #endregion

}
