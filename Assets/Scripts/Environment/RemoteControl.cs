using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RemoteControl : MonoBehaviour
{
    [SerializeField] private Transform customerLeaveButton,stopTimerButton;
    [SerializeField] private float y,oldy,duration;
    [SerializeField] private Ease ease;
    [SerializeField] private GameData gameData;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStopTimer,OnStopTimer);
        EventManager.AddHandler(GameEvent.OnCustomerLeavePress,OnCustomerLeavePress);

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStopTimer,OnStopTimer);
        EventManager.RemoveHandler(GameEvent.OnCustomerLeavePress,OnCustomerLeavePress);

    }


    private void OnStopTimer()
    {
        gameData.isGivingProduct=true;
        stopTimerButton.DOLocalMoveY(y,duration).SetEase(ease).OnComplete(()=>{
            EventManager.Broadcast(GameEvent.OnPressStopTimer);
            stopTimerButton.DOLocalMoveY(oldy,duration);
        });
    }

    private void OnCustomerLeavePress()
    {
        customerLeaveButton.DOLocalMoveY(y,duration).SetEase(ease).OnComplete(()=>{
            EventManager.Broadcast(GameEvent.OnCustomerLeavingPoint);
            customerLeaveButton.DOLocalMoveY(oldy,duration);
        });
        
    }
}
