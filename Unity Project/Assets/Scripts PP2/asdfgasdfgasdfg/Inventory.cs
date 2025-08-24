using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    [SerializeField]
    private InventorySlot slotPrefab;

    [SerializeField]
    private Transform slotParent;

    [SerializeField, Range(0, 18)]
    private int numSlots = 18;
    [SerializeField]
    protected StartingItem[] startingItems = Array.Empty<StartingItem>();

    protected readonly List<InventorySlot> slots = new List<InventorySlot>();

    protected virtual void Awake()
    {
        ValidationUtility.ValidateReference(slotPrefab, nameof(slotPrefab));
        ValidationUtility.ValidateReference(slotParent, nameof(slotParent));
        ValidationUtility.ValidateReference(startingItems, nameof(startingItems));

        for (int i = 0; i < startingItems.Length; i++)
        {
            ValidationUtility.ValidateReference(startingItems[i], $"{nameof(startingItems)}[{i}]");
        }

        slotPrefab.gameObject.SetActive(false);

        SpawnSlots();
        SpawnStartingItems();
    }
    public void AddItem(Item item)
    {
        InventorySlot itemSlot = FindSlotForItem(item);
        if (itemSlot == null)
        {
            return;
        }

        itemSlot.AddItem(item);
    }

    protected abstract InventorySlot FindSlotForItem(Item item);

    private void SpawnSlots()
    {
        for (int i = 0; i < numSlots; i++)
        {
            InventorySlot slot = Instantiate(slotPrefab);
            slot.transform.SetParent(slotParent, false);
            slot.gameObject.SetActive(true);
            slots.Add(slot);
        }
    }

    protected virtual void SpawnStartingItems()
    {
        for (int i = 0; i < startingItems.Length; i++)
        {
            StartingItem startingItem = startingItems[i];

            if (startingItem.item == null)
            {
                continue;
            }

            for (int j = 0; j < startingItem.count; j++)
            {
                Item item = startingItem.item.Create();
                AddItem(item);
            }
        }
    }
}
