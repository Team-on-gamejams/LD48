using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Hotbar : Inventory {
	public Action onChangeSelection;

	int selectedLeft;
	int selectedRight;

	protected override void Awake() {
		base.Awake();

		selectedLeft = 0;
		selectedRight = 1;

		(items[selectedLeft] as HotbarItem).SetSelectedFrame(true);
		(items[selectedRight] as HotbarItem).SetSelectedFrame(false);
	}

	public ItemData? GetItemInBothHands() {
		if(selectedLeft == selectedRight)
			return items[selectedLeft].item;
		return null;
	}

	public ItemData GetLeftItem() {
		return items[selectedLeft].item;
	}

	public ItemData GetRightItem() {
		return items[selectedRight].item;
	}

	public void UpdateItemLeftHand() {
		items[selectedLeft].DrawItem();
	}

	public void UpdateItemRightHand() {
		items[selectedRight].DrawItem();
	}

	public void SetSelection(int id, bool isLeftHand) {
		if (isLeftHand) {
			(items[selectedLeft] as HotbarItem).RemoveSelectedFrame(true);
			selectedLeft = id;
			(items[selectedLeft] as HotbarItem).SetSelectedFrame(true);
		}
		else {
			(items[selectedRight] as HotbarItem).RemoveSelectedFrame(false);
			selectedRight = id;
			(items[selectedRight] as HotbarItem).SetSelectedFrame(false);
		}

		onChangeSelection?.Invoke();
	}

	public void MoveSelectionUp(bool isLeftHand) {
		if (isLeftHand) {
			(items[selectedLeft] as HotbarItem).RemoveSelectedFrame(true);

			++selectedLeft;
			if (selectedLeft >= items.Length)
				selectedLeft = 0;

			(items[selectedLeft] as HotbarItem).SetSelectedFrame(true);
		}
		else {
			(items[selectedRight] as HotbarItem).RemoveSelectedFrame(false);

			++selectedRight;
			if (selectedRight >= items.Length)
				selectedRight = 0;

			(items[selectedRight] as HotbarItem).SetSelectedFrame(false);
		}

		onChangeSelection?.Invoke();
	}

	public void MoveSelectionDown(bool isLeftHand) {
		if (isLeftHand) {
			(items[selectedLeft] as HotbarItem).RemoveSelectedFrame(true);

			--selectedLeft;
			if (selectedLeft < 0)
				selectedLeft = items.Length - 1;

			(items[selectedLeft] as HotbarItem).SetSelectedFrame(true);
		}
		else {
			(items[selectedRight] as HotbarItem).RemoveSelectedFrame(false);

			--selectedRight;
			if (selectedRight < 0)
				selectedRight = items.Length - 1;

			(items[selectedRight] as HotbarItem).SetSelectedFrame(false);
		}

		onChangeSelection?.Invoke();
	}
}
