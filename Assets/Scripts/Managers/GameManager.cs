using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameData gameData;


    private WaitForSeconds waitForSeconds;


    private void Awake() 
    {
        ClearData(true);
    }

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(2);
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnFail,OnFail);

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnFail,OnFail);

    }

    
    

    private void OnNextLevel()
    {
        ClearData(false);
        
    }

    private void OnRestartLevel()
    {
        ClearData(false);
    }

   

   
    private void OnFail()
    {
        gameData.isGameEnd=true;
        StartCoroutine(OpenFail());
    }

    private void OnSuccess()
    {
        gameData.isGameEnd=true;
        StartCoroutine(OpenSuccess());
    }

    private void ClearData(bool val)
    {
        gameData.isGameEnd=val;
        gameData.CustomerNumber=0;
    }

    private IEnumerator OpenSuccess()
    {
        yield return waitForSeconds;
        OpenSuccessPanel();
    }


    private void OpenSuccessPanel()
    {
        EventManager.Broadcast(GameEvent.OnSuccessUI);
    }

    private IEnumerator OpenFail()
    {
        yield return waitForSeconds;
        OpenFailPanel();
    }


    private void OpenFailPanel()
    {
        EventManager.Broadcast(GameEvent.OnFailUI);
    }



    
}
