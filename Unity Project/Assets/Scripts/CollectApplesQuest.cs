using System;
using UnityEngine;

public class CollectApplesQuest : Quest
{
    PlayerControls player;

    [SerializeField]
    private QuestUIManager questUIManager;

    [SerializeField]
    public Apple[] applesToCollect = Array.Empty<Apple>();

    public int collectedApples = 0;

    private void Awake()
    {
        foreach(Apple apple in applesToCollect)
        {
            apple.OnDestroyed += HandleAppleDestroyed;
        }
    }

    private void HandleAppleDestroyed()
    {
        if(collectedApples < 12)
        {
            collectedApples++;
        }
        
        if (collectedApples >= 12)
        {
            CompleteQuest();
            Debug.Log("Quest Completed:");
        }
    }
}
