using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    [SerializeField] private GameObject product;
    [SerializeField] private Ease ease,hitEase;


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
        EventManager.AddHandler(GameEvent.OnProductHit,OnProductHit);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCustomerSpawn,OnCustomerSpawn);
        EventManager.RemoveHandler(GameEvent.OnProductHit,OnProductHit);
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
        character.transform.localScale=Vector3.zero;
        product.transform.localScale=Vector3.zero;
        yield return waitForSeconds;
        character.transform.DOScale(Vector3.one,1f).SetEase(ease).OnComplete(()=>{
            product.transform.DOScale(Vector3.one,.5f).SetEase(ease);
            EventManager.Broadcast(GameEvent.OnUpdateRequirement);

            //Allow player to use stop time
            gameData.isGivingProduct=false;
        });
    }

    private void OnProductHit()
    {
        transform.DOShakeScale(.1f,.5f,2).SetEase(hitEase);
    }




}
