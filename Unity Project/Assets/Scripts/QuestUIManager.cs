using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    [SerializeField] Animator questAnimator;
    public TextMeshProUGUI questText;
    public DialogueManager dialogueManager;
    public CollectApplesQuest CollectApplesQuest;
    public CollectMushroomsQuest CollectMushroomsQuest;


    private bool isQuestTabOpen = false;


    void Start()
    {
        HideQuestText();
    }

    void Update()
    {
        ToggleQuestUI();
        GenerateQuestText();
    }

    private void ToggleQuestUI()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(isQuestTabOpen == false)
            {
                questAnimator.SetBool("IsOpen", true);
                isQuestTabOpen = true;

            } else if(isQuestTabOpen == true)
            {
                questAnimator.SetBool("IsOpen", false);
                isQuestTabOpen = false;
            }
        }
    }

    public void GenerateQuestText()
    {
        if((dialogueManager.IsAppleQuest() || dialogueManager.IsAppleQuestBargained()) && CollectApplesQuest.collectedApples < 12)
        {
            ShowQuestText($"Apples: {CollectApplesQuest.collectedApples}/12");
        } else if((dialogueManager.IsAppleQuest() || dialogueManager.IsAppleQuestBargained()) && CollectApplesQuest.collectedApples >= 12)
        {
            questText.color = Color.green;
            ShowQuestText($"Apples: {CollectApplesQuest.collectedApples}/12");
        }

        if((dialogueManager.IsMushroomQuest() || dialogueManager.IsMushroomQuestBargained()) && CollectMushroomsQuest.collectedMushrooms < 8)
        {
            ShowQuestText($"Mushrooms: {CollectMushroomsQuest.collectedMushrooms}/8");
        } else if((dialogueManager.IsMushroomQuest() || dialogueManager.IsMushroomQuestBargained()) && CollectMushroomsQuest.collectedMushrooms >= 8)
        {
            questText.color = Color.green;
            ShowQuestText($"Mushrooms: {CollectMushroomsQuest.collectedMushrooms}/8");
        }
    }

    private void ShowQuestText(string text)
    {
        questText.text = text;
        questText.gameObject.SetActive(true);
    }

    private void HideQuestText()
    {
        questText.gameObject.SetActive(false);
    }
}
