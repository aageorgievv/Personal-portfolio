using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static aiControls;
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

    private StateMachine<EState> stateMachine;
    private EDialogueState currentDialogueState = EDialogueState.Default;

    [Header("References")]
    [SerializeField] private PlayerControls playerControls;
    [SerializeField] private Actor actor;
    [SerializeField] private Animator npcAnimator;
    [SerializeField] private Transform npcTransform;
    [SerializeField] private NavMeshAgent npcAgent;
    [SerializeField] private Transform playerTransform;

    [Header("Settings")]
    [SerializeField] float interactionDistance;
    [SerializeField] private float minPunchSpeed;
    [SerializeField] private float maxPunchSpeed;
    [SerializeField] private float runCooldown;

    [Header("Quests")]
    public CollectApplesQuest appleQuest;
    public CollectMushroomsQuest mushroomsQuest;

    private void Start()
    {
        stateMachine = new StateMachine<EState>();

        //Register States
        stateMachine.AddState(EState.Idle, new IdleState());
        stateMachine.AddState(EState.Talk, new TalkState(this, playerControls.transform, interactionDistance));
        stateMachine.AddState(EState.Fight, new FightState(this, npcAnimator, npcAgent, playerTransform, minPunchSpeed, maxPunchSpeed, runCooldown));

        stateMachine.SetState(EState.Idle);

        if(playerControls != null)
        {
            playerControls.OnSteal += SetState;
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
        if(playerControls != null)
        {
            playerControls.ApplyKnockback(knockbackDirection);
        }
    }
}
