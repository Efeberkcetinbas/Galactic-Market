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
    }

    private void OnDisable() 
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);

    }


   

   

    private void OnNextLevel()
    {

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
