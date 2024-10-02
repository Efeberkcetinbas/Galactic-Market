using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Customers/Customer", order = 0)]
public class CustomerData : ScriptableObject
{
    public Sprite sprite;
    public string customerName;
    public string planetName;
    public ProductTypes productTypes;
    public DetectionMethod detectionMethod;
    public int targetNumber;      // For exact, greater, less
    public int rangeMin;          // For between
    public int rangeMax;          // For between
}
