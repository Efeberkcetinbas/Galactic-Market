using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerMinion : MinionBase
{
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,RandomizeColor);
        EventManager.AddHandler(GameEvent.OnStopTimer,OnStopTimer);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,RandomizeColor);
        EventManager.RemoveHandler(GameEvent.OnStopTimer,OnStopTimer);
    }
    public override void PerformAction()
    {
        throw new System.NotImplementedException();
    }

    public override void RandomizeColor()
    {
        int randomNumber=Random.Range(0,colors.Count);
        skinnedMeshRenderer.material.color=colors[randomNumber];
    }

    private void OnStopTimer()
    {
        animator.SetTrigger("MinionJump");
    }
}
