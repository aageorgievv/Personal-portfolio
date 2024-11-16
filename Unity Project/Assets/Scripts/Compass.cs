using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Compass : MonoBehaviour
{
    public Transform player;
    public RectTransform compassBackground;
    public GameObject npcIconPrefab;
    public CollectMushroomsQuest mushroomQuest;
    public CollectApplesQuest applesQuest;


    // Dictionary to store icons associated with each NPC
    private Dictionary<aiControls, GameObject> npcIcons = new Dictionary<aiControls, GameObject>();

    void Start()
    {
        // Initialize icons for each NPC only once
        foreach(aiControls npc in FindObjectsOfType<aiControls>())
        {
            GameObject icon = Instantiate(npcIconPrefab, compassBackground);
            npcIcons[npc] = icon;
        }
    }

    void Update()
    {
        foreach(var npcEntry in npcIcons)
        {
            aiControls npc = npcEntry.Key;
            GameObject icon = npcEntry.Value;
            RectTransform iconRect = icon.GetComponent<RectTransform>();

            if(!npc.IsIdle())
            {
                icon.SetActive(false);
                continue; // Skip further processing for this NPC
            }

            Vector3 playerToNPC = npc.transform.position - player.position;
            playerToNPC.y = 0; // Ignore height differences

            float angle = Vector3.SignedAngle(player.forward, playerToNPC, Vector3.up);

            // Check if NPC is within the 90-degree field of view
            if(Mathf.Abs(angle) <= 90)
            {
                // Show icon and update its position
                icon.SetActive(true);
                float iconXPosition = (angle / 90f) * (compassBackground.rect.width / 2);
                iconRect.anchoredPosition = new Vector2(iconXPosition, iconRect.anchoredPosition.y);
            } else
            {
                // Hide icon if NPC is out of range
                icon.SetActive(false);
            }
        }
    }
}
