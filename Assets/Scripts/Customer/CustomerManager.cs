using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    // List of CustomerData assets for the current level (assign in Inspector)
    public List<CustomerData> levelCustomersData;

    // Reference to the customer GameObjects in the scene (assign in Inspector)
    public List<GameObject> customerGameObjects;

    // Effect prefab when a customer leaves (assign in Inspector)
    public GameObject customerEffect;

    // Private variables
     // Private variables
    private Queue<CustomerData> currentLevelCustomers = new Queue<CustomerData>();
    private CustomerData currentCustomer;
    private GameObject activeCustomerObject;
    private int currentCustomerIndex = 0; // Track the current customer index

    [SerializeField] private GameData gameData;

    void Start()
    {
        // Setup customers for the current level from the Inspector
        SetupLevelCustomers();

        // Serve the first customer
        ServeNextCustomer();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStopTimer,OnStopTimer);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStopTimer,OnStopTimer);
    }

    // Setup customers for the current level
    public void SetupLevelCustomers()
    {
        currentLevelCustomers.Clear();

        foreach (CustomerData data in levelCustomersData)
        {
            currentLevelCustomers.Enqueue(data);
        }
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
            activeCustomerObject.SetActive(true);
            
            // Optionally set customer-specific properties here if needed
            Debug.Log($"Customer {currentCustomer.customerName} from {currentCustomer.planetName} has arrived!");

            // Increment the index for the next customer
            currentCustomerIndex++;
        }
        else
        {
            Debug.Log("All customers have been served for this level!");
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
            // Start the coroutine for the customer leaving
            StartCoroutine(CustomerLeaves(activeCustomerObject));
            return true;
        }
        else
        {
            Debug.Log($"Customer {currentCustomer.customerName} is dissatisfied.");
            // Optionally handle dissatisfaction (e.g., allow retry, apply penalty)
            return false;
        }
    }

    // Check if the player's input satisfies the customer's request
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
        // Play the customer leaving effect once
        Instantiate(customerEffect, customerObject.transform.position, Quaternion.identity);

        // Wait for effect duration (adjust duration as needed)
        yield return new WaitForSeconds(1f);

        // Deactivate the customer object
        customerObject.SetActive(false);

        // Clear the current customer reference
        currentCustomer = null;

        // Clear the active customer object reference
        activeCustomerObject = null;

        // Proceed to the next customer
        ServeNextCustomer();
    }


    private void OnStopTimer()
    {
        HandleCustomerRequest(gameData.RoundedTime);
    }
}
