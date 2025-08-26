using UnityEngine;

public class PlayerInventory : Inventory
{
    protected override InventorySlot FindSlotForItem(Item item)
    {
        if (!ValidationUtility.ValidateReference(item, nameof(item)))
        {
            return null;
        }

        // return first slot with the item
        foreach (InventorySlot slot in slots)
        {
            if (!slot.HasItem)
            {
                continue;
            }

            if (slot.Item.Name == item.Name)
            {
                return slot;
            }
        }

        // return first empty slot
        foreach (InventorySlot slot in slots)
        {
            if (!slot.HasItem)
            {
                return slot;
            }
        }

        // no free slots were found
        Debug.LogError($"Inventory is full");
        return null;
    }
}
