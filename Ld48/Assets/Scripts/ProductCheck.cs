using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ProductCheck : MonoBehaviour
{

    public DungeonObject ObjectType = DungeonObject.Null;
    public int price;
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(IsPressed);
    }

    public void IsPressed()
    {
        GameManager.Instance.player.PlayerInventory.Add(ObjectType);
        GameManager.Instance.player.Wallet-=price;
        Debug.Log("Bought " + ObjectType);
    }
}
