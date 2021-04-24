using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Hotbar : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] HotbarItem[] items;
	int selectedLeft;
	int selectedRight;

	void Awake() {
		selectedLeft = 0;
		selectedRight = 1;

		for(byte i = 0; i < items.Length; ++i) {
			items[i].id = i;
		}

		items[selectedLeft].SetSelectedFrame(false);
		items[selectedRight].SetSelectedFrame(true);
	}

	public void SetSelection(byte id, bool isLeftHand) {
		if (isLeftHand) {
			items[selectedLeft].RemoveSelectedFrame(false);
			selectedLeft = (byte)id;
			items[selectedLeft].SetSelectedFrame(false);
		}
		else {
			items[selectedRight].RemoveSelectedFrame(true);
			selectedRight = (byte)id;
			items[selectedRight].SetSelectedFrame(true);
		}
	}
}
