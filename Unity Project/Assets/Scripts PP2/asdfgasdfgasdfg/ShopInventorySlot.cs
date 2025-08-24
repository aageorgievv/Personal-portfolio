using TMPro;
using UnityEngine;

public class ShopInventorySlot : InventorySlot
{
    [SerializeField]
    private TMP_Text coins;

    protected override void Awake()
    {
        base.Awake();
        ValidationUtility.ValidateReference(coins, nameof(coins));
    }
}
