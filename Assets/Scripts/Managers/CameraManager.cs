using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera cm;



    [Header("Shake Control")]
    [SerializeField] private float shakeTime = 0.5f;
    [SerializeField] private float amplitudeGain=1;
    [SerializeField] private float frequencyGain=1;
    [SerializeField] private float newFieldOfView;
    [SerializeField] private float oldFieldOfView;
    private CinemachineBasicMultiChannelPerlin noise;




    
    

    private void OnEnable() 
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnCustomerLeaves,OnCustomerLeaves);
        EventManager.AddHandler(GameEvent.OnCustomerSpawn,OnCustomerSpawn);
        EventManager.AddHandler(GameEvent.OnSpawnProduct,OnSpawnProduct);
    }

    private void OnDisable() 
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnCustomerLeaves,OnCustomerLeaves);
        EventManager.RemoveHandler(GameEvent.OnCustomerSpawn,OnCustomerSpawn);
        EventManager.RemoveHandler(GameEvent.OnSpawnProduct,OnSpawnProduct);

    }


   

   

    private void OnNextLevel()
    {

    }

    private void OnCustomerLeaves()
    {
        Noise(amplitudeGain,frequencyGain,shakeTime);
        ChangeFieldOfViewHit(newFieldOfView,oldFieldOfView,0.1f);
    }

    private void OnCustomerSpawn()
    {
        Noise(amplitudeGain,frequencyGain,shakeTime);
    }

    private void OnSpawnProduct()
    {
        Noise(.1f,.1f,.1f);
    }


    
    
    

    private void Start() 
    {
        noise=cm.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        if(noise == null)
            Debug.LogError("No MultiChannelPerlin on the virtual camera.", this);
        else
            Debug.Log($"Noise Component: {noise}");

        OnNextLevel();

    }

    

    private void Noise(float amplitudeGain,float frequencyGain,float shakeTime) 
    {
        noise.m_AmplitudeGain = amplitudeGain;
        noise.m_FrequencyGain = frequencyGain;
        StartCoroutine(ResetNoise(shakeTime));    
    }

    private IEnumerator ResetNoise(float duration)
    {
        yield return new WaitForSeconds(duration);
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;    
    }
    public void ChangeFieldOfView(float fieldOfView, float duration = 1)
    {
        DOTween.To(() => cm.m_Lens.FieldOfView, x => cm.m_Lens.FieldOfView = x, fieldOfView, duration);
    }

    

    public void ChangeFieldOfViewHit(float newFieldOfView, float oldFieldOfView, float duration = 1)
    {
        DOTween.To(() => cm.m_Lens.FieldOfView, x => cm.m_Lens.FieldOfView = x, newFieldOfView, duration).OnComplete(()=>{
            DOTween.To(() => cm.m_Lens.FieldOfView, x => cm.m_Lens.FieldOfView = x, oldFieldOfView, duration);
        });
    }

    public void ChangeFollow(Transform Ball)
    {
        cm.m_Follow=Ball;
    }

    public void ChangeLookAt(Transform target)
    {
        cm.m_LookAt=target;
    }

    private void ChangePriority(int val)
    {
        cm.m_Priority=val;
    }


}
