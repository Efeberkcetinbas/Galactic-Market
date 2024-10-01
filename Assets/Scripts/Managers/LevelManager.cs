using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [Header("Indexes")]
    [SerializeField] private GameData gameData;   
    public List<GameObject> levels;

    private void Awake()
    {
        LoadLevel();
    }
    private void LoadLevel()
    {


        gameData.levelIndex = PlayerPrefs.GetInt("LevelNumber");
        if (gameData.levelIndex == levels.Count)
        {
            gameData.levelIndex = 0;
            EventManager.Broadcast(GameEvent.OnRestartGame);
        }
        PlayerPrefs.SetInt("LevelNumber", gameData.levelIndex);
        
        gameData.levelNumber=PlayerPrefs.GetInt("RealLevel");

        EventManager.Broadcast(GameEvent.OnLevelUIUpdate);
       

        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].SetActive(false);
        }
        levels[gameData.levelIndex].SetActive(true);
    }

    public void LoadNextLevel()
    {
        PlayerPrefs.SetInt("LevelNumber", gameData.levelIndex + 1);
        PlayerPrefs.SetInt("RealLevel", PlayerPrefs.GetInt("RealLevel", 0) + 1);
        LoadLevel();
        EventManager.Broadcast(GameEvent.OnNextLevel);
    }

    public void RestartLevel()
    {
        EventManager.Broadcast(GameEvent.OnRestartLevel);
    }

    

    
    
}
