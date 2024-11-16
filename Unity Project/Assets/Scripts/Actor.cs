using UnityEngine;

public class Actor : MonoBehaviour
{
    public string Name;
    public Dialogue QuestDialogue;
    public Dialogue DialogueAppleQuestCompleted;
    public Dialogue DialogueAppleQuestCompletedBargained;
    public Dialogue DialogueMushroomQuestCompleted;
    public Dialogue DialogueMushroomQuestCompletedBargained;
    public Dialogue AppleQuestNotCompleted;
    public Dialogue MushroomQuestNotCompleted;

    private void Update()
    {
    }

    // Trigger dialogue for this actor
    public void StartQuestDialogue()
    {
        DialogueManager.Instance.StartDialogue(Name, QuestDialogue.RootNode);
    }

    public void StartAppleQuestCompleted()
    {
        DialogueManager.Instance.StartDialogue(Name, DialogueAppleQuestCompleted.RootNode);
    }

    public void StartAppleQuestCompletedBargained()
    {
        DialogueManager.Instance.StartDialogue(Name, DialogueAppleQuestCompletedBargained.RootNode);
    }

    public void StartMushroomQuestCompleted()
    {
        DialogueManager.Instance.StartDialogue(Name, DialogueMushroomQuestCompleted.RootNode);
    }

    public void StartMushroomQuestCompletedBargained()
    {
        DialogueManager.Instance.StartDialogue(Name, DialogueMushroomQuestCompletedBargained.RootNode);
    }

    public void StartAppleQuestNotCompleted()
    {
        DialogueManager.Instance.StartDialogue(Name, AppleQuestNotCompleted.RootNode);
    }
    public void StartMushroomQuestNotCompleted()
    {
        DialogueManager.Instance.StartDialogue(Name, MushroomQuestNotCompleted.RootNode);
    }
}