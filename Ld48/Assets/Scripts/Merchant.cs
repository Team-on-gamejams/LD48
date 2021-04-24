using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Product
{
    public GameObject ProductToGo;
    public int price;
    public DungeonObject instrument;
}

public enum Mode { Buy, Sell }
public class Merchant : MonoBehaviour
{
    public Mode WorkMode;
    public List<Product> Products = new List<Product>();
    public GameObject BuyingMenu;
    public Transform parent;

    private void Start()
    {
        GameObject obj;
        for (int i = 0; i < Products.Capacity; i++)
        {
            obj = Instantiate(Products[i].ProductToGo, parent);
            ProductCheck check = obj.GetComponent<ProductCheck>();
            if (WorkMode == Mode.Buy)
                check.price = Products[i].price;
            else
                check.price = Products[i].price * -1;
            check.ObjectType = Products[i].instrument;
        }
    }
    public void OnMouseDown()
    {
        if (BuyingMenu.activeSelf)
            BuyingMenu.SetActive(false);
        else
            BuyingMenu.SetActive(true);
        Debug.Log("buy");
    }
}
