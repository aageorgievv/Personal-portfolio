using UnityEngine;
using UnityEngine.AI;

public enum EState
{
    Idle,
    Talk,
    Fight,
    StealingReaction
}

public enum EDialogueState
{
    Default,
    QuestGivenApple,
    QuestGivenMushroom,
    QuestCompleted,
    QuestCompletedBargainedApple,
    QuestCompletedBargainedMushroom,
}

public class NPCController : MonoBehaviour
{
    public EDialogueState CurrentDialogueState => currentDialogueState;

    [Header("References")]
    [SerializeField] private PlayerControls playerControls;
    [SerializeField] private Actor actor;
    [SerializeField] private Animator npcAnimator;
    [SerializeField] private Transform npcTransform;
    [SerializeField] private NavMeshAgent npcAgent;
    [SerializeField] private Transform playerTransform;

    [Header("Settings")]
    [SerializeField, Min(0)] private float interactionDistance;
    [SerializeField, Min(0)] private float minPunchSpeed;
    [SerializeField, Min(0)] private float maxPunchSpeed;
    [SerializeField, Min(0)] private float runCooldown;

    [Header("Quests")]
    [SerializeField] private CollectApplesQuest appleQuest;
    [SerializeField] private CollectMushroomsQuest mushroomsQuest;

    private StateMachine<EState> stateMachine;
    private EDialogueState currentDialogueState = EDialogueState.Default;

    private void Awake()
    {
        ValidationUtility.ValidateReference(playerControls, nameof(playerControls));
        ValidationUtility.ValidateReference(actor, nameof(actor));
        ValidationUtility.ValidateReference(npcAnimator, nameof(npcAnimator));
        ValidationUtility.ValidateReference(npcTransform, nameof(npcTransform));
        ValidationUtility.ValidateReference(npcAgent, nameof(npcAgent));
        ValidationUtility.ValidateReference(playerTransform, nameof(playerTransform));

        stateMachine = new StateMachine<EState>();

        //Register States
        stateMachine.AddState(EState.Idle, new IdleState());
        stateMachine.AddState(EState.Talk, new TalkState(this, playerControls.transform, interactionDistance));
        stateMachine.AddState(EState.Fight, new FightState(this, npcAnimator, npcAgent, playerTransform, minPunchSpeed, maxPunchSpeed, runCooldown));

        stateMachine.SetState(EState.Idle);

        if (playerControls != null)
        {
            playerControls.OnStealEvent += HandleStealEvent;
        }
    }

    private void OnDestroy()
    {
        if (playerControls != null)
        {
            playerControls.OnStealEvent -= HandleStealEvent;
        }
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void SetState(EState state)
    {
        stateMachine.SetState(state);
    }

    public void SetDialogueState(EDialogueState state)
    {
        currentDialogueState = state;
    }

    public void TryTalk()
    {
        if (Vector3.Distance(transform.position, playerControls.transform.position) <= interactionDistance)
        {
            // Switch dialogues based on the current dialogue state
            switch (currentDialogueState)
            {
                case EDialogueState.Default:
                    actor.StartQuestDialogue();
                    break;

                case EDialogueState.QuestGivenApple:
                    if (appleQuest.IsCompleted)
                    {
                        actor.StartAppleQuestCompleted();
                        currentDialogueState = EDialogueState.QuestCompleted;
                    }
                    else
                    {
                        actor.StartAppleQuestNotCompleted();
                    }
                    break;

                case EDialogueState.QuestGivenMushroom:
                    if (mushroomsQuest.IsCompleted)
                    {
                        actor.StartMushroomQuestCompleted();
                        currentDialogueState = EDialogueState.QuestCompleted;
                    }
                    else
                    {
                        actor.StartMushroomQuestNotCompleted();
                    }
                    break;

                case EDialogueState.QuestCompleted:
                    break;

                case EDialogueState.QuestCompletedBargainedApple:
                    if (appleQuest.IsCompleted)
                    {
                        actor.StartAppleQuestCompletedBargained();
                        currentDialogueState = EDialogueState.QuestCompleted;
                    }
                    else
                    {
                        actor.StartAppleQuestNotCompleted();
                    }
                    break;

                case EDialogueState.QuestCompletedBargainedMushroom:
                    if (mushroomsQuest.IsCompleted)
                    {
                        actor.StartMushroomQuestCompletedBargained();
                        currentDialogueState = EDialogueState.QuestCompleted;
                    }
                    else
                    {
                        actor.StartMushroomQuestNotCompleted();
                    }
                    break;
            }

            stateMachine.SetState(EState.Talk);
        }
    }

    public void OnPunchHit(Collider other)
    {

        Vector3 knockbackDirection = (playerControls.transform.position - transform.position).normalized;
        if (playerControls != null)
        {
            playerControls.ApplyKnockback(knockbackDirection);
        }
    }

    private void HandleStealEvent(EState state)
    {
        SetState(state);
    }
}
