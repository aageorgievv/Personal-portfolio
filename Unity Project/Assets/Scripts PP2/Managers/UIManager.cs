using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IManager
{
    [Header("References")]
    [SerializeField] private Animator questAnimator;
    [SerializeField] private CollectApplesQuest CollectApplesQuest;
    [SerializeField] private CollectMushroomsQuest CollectMushroomsQuest;
    [SerializeField] private Camera playerCamera;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private RawImage prompt;
    [SerializeField] private TextMeshProUGUI interactionText;

    [Header("Settings")]
    [SerializeField] private float maxInteractionDistance = 3f;

    private DialogueManager dialogueManager;

    private bool isQuestTabOpen = false;

    private const string isOpenKey = "IsOpen";

    private void Start()
    {
        dialogueManager = GameManager.GetManager<DialogueManager>();
        ValidationUtility.ValidateReference(dialogueManager, nameof(dialogueManager));
        dialogueManager.OnDialogueStateEvent += HandleDialogueResult;

        HideQuestText();

    }

    private void Update()
    {
        ToggleQuestUI();
        GenerateQuestText();
        UpdateInteractionUI();
    }

    private void ToggleQuestUI()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isQuestTabOpen == false)
            {
                questAnimator.SetBool(isOpenKey, true);
                isQuestTabOpen = true;

            }
            else if (isQuestTabOpen == true)
            {
                questAnimator.SetBool(isOpenKey, false);
                isQuestTabOpen = false;
            }
        }
    }

    public void GenerateQuestText()
    {
        DialogueManager dialogueManager = GameManager.GetManager<DialogueManager>();
/*
        if ((dialogueManager.IsAppleQuest() || dialogueManager.IsAppleQuestBargained()) && CollectApplesQuest.collectedApples < 12)
        {
            ShowQuestText($"Apples: {CollectApplesQuest.collectedApples}/12");
        }
        else if ((dialogueManager.IsAppleQuest() || dialogueManager.IsAppleQuestBargained()) && CollectApplesQuest.collectedApples >= 12)
        {
            questText.color = Color.green;
            ShowQuestText($"Apples: {CollectApplesQuest.collectedApples}/12");
        }

        if ((dialogueManager.IsMushroomQuest() || dialogueManager.IsMushroomQuestBargained()) && CollectMushroomsQuest.collectedMushrooms < 8)
        {
            ShowQuestText($"Mushrooms: {CollectMushroomsQuest.collectedMushrooms}/8");
        }
        else if ((dialogueManager.IsMushroomQuest() || dialogueManager.IsMushroomQuestBargained()) && CollectMushroomsQuest.collectedMushrooms >= 8)
        {
            questText.color = Color.green;
            ShowQuestText($"Mushrooms: {CollectMushroomsQuest.collectedMushrooms}/8");
        }*/

        if (dialogueManager == null) return;

        if (dialogueManager.IsAppleQuest() || dialogueManager.IsAppleQuestBargained())
        {
            UpdateQuestText("Apples", CollectApplesQuest.collectedApples, 12);
        }

        if (dialogueManager.IsMushroomQuest() || dialogueManager.IsMushroomQuestBargained())
        {
            UpdateQuestText("Mushrooms", CollectMushroomsQuest.collectedMushrooms, 8);
        }
    }

    private void UpdateInteractionUI()
    {
        // Raycast from the center of the screen
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, maxInteractionDistance))
        {
            // Check the tag of the hit object and update the UI accordingly
            if (hit.collider.CompareTag("NPC"))
            {
                ShowPrompt("Interact");
            }
            else if (hit.collider.CompareTag("Anvil"))
            {
                ShowPrompt("Steal");
            }
            else if (hit.collider.CompareTag("Apple"))
            {
                ShowPrompt("Pick Up");
            }
            else if (hit.collider.CompareTag("Mushroom"))
            {
                ShowPrompt("Pick Up");
            }
            else
            {
                HidePrompt();
            }
        }
        else
        {
            HidePrompt();
        }
    }

    private void UpdateQuestText(string itemName, int collected, int target)
    {
        questText.color = collected >= target ? Color.green : Color.white;
        ShowQuestText($"{itemName}: {collected}/{target}");
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

    private void ShowPrompt(string text)
    {
        interactionText.text = text;
        prompt.gameObject.SetActive(true);
    }

    private void HidePrompt()
    {
        prompt.gameObject.SetActive(false);
    }

    private void HandleDialogueResult(EDialogueState result)
    {
        GenerateQuestText();
    }
}
