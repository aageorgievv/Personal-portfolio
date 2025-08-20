using System;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public Action<string, DialogueNode> OnDialogueStartedEvent;

    [Header("Dialogue")]
    [SerializeField] private string Name;
    [SerializeField] private Dialogue QuestDialogue;
    [SerializeField] private Dialogue DialogueAppleQuestCompleted;
    [SerializeField] private Dialogue DialogueAppleQuestCompletedBargained;
    [SerializeField] private Dialogue DialogueMushroomQuestCompleted;
    [SerializeField] private Dialogue DialogueMushroomQuestCompletedBargained;
    [SerializeField] private Dialogue AppleQuestNotCompleted;
    [SerializeField] private Dialogue MushroomQuestNotCompleted;

    // Trigger dialogue for this actor
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
}