using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameEvent
{
    

    //Customer
    OnCustomerSpawn,
    OnUpdateRequirement,
    OnCustomerSatisfy,
    OnCustomerLeavingPoint,
    OnCustomerLeavePress,
    OnCustomerLeaves,
    OnCustomerLeaved,
    OnAllCustomerSatisfy,
    OnUpdateCustomerNumber,

    //Product
    OnSpawnProduct,
    OnProductHit,
    OnShootingEnded,
    
    //Game Management
    OnGameStart,
    OnIncreaseScore,
    OnUIUpdate,
    OnLevelUIUpdate,
    
    OnNextLevel,
    OnSuccess,
    OnFail,
    OnSuccessUI,
    OnFailUI,
    OnRestartLevel,
    OnRestartGame,

    //Time Management
    OnStartTimer,
    OnPressStopTimer,
    OnStopTimer

}
public class EventManager
{
    private static Dictionary<GameEvent,Action> eventTable = new Dictionary<GameEvent, Action>();
    
    private static Dictionary<GameEvent,Action<int>> IdEventTable=new Dictionary<GameEvent, Action<int>>();
    //2 parametre baglayacagimiz ile bagladigimiz

    
    public static void AddHandler(GameEvent gameEvent,Action action)
    {
        if(!eventTable.ContainsKey(gameEvent))
            eventTable[gameEvent]=action;
        else eventTable[gameEvent]+=action;
    }

    public static void RemoveHandler(GameEvent gameEvent,Action action)
    {
        if(eventTable[gameEvent]!=null)
            eventTable[gameEvent]-=action;
        if(eventTable[gameEvent]==null)
            eventTable.Remove(gameEvent);
    }

    public static void Broadcast(GameEvent gameEvent)
    {
        if(eventTable[gameEvent]!=null)
            eventTable[gameEvent]();
    }
    
}
