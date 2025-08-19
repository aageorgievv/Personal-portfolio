using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : State
{
    private readonly NPCController npcController;
    private readonly Transform playerTransform;
    private readonly float interactionDistance;

    public TalkState(NPCController npcController, Transform playerTransform, float interactionDistance)
    {
        this.npcController = npcController;
        this.playerTransform = playerTransform;
        this.interactionDistance = interactionDistance;
    }

    public override void OnStarted()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void OnUpdate()
    {
        LookTowardsPlayer();
    }

    public override void OnStopped()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LookTowardsPlayer()
    {
        if (Vector3.Distance(npcController.transform.position, playerTransform.position) > interactionDistance)
        {
            npcController.SetState(EState.Idle);
            return;
        }

        Vector3 blacksmithToPlayer = (playerTransform.position - npcController.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(blacksmithToPlayer);
        Quaternion targetRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);

        npcController.transform.rotation = Quaternion.Slerp(npcController.transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}
