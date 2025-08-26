using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;


public class DialogueManager : MonoBehaviour, IManager
{
    public event Action<EDialogueState> OnDialogueStateEvent;

    [Header("References")]
    [SerializeField] private Actor actor;
    [SerializeField] private Animator dialogueAnimator;
    [SerializeField] private NPCController npcController;
    [SerializeField] private ShopUIHandler shopUIHandler;

    [Header("UI References")]
    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private TextMeshProUGUI DialogTitleText, DialogBodyText;
    [SerializeField] private GameObject responseButtonPrefab;
    [SerializeField] private Transform responseButtonContainer;
    [SerializeField] private Button shopButton;

    [Header("Settings")]
    [SerializeField, Min(0)] float textSpeed;

    private EDialogueResult currentDialogueResult = EDialogueResult.None;

    private const string isOpenKey = "IsOpen";

    private void Awake()
    {
        if (actor != null)
        {
            actor.OnDialogueStartedEvent += HandleStartDialogue;
        }

        if(shopButton != null)
        {
            shopButton.onClick.AddListener(() => shopUIHandler.ToggleShop());
        }
        HideDialogue();
    }

    private void StartDialogue(string title, DialogueNode node)
    {
        // Remove any existing response buttons
        foreach (Transform child in responseButtonContainer)
        {
            Destroy(child.gameObject);
        }

        DialogTitleText.text = title;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(node.dialogueText));

        

        // Create and setup response buttons based on current dialogue node
        foreach (DialogueResponse response in node.responses)
        {
            GameObject buttonObj = Instantiate(responseButtonPrefab, responseButtonContainer);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;

            // Setup button to trigger SelectResponse when clicked
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => SelectResponse(response, title));
        }

        ShowDialogue();
    }

    // Handles response selection and triggers next dialogue node
    public void SelectResponse(DialogueResponse response, string title)
    {
        // Check if there's a follow-up node
        if (!response.nextNode.IsLastNode())
        {
            StartDialogue(title, response.nextNode); // Start next dialogue
        }
        else
        {
            HideDialogue();

            switch (response.quest)
            {
                case EDialogueResult.None:
                    break;
                case EDialogueResult.QuestCollectApples:
                    currentDialogueResult = EDialogueResult.QuestCollectApples;
                    OnDialogueStateEvent.Invoke(EDialogueState.QuestGivenApple);
                    break;
                case EDialogueResult.QuestCollectMushrooms:

                    currentDialogueResult = EDialogueResult.QuestCollectMushrooms;
                    OnDialogueStateEvent.Invoke(EDialogueState.QuestGivenMushroom);
                    break;
                case EDialogueResult.NPCFightsBack:

                    npcController.SetState(EState.Fight);

                    break;
                case EDialogueResult.AppleBargain:
                    currentDialogueResult = EDialogueResult.AppleBargain;
                    OnDialogueStateEvent.Invoke(EDialogueState.QuestCompletedBargainedApple);
                    break;
                case EDialogueResult.MushroomBargain:
                    currentDialogueResult = EDialogueResult.MushroomBargain;
                    OnDialogueStateEvent.Invoke(EDialogueState.QuestCompletedBargainedMushroom);
                    break;

                default: throw new System.NotImplementedException(response.quest.ToString());
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        DialogBodyText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            DialogBodyText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void HideDialogue()
    {
        dialogueAnimator.SetBool(isOpenKey, false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowDialogue()
    {
        dialogueAnimator.SetBool(isOpenKey, true);
    }

    private void HandleStartDialogue(string name, DialogueNode node)
    {
        StartDialogue(name, node);
    }

    public bool IsAppleQuest() => currentDialogueResult == EDialogueResult.QuestCollectApples;
    public bool IsAppleQuestBargained() => currentDialogueResult == EDialogueResult.AppleBargain;
    public bool IsMushroomQuest() => currentDialogueResult == EDialogueResult.QuestCollectMushrooms;
    public bool IsMushroomQuestBargained() => currentDialogueResult == EDialogueResult.MushroomBargain;
}