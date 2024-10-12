using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CustomerProperties : MonoBehaviour
{
    [Header("Data")]
    public CustomerData customerData;
    [SerializeField] private GameData gameData;

    

    [Header("Object Type")]

    [SerializeField] private GameObject character;
    [SerializeField] private Transform targetPos;
    [SerializeField] private GameObject product;
    [SerializeField] private Ease ease,hitEase;

    [Header("Players")]
    [SerializeField] private List<GameObject> charactersMesh;

    [Header("Animation")]
    [SerializeField] private Animator animatorWestern;
    [SerializeField] private Animator animatorPirate;

    private WaitForSeconds waitForSeconds;
    


    private void Start()
    {
        OnUpdateTargetPos();
        waitForSeconds=new WaitForSeconds(2);
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCustomerSpawn,OnCustomerSpawn);
        EventManager.AddHandler(GameEvent.OnProductHit,OnProductHit);
        EventManager.AddHandler(GameEvent.OnPressStopTimer,OnPressStopTimer);
        EventManager.AddHandler(GameEvent.OnCustomerLeaved,OnCustomerLeaved);
        EventManager.AddHandler(GameEvent.OnShootingEnded,OnShootingEnded);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCustomerSpawn,OnCustomerSpawn);
        EventManager.RemoveHandler(GameEvent.OnProductHit,OnProductHit);
        EventManager.RemoveHandler(GameEvent.OnPressStopTimer,OnPressStopTimer);
        EventManager.RemoveHandler(GameEvent.OnCustomerLeaved,OnCustomerLeaved);
        EventManager.RemoveHandler(GameEvent.OnShootingEnded,OnShootingEnded);
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
        CreateRandomCharacter();
        yield return waitForSeconds;
        EventManager.Broadcast(GameEvent.OnUpdateRequirement);
        character.transform.DOScale(Vector3.one,1f).SetEase(ease).OnComplete(()=>{
            product.transform.DOScale(Vector3.one*100,.5f).SetEase(ease);
            //EventManager.Broadcast(GameEvent.OnUpdateRequirement);

            //Allow player to use stop time
            gameData.isGivingProduct=false;
        });
    }

    private void OnProductHit()
    {
        transform.DOShakeScale(.1f,.1f,1).SetEase(hitEase);
        product.transform.DOShakeScale(.1f,20f,2).SetEase(hitEase);
    }

    private void OnPressStopTimer()
    {
        animatorWestern.SetTrigger("CollectProducts");
        animatorPirate.SetTrigger("CollectProducts");
    }

    private void OnCustomerLeaved()
    {
        animatorWestern.SetTrigger("Falling");
        animatorPirate.SetTrigger("Falling");
    }

    private void OnShootingEnded()
    {
        animatorWestern.SetTrigger("Thanks");
        animatorPirate.SetTrigger("Thanks");
    }

    private void CreateRandomCharacter()
    {
        int randomIndex=Random.Range(0,charactersMesh.Count);
        for (int i = 0; i < charactersMesh.Count; i++)
        {
            charactersMesh[i].SetActive(false);
        }
        charactersMesh[randomIndex].SetActive(true);
    }




}
