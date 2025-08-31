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
    [SerializeField] private NPCController npcController;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private RawImage prompt;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("Settings")]
    [SerializeField] private float maxInteractionDistance = 3f;

    private DialogueManager dialogueManager;
    private MoneyManager moneyManager;

    private bool isQuestTabOpen = false;

    private const string isOpenKey = "IsOpen";

    private readonly Dictionary<string, string> promptsByTag = new Dictionary<string, string>();

    private void Awake()
    {
        promptsByTag.Add("NPC", "Interact");
        promptsByTag.Add("Anvil", "Steal");
        promptsByTag.Add("Apple", "Pick Up");
        promptsByTag.Add("Mushroom", "Pick Up");

        GameManager.ExecuteWhenInitialized(HandleGameManagerInitialized);
        HideQuestText();
    }

    private void HandleGameManagerInitialized()
    {
        dialogueManager = GameManager.GetManager<DialogueManager>();
        moneyManager = GameManager.GetManager<MoneyManager>();
        ValidationUtility.ValidateReference(dialogueManager, nameof(dialogueManager));
        ValidationUtility.ValidateReference(moneyManager, nameof(moneyManager));
        dialogueManager.OnDialogueStateEvent += HandleDialogueResult;
        moneyManager.OnMoneyChanged += HandleUpdatingMoneyText;
    }

    private void OnDestroy()
    {
        if(dialogueManager !=  null)
        {
            dialogueManager.OnDialogueStateEvent -= HandleDialogueResult;
        }
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

        if (dialogueManager == null) return;

        if (dialogueManager.IsAppleQuest() || dialogueManager.IsAppleQuestBargained())
        {
            UpdateQuestText("Apples", CollectApplesQuest.collectedApples, 12);
        }

        if (dialogueManager.IsMushroomQuest() || dialogueManager.IsMushroomQuestBargained())
        {
            UpdateQuestText("Mushrooms", CollectMushroomsQuest.collectedMushrooms, 8);
        }

        if (dialogueManager.IsEscortQuest())
        {
            UpdateEscortQuestText("Acompany the blacksmith");
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
            if (!promptsByTag.TryGetValue(hit.collider.tag, out string prompt))
            {
                HidePrompt();
                return;
            }

            ShowPrompt(prompt);
        }
        else
        {
            HidePrompt();
        }
    }

    private void HandleUpdatingMoneyText(int amount)
    {
        moneyText.text = $"{amount}";
    }

    private void UpdateQuestText(string itemName, int collected, int target)
    {
        questText.color = collected >= target ? Color.green : Color.white;
        ShowQuestText($"{itemName}: {collected}/{target}");
    }

    private void UpdateEscortQuestText(string itemName)
    {
        questText.color = npcController.EscortCompleted ? Color.green : Color.white;
        ShowQuestText($"{itemName}");
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
