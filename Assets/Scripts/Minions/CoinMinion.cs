using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMinion : MinionBase
{
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,RandomizeColor);
        EventManager.AddHandler(GameEvent.OnCoinPulled,OnCoinPulled);
        EventManager.AddHandler(GameEvent.OnCustomerLeaved,OnCustomerLeaved);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,RandomizeColor);
        EventManager.RemoveHandler(GameEvent.OnCoinPulled,OnCoinPulled);
        EventManager.RemoveHandler(GameEvent.OnCustomerLeaved,OnCustomerLeaved);
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

    private void OnCustomerLeaved()
    {
        animator.SetBool("CoinPull",true);
    }

    private void OnCoinPulled()
    {
        animator.SetBool("CoinPull",false);
    }
}
