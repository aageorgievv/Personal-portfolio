using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIHandler : MonoBehaviour
{
    public bool IsShopOpen {  get; private set; }

    [SerializeField] private GameObject shopUI;

    public void ToggleShop()
    {
        IsShopOpen = !IsShopOpen;
        shopUI.SetActive(IsShopOpen);
    }
}
