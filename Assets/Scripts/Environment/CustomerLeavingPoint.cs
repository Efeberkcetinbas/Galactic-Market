using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CustomerLeavingPoint : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> signals=new List<MeshRenderer>();
    

    private WaitForSeconds waitForSeconds;


    private void Start()
    {
        waitForSeconds=new WaitForSeconds(.5f);
    }


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
        SetSignals(Color.green,0.5f);
        StartCoroutine(SetCustomerLeaves());
    }

    private void SetSignals(Color color,float duration)
    {
        for (int i = 0; i < signals.Count; i++)
        {
            signals[i].material.DOColor(color,duration);
        }
    }

    private IEnumerator SetCustomerLeaves()
    {
        yield return waitForSeconds;
        EventManager.Broadcast(GameEvent.OnCustomerLeaves);
    }

    private void OnCustomerSpawn()
    {
        SetSignals(Color.white,0.25f);
    }

    private void OnRestartLevel()
    {
        SetSignals(Color.white,0.1f);
    }
}
