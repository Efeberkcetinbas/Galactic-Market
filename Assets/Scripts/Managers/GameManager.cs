using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameData gameData;
    public PlayerData playerData;


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

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);

    }

    
    

    private void OnNextLevel()
    {
        ClearData(false);
        
    }

    private void OnRestartLevel()
    {
        ClearData(false);
    }

    private void OnConditionSuccess()
    {
        Debug.Log("PERFECT. CONG");
        EventManager.Broadcast(GameEvent.OnSuccess);
        StartCoroutine(OpenSuccess());

        ///////////////////////////
        
        Debug.Log("END FAIL");
        EventManager.Broadcast(GameEvent.OnGameOver);
    }

   
    private void OnGameOver()
    {
        gameData.isGameEnd=true;
        StartCoroutine(OpenFail());
    }

    private void ClearData(bool val)
    {
        gameData.isGameEnd=val;
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
        //effektif
        EventManager.Broadcast(GameEvent.OnFailUI);
    }



    
}
