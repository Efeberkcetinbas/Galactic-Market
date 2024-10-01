using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.UI;

//Enum ile timer type yap ilk leveller basit yavasca artarken ilerki levellerde hizla arttir.
public enum TimerTypes
{
    basicTimer,
    milliTimer,
}

public class ClockManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    private bool isStop;

    public Image progressBarFill; 

    //Timer
    [Header("TIME")]
    private float cTime;
    private int multiply;
    [SerializeField] private TextMeshProUGUI timerText;
    private TimerTypes timerTypes;
    private void Start()
    {
        OnNextLevel();
    }

    private void Update()
    {
        if(!gameData.isGameEnd)
            SetTimer();
    }

    private void SetTimer()
    {
        if(!isStop)
        {
            if(cTime<=gameData.maxTimerRange)
            {
                cTime+=Time.deltaTime*multiply;
                gameData.RoundedTime=Mathf.RoundToInt(cTime);
                timerText.SetText(gameData.RoundedTime.ToString());
            }
            else
            {
                cTime=0;
            }

            float fillAmount = Mathf.Clamp01(cTime / gameData.maxTimerRange);
            progressBarFill.fillAmount = fillAmount;

            // Update the color based on the progress
            progressBarFill.color = GetColorForProgress(fillAmount);
            
        }
        
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnStartTimer,OnStartTimer);
        EventManager.AddHandler(GameEvent.OnStopTimer,OnStopTimer);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnStartTimer,OnStartTimer);
        EventManager.RemoveHandler(GameEvent.OnStopTimer,OnStopTimer);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);

    }

    private void OnNextLevel()
    {
        CheckTimerType();
        cTime=0;
        gameData.RoundedTime=0;
        timerText.SetText(gameData.RoundedTime.ToString());
        progressBarFill.fillAmount=0;
        /*isStop=true;
        gameData.RoundedTime=0;*/

        Debug.Log(multiply);
    }

    private void OnRestartLevel()
    {
        gameData.RoundedTime=0;
        timerText.SetText(gameData.RoundedTime.ToString());
    }

    private void CheckTimerType()
    {
        switch(gameData.timerTypes)
        {
            case TimerTypes.basicTimer:
                Debug.Log("BURAYA");
                multiply=1;
                break;
            case TimerTypes.milliTimer:
                Debug.Log("ORAYA");
                //Burasi maxRange arttikca degisecek
                multiply=50;
                break;
        }
        
    }

    
    private void OnStartTimer()
    {   
        isStop=false;
    }

    

    private void OnStopTimer()
    {
        isStop=true;
        ScaleUP();
    }

    private void ScaleUP()
    {
        //timerText.transform.DOScale(Vector3.one*1.5f,.25f).OnComplete(()=>timerText.transform.DOScale(Vector3.one,.25f));
    }

    private Color GetColorForProgress(float progress)
    {
        // Define the colors
        Color green = Color.green;
        Color yellow = Color.yellow;
        Color orange = new Color(1f, 0.5f, 0f); // Orange
        Color red = Color.red;

        if (progress < 0.33f)
        {
            // From Green to Yellow (0% - 33%)
            return Color.Lerp(green, yellow, progress / 0.33f);
        }
        else if (progress < 0.66f)
        {
            // From Yellow to Orange (33% - 66%)
            return Color.Lerp(yellow, orange, (progress - 0.33f) / 0.33f);
        }
        else
        {
            // From Orange to Red (66% - 100%)
            return Color.Lerp(orange, red, (progress - 0.66f) / 0.34f);
        }
    }
}
