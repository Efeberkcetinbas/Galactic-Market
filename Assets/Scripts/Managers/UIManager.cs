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
    [SerializeField] private TextMeshProUGUI customerName;
    [SerializeField] private Image customerPlanetImage;
    [SerializeField] private List<Sprite> planets=new List<Sprite>();
    [SerializeField] private float typingSpeed = 0.1f; // Time between each letter

    
    

    [Header("DATA'S")]
    public GameData gameData;
    public CustomerData customerData;

    [Header("Progress")]
    [SerializeField] private List<Image> progressImages=new List<Image>();
    private int index=0;

    //Random Name Generator
    private List<string> prefixes = new List<string> { "Ka", "Al", "El", "Mar", "Ra", "Thal" };
    private List<string> middleParts = new List<string> { "le", "dor", "zon", "win", "nar", "mel" };
    private List<string> suffixes = new List<string> { "on", "el", "us", "ar", "eth", "is" };
    
    private const int maxNameLength = 10; // Max length of 10 characters

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnUIUpdate, OnUIUpdate);
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnLevelUIUpdate,OnLevelUIUpdate);
        EventManager.AddHandler(GameEvent.OnCustomerSatisfy,OnCustomerSatisfy);
        EventManager.AddHandler(GameEvent.OnUpdateRequirement, OnUpdateRequirement);
        EventManager.AddHandler(GameEvent.OnUpdateCustomerNumber,OnUpdateCustomerNumber);
        
        
    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnUIUpdate, OnUIUpdate);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnLevelUIUpdate,OnLevelUIUpdate);
        EventManager.RemoveHandler(GameEvent.OnCustomerSatisfy,OnCustomerSatisfy);
        EventManager.RemoveHandler(GameEvent.OnUpdateRequirement, OnUpdateRequirement);
        EventManager.RemoveHandler(GameEvent.OnUpdateCustomerNumber,OnUpdateCustomerNumber);
    }

    private void Start()
    {
        
        OnRestartLevel();
    }

    private void OnRestartLevel()
    {
        index=0;
    }

    private void OnNextLevel()
    {
        index=0;
        CountCustomerProgress();
    }

    private void OnUpdateCustomerNumber()
    {
        CountCustomerProgress();
    }


    private void CountCustomerProgress()
    {

        for (int i = 0; i < progressImages.Count; i++)
        {
            progressImages[i].color=Color.white;
            progressImages[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < gameData.CustomerNumber; i++)
        {
            progressImages[i].gameObject.SetActive(true);
        }
    }

    private void OnCustomerSatisfy()
    {
        progressImages[index].DOColor(Color.green,1f);
        index++;
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

        string randomName=GenerateName();
        StartCoroutine(TypeName(randomName));
        
        PlanetSelector();


    }

    private void PlanetSelector()
    {
        int randomPlanetIndex=Random.Range(0,planets.Count);
        customerPlanetImage.sprite=planets[randomPlanetIndex];
    }

    //Random Name Generator
    private string GenerateName()
    {
        string name = "";

        // Always start with a prefix
        string prefix = prefixes[Random.Range(0, prefixes.Count)];
        name += prefix;

        // Optionally include a middle part, only if it won't exceed maxNameLength
        bool includeMiddle = Random.Range(0, 2) == 0; // 50% chance
        if (includeMiddle)
        {
            string middlePart = middleParts[Random.Range(0, middleParts.Count)];
            if (name.Length + middlePart.Length <= maxNameLength)
            {
                name += middlePart;
            }
        }

        // Always include a suffix, if it won't exceed maxNameLength
        string suffix = suffixes[Random.Range(0, suffixes.Count)];
        if (name.Length + suffix.Length <= maxNameLength)
        {
            name += suffix;
        }

        return name;
    }

    private IEnumerator TypeName(string nameToDisplay)
    {
        customerName.text = ""; // Clear the text initially
        foreach (char letter in nameToDisplay)
        {
            customerName.text += letter; // Add each letter to the text
            EventManager.Broadcast(GameEvent.OnCustomerNameTyping);
            yield return new WaitForSeconds(typingSpeed); // Wait for typingSpeed before showing the next letter
        }
    }
    
}
