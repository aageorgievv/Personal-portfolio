using System;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public event Action<string, DialogueNode> OnDialogueStartedEvent;

    [Header("Dialogue")]
    [SerializeField] private string Name;
    [SerializeField] private Dialogue QuestDialogue;
    [SerializeField] private Dialogue DialogueAppleQuestCompleted;
    [SerializeField] private Dialogue DialogueAppleQuestCompletedBargained;
    [SerializeField] private Dialogue DialogueMushroomQuestCompleted;
    [SerializeField] private Dialogue DialogueMushroomQuestCompletedBargained;
    [SerializeField] private Dialogue AppleQuestNotCompleted;
    [SerializeField] private Dialogue MushroomQuestNotCompleted;
    [SerializeField] private Dialogue EscortQuestCompleted;


    private void Awake()
    {
        //Fix validation
    }

    // Fix references
    public void StartQuestDialogue()
    {
        OnDialogueStartedEvent?.Invoke(Name, QuestDialogue.RootNode);
    }

    public void StartAppleQuestCompleted()
    {
        OnDialogueStartedEvent?.Invoke(Name, DialogueAppleQuestCompleted.RootNode);
    }

    public void StartAppleQuestCompletedBargained()
    {
        OnDialogueStartedEvent?.Invoke(Name, DialogueAppleQuestCompletedBargained.RootNode);

    }

    public void StartMushroomQuestCompleted()
    {
        OnDialogueStartedEvent?.Invoke(Name, DialogueMushroomQuestCompleted.RootNode);
    }

    public void StartMushroomQuestCompletedBargained()
    {
        OnDialogueStartedEvent?.Invoke(Name, DialogueMushroomQuestCompletedBargained.RootNode);
    }

    public void StartAppleQuestNotCompleted()
    {
        OnDialogueStartedEvent?.Invoke(Name, AppleQuestNotCompleted.RootNode);
    }
    public void StartMushroomQuestNotCompleted()
    {
        OnDialogueStartedEvent?.Invoke(Name, MushroomQuestNotCompleted.RootNode);
    }
    public void EscortQuestComplete()
    {
        OnDialogueStartedEvent?.Invoke(Name, EscortQuestCompleted.RootNode);
    }
}