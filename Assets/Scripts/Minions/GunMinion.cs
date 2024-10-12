using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMinion : MinionBase
{
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,RandomizeColor);
        EventManager.AddHandler(GameEvent.OnShootingBegin,OnShootingBegin);
        EventManager.AddHandler(GameEvent.OnShootingEnded,OnShootingEnded);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,RandomizeColor);
        EventManager.RemoveHandler(GameEvent.OnShootingBegin,OnShootingBegin);
        EventManager.RemoveHandler(GameEvent.OnShootingEnded,OnShootingEnded);
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

    private void OnShootingBegin()
    {
        animator.SetBool("Shoot",true);
    }

    private void OnShootingEnded()
    {
        animator.SetBool("Shoot",false);
    }
}
