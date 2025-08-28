using System;
using UnityEngine;
using UnityEngine.AI;

public class FightState : State
{
    private readonly Animator npcAnimator;
    private readonly Transform npcTransform;
    private readonly NavMeshAgent npcAgent;
    private readonly Transform playerTransform;

    private readonly float minPunchSpeed;
    private readonly float maxPunchSpeed;
    private readonly float runCooldown;

    private float lastRunTime = float.MinValue;
    private float punchCooldown;

    private bool isPunchingLeft = true;

    private const string PunchLeftTrigger = "PunchLeft";
    private const string PunchRightTrigger = "PunchRight";
    private const string IsCloseKey = "IsClose";
    private const string CanPunchKey = "CanPunch";

    private const string PunchLeftStateName = "punchLeft";
    private const string PunchRightStateName = "punchRight";

    public FightState(NPCController npcController, Animator npcAnimator, NavMeshAgent npcAgent, Transform playerTransform, float minPunchSpeed, float maxPunchSpeed, float runCooldown)
    {
        this.npcAnimator = npcAnimator != null ? npcAnimator : throw new ArgumentNullException(nameof(npcAnimator));
        this.npcTransform = npcController.transform != null ? npcController.transform : throw new ArgumentNullException(nameof(npcController.transform));
        this.npcAgent = npcAgent != null ? npcAgent : throw new ArgumentNullException(nameof(npcAgent));
        this.playerTransform = playerTransform != null ? playerTransform : throw new ArgumentNullException(nameof(playerTransform));

        this.minPunchSpeed = minPunchSpeed;
        this.maxPunchSpeed = maxPunchSpeed;
        this.runCooldown = runCooldown;
    }

    public override void OnStarted()
    {

    }

    public override void OnStopped()
    {
        npcAnimator.SetBool(IsCloseKey, false);
        npcAnimator.SetBool(CanPunchKey, false);
    }

    public override void OnUpdate()
    {
        MoveToPlayer();
        RotateTowardsPlayer();
        Animation();
    }

    private void MoveToPlayer()
    {
        npcAgent.SetDestination(playerTransform.position);
    }

    private void RotateTowardsPlayer()
    {
        // Rotate towards the player
        Vector3 direction = (playerTransform.position - npcTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Quaternion targetRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);

        npcTransform.rotation = Quaternion.Slerp(npcTransform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void Animation()
    {

        // Trigger animation if close to the player
        float distanceToPlayer = Vector3.Distance(npcTransform.position, playerTransform.position);
        float timeSinceLastRun = Time.time - lastRunTime;

        if (distanceToPlayer >= 5f && timeSinceLastRun > runCooldown)
        {
            npcAnimator.SetBool(IsCloseKey, false);
            npcAgent.speed = 6;
            lastRunTime = Time.time;
        }
        else if (distanceToPlayer < 3f)
        {
            npcAgent.speed = 3;
            npcAnimator.SetBool(IsCloseKey, true);
        }

        // Punching logic with responsive timing
        if (distanceToPlayer < 2.5f)
        {
            npcAnimator.SetBool(CanPunchKey, true);
            // Adjust punch speed dynamically based on distance
            float punchSpeed = Mathf.Lerp(minPunchSpeed, maxPunchSpeed, 2.5f);

            // Trigger punch animations alternately based on cooldown
            if (punchCooldown <= 0f &&
                !npcAnimator.GetCurrentAnimatorStateInfo(0).IsName(PunchLeftStateName) &&
                !npcAnimator.GetCurrentAnimatorStateInfo(0).IsName(PunchRightStateName))
            {
                if (isPunchingLeft)
                {
                    npcAnimator.SetTrigger(PunchLeftTrigger);
                }
                else
                {
                    npcAnimator.SetTrigger(PunchRightTrigger);
                }

                isPunchingLeft = !isPunchingLeft;
                punchCooldown = punchSpeed;
            }
        }
        else
        {
            npcAnimator.SetBool(CanPunchKey, false);
        }

        // Cooldown timer
        if (punchCooldown > 0f)
        {
            punchCooldown -= Time.deltaTime;
        }
    }
}
