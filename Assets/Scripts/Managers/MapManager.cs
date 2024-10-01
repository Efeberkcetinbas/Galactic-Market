using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MapManager : MonoBehaviour
{
    [SerializeField] private List<Transform> levelButtons=new List<Transform>();

    [SerializeField] private Color selectedColor,defaultColor;

    [SerializeField] private GameData gameData;

    private void Start()
    {
        CheckButtons();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);

    }

    private void CheckButtons()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            levelButtons[i].GetChild(0).GetComponent<Image>().color=defaultColor;
            levelButtons[i].GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText((i+1+gameData.levelNumber).ToString());
        }

        /*levelButtons[gameData.levelIndex].GetChild(0).GetComponent<Image>().color=selectedColor;
        levelButtons[gameData.levelIndex].localScale=Vector3.one*1.25f;*/

        levelButtons[0].GetChild(0).GetComponent<Image>().color=selectedColor;
        levelButtons[0].localScale=Vector3.one*1.25f;

    }

    private void OnNextLevel()
    {
        CheckButtons();
    }

}
