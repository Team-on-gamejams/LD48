using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour {
	[Header("Drag"), Space]
	public GameObject parentForDraggedSlot;
	bool isDrag;

	[Header("Refs"), Space]
	[SerializeField] protected Inventory delegatedInventory;
	[SerializeField] protected InventoryItem[] items;
	[SerializeField] protected CanvasGroup cg;

	bool isShowed;

	protected virtual void Start() {
		InventoryItem[] items = GetComponentsInChildren<InventoryItem>(true);
		for (byte i = 0; i < items.Length; ++i) {
			items[i].id = i;
		}

		if (cg) {
			isShowed = false;
			cg.blocksRaycasts = cg.interactable = false;
			cg.alpha = 0.0f;
		}
		else {
			isShowed = true;
		}
	}

	public void ToggleVisible() {
		if (!cg)
			return;

		LeanTween.cancel(gameObject, false);

		if (isShowed) {
			isShowed = false;
			cg.blocksRaycasts = cg.interactable = false;
			LeanTweenEx.ChangeAlpha(cg, 0.0f, 0.1f);
		}
		else {
			isShowed = true;
			cg.blocksRaycasts = cg.interactable = true;
			LeanTweenEx.ChangeAlpha(cg, 1.0f, 0.1f);
		}
	}
}
