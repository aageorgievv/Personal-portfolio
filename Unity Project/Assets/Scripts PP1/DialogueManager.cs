using System.Threading;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static aiControls;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    // UI references
    public GameObject DialogueBox; 
    public TextMeshProUGUI DialogTitleText, DialogBodyText; 
    public GameObject responseButtonPrefab;
    public Transform responseButtonContainer; 
    public aiControls aiControls;
    public QuestUIManager questUIManager;
    [SerializeField] Animator dialogueAnimator;
    public GameObject icon;

    [SerializeField] float textSpeed;

    private EDialogueResult currentDialogueResult = EDialogueResult.None;


    //PP2-------------------------------------

    [SerializeField] private NPCController npcController;




    //----------------------------------------

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of DialogueManager
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }

        // Initially hide the dialogue UI
        HideDialogue();
    }

    // Starts the dialogue with given title and dialogue node
    public void StartDialogue(string title, DialogueNode node)
    {
        // Remove any existing response buttons
        foreach(Transform child in responseButtonContainer)
        {
            Destroy(child.gameObject);
        }

        DialogTitleText.text = title;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(node.dialogueText));

        // Create and setup response buttons based on current dialogue node
        foreach(DialogueResponse response in node.responses)
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
        if(!response.nextNode.IsLastNode())
        {
            StartDialogue(title, response.nextNode); // Start next dialogue
        } else
        {
            HideDialogue();

            switch(response.quest)
            {
                case EDialogueResult.None:
                    break;
                case EDialogueResult.QuestCollectApples:

                    currentDialogueResult = EDialogueResult.QuestCollectApples;
                    npcController.SetDialogueState(EDialogueState.QuestGivenApple);
                    questUIManager.GenerateQuestText();
                    icon.SetActive(false);

                    break;
                case EDialogueResult.QuestCollectMushrooms:

                    currentDialogueResult = EDialogueResult.QuestCollectMushrooms;
                    npcController.SetDialogueState(EDialogueState.QuestGivenMushroom);

                    questUIManager.GenerateQuestText();
                    icon.SetActive(false);

                    break;
                case EDialogueResult.NPCFightsBack:

                    aiControls.IsFighting();

                    break;
                case EDialogueResult.AppleBargain:

                    currentDialogueResult = EDialogueResult.AppleBargain;
                    npcController.SetDialogueState(EDialogueState.QuestCompletedBargainedApple);
                    questUIManager.GenerateQuestText();
                    icon.SetActive(false);

                    break;
                case EDialogueResult.MushroomBargain:

                    currentDialogueResult = EDialogueResult.MushroomBargain;
                    npcController.SetDialogueState(EDialogueState.QuestCompletedBargainedMushroom);
                    questUIManager.GenerateQuestText();
                    icon.SetActive(false);

                    break;

                default: throw new System.NotImplementedException(response.quest.ToString());
            }
        }
    }

    public bool IsAppleQuest()
    {
        return currentDialogueResult == EDialogueResult.QuestCollectApples;
    }

    public bool IsAppleQuestBargained()
    {
        return currentDialogueResult == EDialogueResult.AppleBargain;
    }

    public bool IsMushroomQuest()
    {
        return currentDialogueResult == EDialogueResult.QuestCollectMushrooms;
    }
    public bool IsMushroomQuestBargained()
    {
        return currentDialogueResult == EDialogueResult.MushroomBargain;
    }





    IEnumerator TypeSentence(string sentence)
    {
        DialogBodyText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            DialogBodyText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    // Hide the dialogue UI
    public void HideDialogue()
    {
        dialogueAnimator.SetBool("IsOpen", false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Show the dialogue UI
    public void ShowDialogue()
    {

        dialogueAnimator.SetBool("IsOpen", true);
    }

    // Check if dialogue is currently active
    public bool IsDialogueActive()
    {
        return DialogueBox.activeSelf;
    }
}