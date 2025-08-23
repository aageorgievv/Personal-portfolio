using UnityEngine;

public class MouseManager : MonoBehaviour, IManager
{
    public Item currentlyHeldItem;

    [SerializeField]
    private Inventory playerInventoryManager;

    private void Awake()
    {
        ValidationUtility.ValidateReference(playerInventoryManager, nameof(playerInventoryManager));
    }

    public void UpdateHeldItem(UISlotHandler currentSlot)
    {
        Item currentActiveItem = currentSlot.item;

        if (currentlyHeldItem != null && currentActiveItem != null && currentlyHeldItem.itemId == currentActiveItem.itemId)
        {
            playerInventoryManager.StackInInventory(currentSlot, currentlyHeldItem);
            currentlyHeldItem = null;
            return;
        }

        if(currentSlot.item != null)
        {
            playerInventoryManager.ClearItemSlot(currentSlot);
        }

        if(currentlyHeldItem != null)
        {
            playerInventoryManager.PlaceInInventory(currentSlot,currentlyHeldItem);
        }

        currentlyHeldItem = currentActiveItem;
    }

    public void PickUpFromStack(UISlotHandler currentSlot)
    {
        if (ValidationUtility.ValidateReference(currentSlot.item, nameof(currentSlot.item))) {
            return;
        }

        if(currentlyHeldItem != null && currentlyHeldItem.itemId != currentSlot.item.itemId)
        {
            return;
        }

        if(currentlyHeldItem == null)
        {
            currentlyHeldItem = currentSlot.item.Clone();
            currentlyHeldItem.itemCount = 0;
        }

        currentlyHeldItem.itemCount++;
        currentSlot.item.itemCount--;
        currentSlot.itemCountText.text = currentSlot.item.itemCount.ToString();

        if (currentSlot.item.itemCount <= 0)
        {
            playerInventoryManager.ClearItemSlot(currentSlot);
        }
    }
}
