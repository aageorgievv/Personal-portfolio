using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class aiControls : MonoBehaviour
{
    public enum EState
    {
        Idle,
        Talk,
        Fight,
        QuestBehaviour,
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

    private EState currentState = EState.Idle;
    private EState previousState;

    public EDialogueState currentDialogueState = EDialogueState.Default;

    private PlayerControls PlayerControls;
    public Transform playerTransform;
    public NavMeshAgent agent;
    public Actor actor;
    public Animator aiAnimator;
    public GameObject icon;
    public DialogueManager dialogueManager;
    public Camera playerCamera;

    [SerializeField]
    public CollectApplesQuest appleQuest;
    public CollectMushroomsQuest mushroomsQuest;

    private float punchCooldown = 0f;
    private float maxPunchSpeed = 0.5f;
    private float minPunchSpeed = 1.5f;

    private bool isPunchingLeft = true;

    private float lastRunTime = float.MinValue;
    private const float runCooldown = 5;
    public float maxInteractionDistance = 3f;

    // Start is called before the first frame update
    void Start()
    {
        appleQuest.OnQuestCompleted += OnAppleQuestCompleted;
        previousState = currentState;
        PlayerControls = playerTransform.GetComponent<PlayerControls>();
    }

    private void OnAppleQuestCompleted()
    {
        // only called when the quest is completed

    }

    // Update is called once per frame
    void Update()
    {
        SwitchStates();
    }

    private void SwitchStates()
    {
        IsCloseEnoughToInteract();

        switch(currentState)
        {
            case EState.Idle:
                // Logic
                break;
            case EState.Talk:
                HandleTalkState();
                break;
            case EState.Fight:
                FightState();
                break;
            case EState.QuestBehaviour:
                // Logic
                break;
            case EState.StealingReaction:
                // Logic
                break;
        }


    }

    private void IsCloseEnoughToInteract()
    {
        // Create a ray from the player's camera
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Check if the ray hits something within the interaction distance
        if(Physics.Raycast(ray, out hit, maxInteractionDistance) && Input.GetKeyDown(KeyCode.E))
        {
            // Ensure the hit object is the NPC
            if(hit.collider.CompareTag("NPC")) // Replace "NPC" with the actual tag of your NPCs
            {
                previousState = currentState;
                currentState = EState.Talk;

                // Switch dialogues based on the current dialogue state
                switch(currentDialogueState)
                {
                    case EDialogueState.Default:
                        actor.StartQuestDialogue();
                        break;

                    case EDialogueState.QuestGivenApple:
                        if(appleQuest.IsCompleted)
                        {
                            actor.StartAppleQuestCompleted();
                            currentDialogueState = EDialogueState.QuestCompleted;
                        } else
                        {
                            actor.StartAppleQuestNotCompleted();
                        }
                        break;

                    case EDialogueState.QuestGivenMushroom:
                        if(mushroomsQuest.IsCompleted)
                        {
                            actor.StartMushroomQuestCompleted();
                            currentDialogueState = EDialogueState.QuestCompleted;
                        } else
                        {
                            actor.StartMushroomQuestNotCompleted();
                        }
                        break;

                    case EDialogueState.QuestCompleted:
                        break;

                    case EDialogueState.QuestCompletedBargainedApple:
                        if(appleQuest.IsCompleted)
                        {
                            actor.StartAppleQuestCompletedBargained();
                            currentDialogueState = EDialogueState.QuestCompleted;
                        } else
                        {
                            actor.StartAppleQuestNotCompleted();
                        }
                        break;

                    case EDialogueState.QuestCompletedBargainedMushroom:
                        if(mushroomsQuest.IsCompleted)
                        {
                            actor.StartMushroomQuestCompletedBargained();
                            currentDialogueState = EDialogueState.QuestCompleted;
                        } else
                        {
                            actor.StartMushroomQuestNotCompleted();
                        }
                        break;
                }

                // Unlock the cursor for dialogue interaction
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }


    public void IsFighting()
    {
        previousState = currentState;
        currentState = EState.Fight;
        icon.SetActive(false);
        //Debug.Log("Fight");
    }

    public bool IsIdle()
    {
        return currentState == EState.Idle;
    }
    private void HandleTalkState()
    {
        if(Vector3.Distance(transform.position, playerTransform.position) > 3f)
        {
            currentState = EState.Idle;
            return;
        }

        // Rotating towards player once interracting
        Vector3 aiToPlayer = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(aiToPlayer);
        Quaternion targetRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);

        // Slerp to smoothly interpolate only the Y-axis rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void FightState()
    {
        if(agent != null && playerTransform != null)
        {

            // Set destination for NavMeshAgent
            agent.SetDestination(playerTransform.position);

            
            // Rotate towards the player
            Vector3 aiToPlayer = (playerTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(aiToPlayer);
            Quaternion targetRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            // Trigger animation if close to the player
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            float timeSinceLastRun = Time.time - lastRunTime;
            if(distanceToPlayer >= 5f && timeSinceLastRun > runCooldown)
            {
                aiAnimator.SetBool("IsClose", false);
                agent.speed = 6;
                lastRunTime = Time.time;
            } else if (distanceToPlayer < 3f)
            {
                agent.speed = 3;
                aiAnimator.SetBool("IsClose", true);
            }

            // Punching logic with responsive timing
            if(distanceToPlayer < 2.5f)
            {
                aiAnimator.SetBool("CanPunch", true);
                // Adjust punch speed dynamically based on distance
                float punchSpeed = Mathf.Lerp(maxPunchSpeed, minPunchSpeed, 2.5f);

                // Trigger punch animations alternately based on cooldown
                if(punchCooldown <= 0f &&
                    !aiAnimator.GetCurrentAnimatorStateInfo(0).IsName("punchLeft") &&
                    !aiAnimator.GetCurrentAnimatorStateInfo(0).IsName("punchRight"))
                {
                    if(isPunchingLeft)
                    {
                        aiAnimator.SetTrigger("PunchLeft");
                    } else
                    {
                        aiAnimator.SetTrigger("PunchRight");
                    }

                    isPunchingLeft = !isPunchingLeft;
                    punchCooldown = punchSpeed; // Reset cooldown based on speed
                }
            } else
            {
                aiAnimator.SetBool("CanPunch", false);
            }

            // Cooldown timer
            if(punchCooldown > 0f)
            {
                punchCooldown -= Time.deltaTime;
            }

            //Debug.Log("Distance: " + distanceToPlayer + ", Speed set to: " + agent.speed);
        }
    }

    public void OnPunchHit(Collider other)
    {

        Vector3 knockbackDirection = (playerTransform.position - transform.position).normalized;
        if(PlayerControls != null)
        {
            PlayerControls.ApplyKnockback(knockbackDirection);
        }
    }
}
