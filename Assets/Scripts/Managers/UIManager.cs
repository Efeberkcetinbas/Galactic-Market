using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Scene Texts")]
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Customer")]
    [SerializeField] private TextMeshProUGUI customerRequirementText;
    

    [Header("DATA'S")]
    public GameData gameData;
    public CustomerData customerData;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnUIUpdate, OnUIUpdate);
        EventManager.AddHandler(GameEvent.OnLevelUIUpdate,OnLevelUIUpdate);
        EventManager.AddHandler(GameEvent.OnUpdateRequirement, OnUpdateRequirement);
        
        
    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnUIUpdate, OnUIUpdate);
        EventManager.RemoveHandler(GameEvent.OnLevelUIUpdate,OnLevelUIUpdate);
        EventManager.RemoveHandler(GameEvent.OnUpdateRequirement, OnUpdateRequirement);
    }

    
    private void OnUIUpdate()
    {
        score.SetText(gameData.score.ToString());
        score.transform.DOScale(new Vector3(1.5f,1.5f,1.5f),0.2f).OnComplete(()=>score.transform.DOScale(new Vector3(1,1f,1f),0.2f));
    }

    private void OnLevelUIUpdate()
    {
        levelText.SetText("LEVEL " + (gameData.levelNumber+1).ToString());
    }

    private void OnUpdateRequirement()
    {
        customerData=FindObjectOfType<CustomerProperties>().customerData;
        switch (customerData.detectionMethod)
        {
            case DetectionMethod.GreaterThan:
                customerRequirementText.SetText("Greater than " + customerData.targetNumber.ToString());
                break;
            case DetectionMethod.LessThan:
                customerRequirementText.SetText("Less than " + customerData.targetNumber.ToString());
                break;
            case DetectionMethod.Between:
                customerRequirementText.SetText("Between " + customerData.rangeMin.ToString() + " - " + customerData.rangeMax.ToString());
                break;
            case DetectionMethod.MultipleOf:
                customerRequirementText.SetText("Multiple of " + customerData.targetNumber.ToString());
                break;
            case DetectionMethod.Exact:
                customerRequirementText.SetText("Exactly " + customerData.targetNumber.ToString());
                break;
            default:
                customerRequirementText.SetText("FINAL");
                break;
        }
    }
    
}
