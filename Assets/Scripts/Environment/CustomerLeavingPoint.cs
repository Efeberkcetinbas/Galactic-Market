using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CustomerLeavingPoint : MonoBehaviour
{
    //If I cant find available hole animation. TEMP !!!!!!!!!!!!!!!!!!!!!!!!!
    [SerializeField] private Transform leftDoor,rightDoor,hole;
    [SerializeField] private Ease doorEase;


    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCustomerLeavingPoint,OnCustomerLeavingPoint);
        EventManager.AddHandler(GameEvent.OnCustomerSpawn,OnCustomerSpawn);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCustomerLeavingPoint,OnCustomerLeavingPoint);
        EventManager.RemoveHandler(GameEvent.OnCustomerSpawn,OnCustomerSpawn);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);

    }


    private void OnCustomerLeavingPoint()
    {
        rightDoor.DOLocalMoveX(-1,0.5f).SetEase(doorEase);
        leftDoor.DOLocalMoveX(1,0.5f).SetEase(doorEase).OnComplete(()=>{
            hole.gameObject.SetActive(true);
            EventManager.Broadcast(GameEvent.OnCustomerLeaves);
        });
    }

    private void OnCustomerSpawn()
    {
        hole.gameObject.SetActive(false);
        rightDoor.DOLocalMoveX(0,0.5f).SetEase(doorEase);
        leftDoor.DOLocalMoveX(0,0.5f).SetEase(doorEase);
    }

    private void OnRestartLevel()
    {
        hole.gameObject.SetActive(false);
        rightDoor.localPosition=new Vector3(0,0,0);
        leftDoor.localPosition=new Vector3(0,0,0);
    }
}
