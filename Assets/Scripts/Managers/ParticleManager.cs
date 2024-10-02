using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> successParticles=new List<ParticleSystem>();
    [SerializeField] private List<ParticleSystem> fallingParticles=new List<ParticleSystem>();


    private void OnEnable()
    {
        /*EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);*/
        EventManager.AddHandler(GameEvent.OnCustomerLeaves, OnCustomerLeaves);

    }

    private void OnDisable()
    {
        /*EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);*/
        EventManager.RemoveHandler(GameEvent.OnCustomerLeaves, OnCustomerLeaves);
    }

    private void OnSuccess()
    {
        OpenClose(true);

        for (int i = 0; i < successParticles.Count; i++)
        {
            successParticles[i].Play();
        }
    }

    private void OpenClose(bool val)
    {
        for (int i = 0; i < successParticles.Count; i++)
        {
            successParticles[i].gameObject.SetActive(val);
        }
    }

    private void OnNextLevel()
    {
        for (int i = 0; i < successParticles.Count; i++)
        {
            successParticles[i].Stop();
        }
        OpenClose(false);
    }

    private void OnCustomerLeaves()
    {
        int randomVal=Random.Range(0,fallingParticles.Count);
        fallingParticles[randomVal].Play();
    }

}
