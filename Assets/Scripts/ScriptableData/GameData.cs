using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 0)]
public class GameData : ScriptableObject 
{
    //Time Mananagement
    public int RoundedTime;
    public int maxTimerRange;

    public bool isStartTimer=false;

    public TimerTypes timerTypes;

    //Game Management
    public int score;
    public int increaseScore;
    public int levelIndex;
    public int levelNumber;

    public bool isGameEnd=false;

}
