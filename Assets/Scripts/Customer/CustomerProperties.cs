using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CustomerProperties : MonoBehaviour
{
    [Header("Data")]
    public CustomerData customerData;
    [SerializeField] private GameData gameData;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI planetText;

    [Header("Object Type")]

    [SerializeField] private GameObject character;
    [SerializeField] private Transform targetPos;
    [SerializeField] private GameObject productType;
    [SerializeField] private Ease ease;


    private WaitForSeconds waitForSeconds;


    private void Start()
    {
        OnUpdateUI();
        OnUpdateTargetPos();
        waitForSeconds=new WaitForSeconds(2);
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCustomerSpawn,OnCustomerSpawn);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCustomerSpawn,OnCustomerSpawn);
    }
    private void OnUpdateUI()
    {
        nameText.SetText(customerData.customerName);
        planetText.SetText(customerData.planetName);
    }

    private void OnUpdateTargetPos()
    {
        gameData.TargetPos=targetPos;
    }

    private void OnCustomerSpawn()
    {
        StartCoroutine(Create());
    }

    private IEnumerator Create()
    {
        yield return waitForSeconds;
        character.transform.DOScale(Vector3.one,0.25f).SetEase(ease).OnComplete(()=>{
            productType.transform.DOScale(Vector3.one,.5f).SetEase(ease);
            EventManager.Broadcast(GameEvent.OnUpdateRequirement);
        });
    }


}