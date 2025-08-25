using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlotHandler : MonoBehaviour, IPointerClickHandler
{
    public ItemData item;
    public Image icon;
    public TMP_Text itemCountText;

    private MouseManager mouseManager;

    private void Awake()
    {
        //GameManager.ExecuteWhenInitialized(HandleGameManagerInitialized);
    }

    private void HandleGameManagerInitialized()
    {
        //mouseManager = GameManager.GetManager<MouseManager>();
        //ValidationUtility.ValidateReference(mouseManager, nameof(mouseManager));
    }

    private void Start()
    {
        //if (item != null)
        //{
        //    item = item.Clone();
        //    icon.sprite = item.itemSprite;
        //    //itemCountText.text = item.itemCount.ToString();
        //}
        //else
        //{
        //    icon.gameObject.SetActive(false);
        //    itemCountText.text = string.Empty;
        //}
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (eventData.button == PointerEventData.InputButton.Right)
        //{
        //    if (!ValidationUtility.ValidateReference(item, nameof(item)))
        //    {
        //        return;
        //    }

        //    mouseManager.PickUpFromStack(this);
        //    return;
        //}
        //mouseManager.UpdateHeldItem(this);
    }
}
