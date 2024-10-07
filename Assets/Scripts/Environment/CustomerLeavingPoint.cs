using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CustomerLeavingPoint : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> signals=new List<MeshRenderer>();
    [SerializeField] private List<Transform> portalOpening=new List<Transform>();
    [SerializeField] private Ease portalEase;

    

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
        SetOpening(portalOpening,true);
        StartCoroutine(SetCustomerLeaves());
    }

    private void SetSignals(Color color,float duration)
    {
        for (int i = 0; i < signals.Count; i++)
        {
            signals[i].material.DOColor(color,duration);
        }
    }

    private void SetOpening(List<Transform> lists, bool val)
    {
        if(val)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                lists[i].localScale=Vector3.zero;
                lists[i].gameObject.SetActive(val);
                lists[i].DOScale(Vector3.one,0.25f).SetEase(portalEase);
            }
        }

        else
        {
            for (int i = 0; i < lists.Count; i++)
            {
                lists[i].gameObject.SetActive(val);
            }
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
        SetOpening(portalOpening,false);
    }

    private void OnRestartLevel()
    {
        SetSignals(Color.white,0.1f);
        SetOpening(portalOpening,false);
    }
}
