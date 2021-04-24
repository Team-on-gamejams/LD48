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
	int selectedLeft;
	int selectedRight;

	void Awake() {
		selectedLeft = 0;
		selectedRight = 1;

		(items[selectedLeft] as HotbarItem).SetSelectedFrame(true);
		(items[selectedRight] as HotbarItem).SetSelectedFrame(false);
	}

	public void SetSelection(byte id, bool isLeftHand) {
		if (isLeftHand) {
			(items[selectedLeft] as HotbarItem).RemoveSelectedFrame(true);
			selectedLeft = (byte)id;
			(items[selectedLeft] as HotbarItem).SetSelectedFrame(true);
		}
		else {
			(items[selectedRight] as HotbarItem).RemoveSelectedFrame(false);
			selectedRight = (byte)id;
			(items[selectedRight] as HotbarItem).SetSelectedFrame(false);
		}
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
	}
}
