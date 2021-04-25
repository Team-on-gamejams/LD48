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

	private void Awake() {
		hotbar.onChangeSelection += RedrawItemsInHand;
	}

	void Start() {
		RedrawItemsInHand();
	}

	private void OnDestroy() {
		hotbar.onChangeSelection -= RedrawItemsInHand;
	}

	public void StartUseLeftItem() {

	}

	public void StoptUseLeftItem() {

	}

	public void StartUseRightItem() {

	}

	public void StoptUseRightItem() {

	}

	void RedrawItemsInHand() {
		ItemData? itemInBothHands = hotbar.GetItemInBothHands();

		if (itemInBothHands.HasValue) {
			leftItemSr.color = leftItemSr.color.SetA(0.0f);
			rightItemSr.color = rightItemSr.color.SetA(0.0f);
			bothItemSr.color = bothItemSr.color.SetA(1.0f);
			bothItemSr.sprite = itemInBothHands.Value.itemSO.sprite;
		}
		else {
			bothItemSr.color = bothItemSr.color.SetA(0.0f);
		
			if (hotbar.GetLeftItem().itemSO == null) {
				leftItemSr.color = leftItemSr.color.SetA(0.0f);
			}
			else {
				leftItemSr.color = leftItemSr.color.SetA(1.0f);
				leftItemSr.sprite = hotbar.GetLeftItem().itemSO.sprite;
			}

			if (hotbar.GetRightItem().itemSO == null) {
				rightItemSr.color = rightItemSr.color.SetA(0.0f);
			}
			else {
				rightItemSr.color = rightItemSr.color.SetA(1.0f);
				rightItemSr.sprite = hotbar.GetRightItem().itemSO.sprite;
			}
		}
	}
}
