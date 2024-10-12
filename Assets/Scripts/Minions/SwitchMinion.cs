using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMinion : MinionBase
{
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,RandomizeColor);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,RandomizeColor);
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
}
