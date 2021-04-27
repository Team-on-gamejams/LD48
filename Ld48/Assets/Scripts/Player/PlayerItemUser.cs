using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class PlayerItemUser : MonoBehaviour {

	[Header("Visual"), Space]
	[SerializeField] SpriteRenderer leftItemSr;
	[SerializeField] SpriteRenderer rightItemSr;
	[SerializeField] SpriteRenderer bothItemSr;

	[Header("Refs"), Space]
	[SerializeField] Hotbar hotbar;

	ItemData itemInBothHands;
	ItemData itemInLeftHand;
	ItemData itemInRightHand;

	bool isUseLeftItem, isUseRightItem;

	private void Awake() {
		hotbar.onChangeSelection += RedrawItemsInHand;
		hotbar.onInventoryChangeEvent += RedrawItemsInHand;
	}

	void Start() {
		RedrawItemsInHand();
	}

	private void OnDestroy() {
		hotbar.onChangeSelection -= RedrawItemsInHand;
		hotbar.onInventoryChangeEvent -= RedrawItemsInHand;
	}

	private void Update() {
		if (itemInBothHands != null) {
			if (isUseLeftItem && itemInLeftHand.itemSO != null) {
				bool needUIRedraw = itemInLeftHand.UseItemWhileInHotbar();
				if (needUIRedraw) {
					hotbar.UpdateItemLeftHand();
					hotbar?.onInventoryChangeEvent();
				}
			}

			if (isUseRightItem && itemInRightHand.itemSO != null) {
				bool needUIRedraw = itemInRightHand.UseItemWhileInHotbarDualWield();
				if (needUIRedraw) {
					hotbar.UpdateItemRightHand();
					hotbar?.onInventoryChangeEvent();
				}
			}
		}
		else {
			if (isUseLeftItem && itemInLeftHand.itemSO != null) {
				bool needUIRedraw = itemInLeftHand.UseItemWhileInHotbar();
				if (needUIRedraw) {
					hotbar.UpdateItemLeftHand();
					hotbar?.onInventoryChangeEvent();
				}
			}

			if (isUseRightItem && itemInRightHand.itemSO != null) {
				bool needUIRedraw = itemInRightHand.UseItemWhileInHotbar();
				if (needUIRedraw) {
					hotbar.UpdateItemRightHand();
					hotbar?.onInventoryChangeEvent();
				}
			}
		}
	}

	public void StartUseLeftItem() {
		isUseLeftItem = true;
	}

	public void StoptUseLeftItem() {
		isUseLeftItem = false;
	}

	public void StartUseRightItem() {
		isUseRightItem = true;
	}

	public void StoptUseRightItem() {
		isUseRightItem = false;
	}

	void RedrawItemsInHand() {
		itemInBothHands = hotbar.GetItemInBothHands();
		itemInLeftHand = hotbar.GetLeftItem();
		itemInRightHand = hotbar.GetRightItem();

		if (itemInBothHands != null && itemInBothHands.itemSO != null) {
			leftItemSr.color = leftItemSr.color.SetA(0.0f);
			rightItemSr.color = rightItemSr.color.SetA(0.0f);
			bothItemSr.color = bothItemSr.color.SetA(1.0f);
			bothItemSr.sprite = itemInBothHands.itemSO.sprite;
		}
		else {
			bothItemSr.color = bothItemSr.color.SetA(0.0f);
		
			if (itemInLeftHand.itemSO == null) {
				leftItemSr.color = leftItemSr.color.SetA(0.0f);
			}
			else {
				leftItemSr.color = leftItemSr.color.SetA(1.0f);
				leftItemSr.sprite = itemInLeftHand.itemSO.sprite;
			}

			if (itemInRightHand.itemSO == null) {
				rightItemSr.color = rightItemSr.color.SetA(0.0f);
			}
			else {
				rightItemSr.color = rightItemSr.color.SetA(1.0f);
				rightItemSr.sprite = itemInRightHand.itemSO.sprite;
			}
		}
	}
}
