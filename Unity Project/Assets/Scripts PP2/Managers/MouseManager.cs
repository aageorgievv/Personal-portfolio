using Unity.VisualScripting;
using UnityEngine;

public class MouseManager : MonoBehaviour, IManager
{
    public Item currentlyHeldItem;

    public void UpdateHeldItem(UISlotHandler currentSlot)
    {
        Item currentActiveItem = currentSlot.item;

        if(currentlyHeldItem != null && currentActiveItem != null && currentlyHeldItem.itemId == currentActiveItem.itemId)
        {
            currentSlot.inventoryManager.StackInInventory(currentSlot, currentlyHeldItem);
            currentlyHeldItem = null;
            return;
        }

        if(currentSlot.item != null)
        {
            currentSlot.inventoryManager.ClearItemSlot(currentSlot);
        }

        if(currentlyHeldItem != null)
        {
            currentSlot.inventoryManager.PlaceInInventory(currentSlot,currentlyHeldItem);
        }

        currentlyHeldItem = currentActiveItem;
    }

    public void PickUpFromStack(UISlotHandler currentSlot)
    {
        if(currentSlot.item == null)
        {
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
            currentSlot.inventoryManager.ClearItemSlot(currentSlot);
        }
    }
}
