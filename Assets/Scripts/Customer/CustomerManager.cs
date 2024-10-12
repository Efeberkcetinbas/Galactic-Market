using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CustomerManager : MonoBehaviour
{
    public ProductSpawner productSpawner;

    // List of CustomerData assets for the current level (assign in Inspector)
    public List<CustomerData> levelCustomersData;

    // Reference to the customer GameObjects in the scene (assign in Inspector)
    public List<GameObject> customerGameObjects;

    // Effect prefab when a customer leaves (assign in Inspector)
    // Every level it can be specified hole type or particle type
    [SerializeField] private ParticleSystem EnterCustomerParticle;

    [SerializeField] private Transform successPaper,failPaper;
    

    // Private variables
     // Private variables
    private Queue<CustomerData> currentLevelCustomers = new Queue<CustomerData>();
    private CustomerData currentCustomer;
    private GameObject activeCustomerObject;
    private int currentCustomerIndex = 0; // Track the current customer index

    [SerializeField] private GameData gameData;

    


    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPressStopTimer,OnPressStopTimer);
        EventManager.AddHandler(GameEvent.OnGameStart,OnGameStart);
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPressStopTimer,OnPressStopTimer);
        EventManager.RemoveHandler(GameEvent.OnGameStart,OnGameStart);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }

    private void OnNextLevel()
    {
        OnGameStart();
    }

    private void OnRestartLevel()
    {
        currentCustomerIndex=0;
        activeCustomerObject.SetActive(false);
        currentCustomer = null;
        activeCustomerObject = null;
        OnGameStart();
    }

    private void OnGameStart()
    {
        // Setup customers for the current level from the Inspector
        SetupLevelCustomers();

        // Serve the first customer
        ServeNextCustomer();
    }

    // Setup customers for the current level
    public void SetupLevelCustomers()
    {
        currentLevelCustomers.Clear();

        foreach (CustomerData data in levelCustomersData)
        {
            currentLevelCustomers.Enqueue(data);
        }

        gameData.CustomerNumber = customerGameObjects.Count;
        EventManager.Broadcast(GameEvent.OnUpdateCustomerNumber);
    }

    // Serve the next customer
    public void ServeNextCustomer()
    {
        // If there is an active customer object, do not serve a new customer until the current one leaves
        if (activeCustomerObject != null)
        {
            return;
        }

        // Check if there are customers left to serve
        if (currentCustomerIndex < customerGameObjects.Count && currentLevelCustomers.Count > 0)
        {
            // Dequeue the next customer
            currentCustomer = currentLevelCustomers.Dequeue();

            // Activate the next customer GameObject
            activeCustomerObject = customerGameObjects[currentCustomerIndex];
            EnterCustomerParticle.Play();
            activeCustomerObject.SetActive(true);
            EventManager.Broadcast(GameEvent.OnCustomerSpawn);
            
            
            // Optionally set customer-specific properties here if needed
            Debug.Log($"Customer {currentCustomer.customerName} from {currentCustomer.planetName} has arrived!");

            // Increment the index for the next customer
            currentCustomerIndex++;
            //gameData.isGivingProduct=false;
        }
        else
        {
            Debug.Log("All customers have been served for this level!");
            EventManager.Broadcast(GameEvent.OnSuccess);
            // Optionally, proceed to the next level or show level completion UI
        }
    }

    // Handle customer request when the player stops the timer
    public bool HandleCustomerRequest(int playerInput)
    {
        if (currentCustomer == null)
            return false;

        // Check if the player's input satisfies the customer's request
        if (CheckCustomerRequest(playerInput))
        {
            Debug.Log($"Customer {currentCustomer.customerName} is satisfied!");
            CheckConclusion(successPaper);
            EventManager.Broadcast(GameEvent.OnCustomerSatisfy);
            // Start the coroutine for the customer leaving
            StartCoroutine(CustomerLeaves(activeCustomerObject));
            return true;
        }
        else
        {
            Debug.Log($"Customer {currentCustomer.customerName} is dissatisfied.");
            CheckConclusion(failPaper);
            EventManager.Broadcast(GameEvent.OnFail);
            // Optionally handle dissatisfaction (e.g., allow retry, apply penalty)
            return false;
        }
    }

    // Check if the player's input satisfies the customer's request
    // Check it is end of the product go on Player !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    private bool CheckCustomerRequest(int inputNumber)
    {
        switch (currentCustomer.detectionMethod)
        {
            case DetectionMethod.GreaterThan:
                return inputNumber > currentCustomer.targetNumber;
            case DetectionMethod.LessThan:
                return inputNumber < currentCustomer.targetNumber;
            case DetectionMethod.Between:
                return inputNumber >= currentCustomer.rangeMin && inputNumber <= currentCustomer.rangeMax;
            case DetectionMethod.MultipleOf:
                return inputNumber % currentCustomer.targetNumber == 0;
            case DetectionMethod.Exact:
                return inputNumber == currentCustomer.targetNumber;
            default:
                return false;
        }
    }

    // Coroutine to handle customer leaving with an effect
    IEnumerator CustomerLeaves(GameObject customerObject)
    {
        EventManager.Broadcast(GameEvent.OnCustomerLeavePress);
        // Wait for effect duration (adjust duration as needed)
        yield return new WaitForSeconds(2f);
        EventManager.Broadcast(GameEvent.OnCustomerLeaved);
        customerObject.transform.DOMoveY(-3,.5f).SetEase(Ease.Linear).OnComplete(()=>{

            // Deactivate the customer object
            customerObject.SetActive(false);

            //For Restart
            customerObject.transform.position=Vector3.zero;
            // Clear the current customer reference
            currentCustomer = null;

            // Clear the active customer object reference
            activeCustomerObject = null;
            
            // Proceed to the next customer
            
        });

        yield return new WaitForSeconds(1f);
        ServeNextCustomer();
    }

    private void CheckConclusion(Transform conclusion)
    {
        conclusion.DOScale(Vector3.one,0.5f).SetEase(Ease.OutElastic).OnComplete(()=>conclusion.DOScale(Vector3.zero,0.5f));
    }

    private void OnPressStopTimer()
    {
        //Will change. OnStop Timer Event adds in spawn and Goes Player
        //HandleCustomerRequest(gameData.RoundedTime);
        if(gameData.RoundedTime>0)
        {
            StartCoroutine(productSpawner.SpawnAndMoveProducts(currentCustomer.productTypes, gameData.RoundedTime, OnAllProductsArrived));
            //gameData.isGivingProduct=true;
        }
        else
        {
            EventManager.Broadcast(GameEvent.OnFail);
        }
        
    }

    private void OnAllProductsArrived()
    {
        Debug.Log("All products have been spawned.");
        HandleCustomerRequest(gameData.RoundedTime);
        
        // Perform any additional logic you want after all products are spawned
    }
}
