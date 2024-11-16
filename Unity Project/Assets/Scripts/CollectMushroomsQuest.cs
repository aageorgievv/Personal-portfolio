using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectMushroomsQuest : Quest
{
    PlayerControls player;

    [SerializeField]
    private QuestUIManager questUIManager;

    [SerializeField]
    public Mushroom[] mushroomsToCollect = Array.Empty<Mushroom>();

    public int collectedMushrooms = 0;

    private void Awake()
    {
        foreach(Mushroom mushroom in mushroomsToCollect)
        {
            mushroom.OnDestroyed += HandleAppleDestroyed;
        }
    }

    private void HandleAppleDestroyed()
    {
        if(collectedMushrooms < 8)
        {
            collectedMushrooms++;
        }

        if(collectedMushrooms >= 8)
        {
            CompleteQuest();
            Debug.Log("Quest Completed:");
        }
    }
}
