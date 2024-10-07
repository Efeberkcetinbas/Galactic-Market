using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductVariety : MonoBehaviour
{
    [SerializeField] private List<Transform> productVarieties;

    [SerializeField] private bool isRandomizingColors;
    

    // Call this method to randomly activate a child
    public void RandomizeVariety()
    {
        // First, deactivate all varieties
        foreach (Transform variety in productVarieties)
        {
            variety.gameObject.SetActive(false);
        }

        // Randomly select and activate one variety
        int randomIndex = Random.Range(0, productVarieties.Count);
        productVarieties[randomIndex].gameObject.SetActive(true);
        
        //For Testing
        if(isRandomizingColors)
            productVarieties[randomIndex].GetComponent<MeshRenderer>().material.color=GetRandomColor();
    }

    //For Testing
    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}
