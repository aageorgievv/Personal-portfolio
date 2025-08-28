using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EscortState : State
{
    private readonly Action OnDestinationReachedEvent;

    private readonly Animator npcAnimator;
    private readonly Transform npcTransform;
    private readonly NavMeshAgent npcAgent;
    private readonly Transform playerTransform;
    private readonly List<Transform> travelPoints;

    private readonly int escortWalkSpeed;

    private int currentTravelPointIndex = -1;
    private int separationDistance = 6;

    private const string IsCloseKey = "IsClose";
    private const string IsWalk = "Walk";
    private const string IsIdle = "Idle";

    public EscortState(Animator npcAnimator, Transform npcTransform, NavMeshAgent npcAgent, Transform playerTransform, List<Transform> travelPoints, Action OnDestinationReachedEvent,  int escortWalkSpeed)
    {
        this.npcAnimator = npcAnimator;
        this.npcTransform = npcTransform;
        this.npcAgent = npcAgent;
        this.playerTransform = playerTransform;
        this.travelPoints = travelPoints;
        this.OnDestinationReachedEvent = OnDestinationReachedEvent;
        this.escortWalkSpeed = escortWalkSpeed;
    }

    public override void OnStarted()
    {
        npcAnimator.SetBool(IsCloseKey, true);
    }

    public override void OnStopped()
    {

    }

    public override void OnUpdate()
    {
        Move();
    }

    public void Move()
    {
        CheckDistanceToPlayer();

        if (!npcAgent.pathPending && npcAgent.remainingDistance <= npcAgent.stoppingDistance)
        {
            if (currentTravelPointIndex == travelPoints.Count - 1)
            {
                if (Vector3.Distance(npcTransform.position, travelPoints[currentTravelPointIndex].position) >= npcAgent.stoppingDistance)
                {
                    npcAgent.isStopped = true;
                    npcAnimator.SetTrigger(IsIdle);
                    OnDestinationReachedEvent.Invoke();
                }
            }
            else
            {
                npcAgent.speed = escortWalkSpeed;
                currentTravelPointIndex++;
                npcAgent.SetDestination(travelPoints[currentTravelPointIndex].position);
            }
        }
    }

    private void CheckDistanceToPlayer()
    {
        if (Vector3.Distance(npcTransform.position, playerTransform.position) >= separationDistance)
        {
            npcAgent.isStopped = true;
            npcAnimator.SetTrigger(IsIdle);

            Vector3 blacksmithToPlayer = (playerTransform.position - npcTransform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(blacksmithToPlayer);
            Quaternion targetRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);

            npcTransform.rotation = Quaternion.Slerp(npcTransform.rotation, targetRotation, Time.deltaTime * 5f);
        }
        else
        {
            npcAgent.isStopped = false;
            npcAnimator.SetTrigger(IsWalk);
        }
    }
}
