using UnityEngine;

public class MouseManager : MonoBehaviour, IManager
{
    public Item currentlyHeldItem;

    private InventoryManager inventoryManager;

    private void Awake()
    {
        GameManager.ExecuteWhenInitialized(HandleGameManagerInitialized);
    }

    private void HandleGameManagerInitialized()
    {
        inventoryManager = GameManager.GetManager<InventoryManager>();
        ValidationUtility.ValidateReference(inventoryManager, nameof(inventoryManager));
    }

    public void UpdateHeldItem(UISlotHandler currentSlot)
    {
        Item currentActiveItem = currentSlot.item;

        if (currentlyHeldItem != null && currentActiveItem != null && currentlyHeldItem.itemId == currentActiveItem.itemId)
        {
            inventoryManager.StackInInventory(currentSlot, currentlyHeldItem);
            currentlyHeldItem = null;
            return;
        }

        if(currentSlot.item != null)
        {
            inventoryManager.ClearItemSlot(currentSlot);
        }

        if(currentlyHeldItem != null)
        {
            inventoryManager.PlaceInInventory(currentSlot,currentlyHeldItem);
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
            inventoryManager.ClearItemSlot(currentSlot);
        }
    }
}
