using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractionUIManager : MonoBehaviour
{
    public RawImage prompt;
    public TextMeshProUGUI interactionText;   
    public Camera playerCamera;
    public float maxInteractionDistance = 3f; 


    private void Update()
    {
        UpdateInteractionUI();
    }

    private void UpdateInteractionUI()
    {
        // Raycast from the center of the screen
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Perform the raycast
        if(Physics.Raycast(ray, out hit, maxInteractionDistance))
        {
            // Check the tag of the hit object and update the UI accordingly
            if(hit.collider.CompareTag("NPC"))
            {
                ShowPrompt("Interact");
            } else if(hit.collider.CompareTag("Anvil"))
            {
                ShowPrompt("Steal");
            } else if(hit.collider.CompareTag("Apple"))
            {
                ShowPrompt("Pick Up");
            } else if(hit.collider.CompareTag("Mushroom"))
            {
                ShowPrompt("Pick Up");
            } else
            {
                HidePrompt();
            }
        } else
        {
            HidePrompt();
        }
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
}
