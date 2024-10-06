using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RemoteControl : MonoBehaviour
{
    [SerializeField] private Transform customerLeaveButton,stopTimerButton;
    [SerializeField] private float stopTimerY,stopTimerOldY,duration,customerY,oldCustomerY;
    [SerializeField] private Ease ease;
    [SerializeField] private GameData gameData;
    [SerializeField] private ParticleSystem timerParticle,customerParticle;

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
        stopTimerButton.DOLocalMoveY(stopTimerY,duration).SetEase(ease).OnComplete(()=>{
            timerParticle.Play();
            EventManager.Broadcast(GameEvent.OnPressStopTimer);
            stopTimerButton.DOLocalMoveY(stopTimerOldY,duration);
        });
    }

    private void OnCustomerLeavePress()
    {
        customerLeaveButton.DOLocalRotate(new Vector3(customerY,0,0),duration).SetEase(ease).OnComplete(()=>{
            customerParticle.Play();
            EventManager.Broadcast(GameEvent.OnCustomerLeavingPoint);
            customerLeaveButton.DOLocalRotate(new Vector3(oldCustomerY,0,0),duration);
        });
        
    }
}
