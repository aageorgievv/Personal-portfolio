using UnityEngine;

public class ShopInventory : Inventory
{
    protected override InventorySlot FindSlotForItem(Item item)
    {
        if (!ValidationUtility.ValidateReference(item, nameof(item)))
        {
            return null;
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

    protected override void SpawnStartingItems()
    {
        for (int i = 0; i < startingItems.Length; i++)
        {
            StartingItem startingItem = startingItems[i];

            if (startingItem.item == null)
            {
                continue;
            }


            Item item = startingItem.item.Create();
            AddItem(item);
        }
    }
}
