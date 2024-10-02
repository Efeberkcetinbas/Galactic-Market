using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RemoteControl : MonoBehaviour
{
    [SerializeField] private Transform customerLeaveButton,stopTimerButton;
    [SerializeField] private float y,oldy,duration;
    [SerializeField] private Ease ease;

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
        stopTimerButton.DOLocalMoveY(y,duration).SetEase(ease).OnComplete(()=>stopTimerButton.DOLocalMoveY(oldy,duration));
    }

    private void OnCustomerLeavePress()
    {
        customerLeaveButton.DOLocalMoveY(y,duration).SetEase(ease).OnComplete(()=>{
            EventManager.Broadcast(GameEvent.OnCustomerLeavingPoint);
            customerLeaveButton.DOLocalMoveY(oldy,duration);
        });
        
    }
}
