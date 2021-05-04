using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingPlace : MonoBehaviour {
	public Action<float> onUpdateCraftTimeFill;
	public Action onEndCraft;

	[Header("Crafting data"), Space]
	[SerializeField] CraftSO.CraftPlaceType craftPlaceType;
	[SerializeField] float craftSpeedMod = 1.0f;
	[SerializeField] Inventory inventoryToAddItem;
	[SerializeField] Inventory inventoryToRemoveItem;

	[Header("Refs"), Space]
	[SerializeField] CraftingUIPlayer ui;
	[SerializeField] SelectCraftUI selectUI;

	CraftSO currCraft;
	float currCraftTime;

	void Awake() {
		if(ui)
			ui.InitUI(this, inventoryToAddItem, inventoryToRemoveItem, craftPlaceType);
		if(selectUI)
			selectUI.InitUI(this, inventoryToAddItem, inventoryToRemoveItem, craftPlaceType);
	}

	void Update() {
		if (currCraft) {
			if (currCraft.craftTime > currCraftTime) {
				currCraftTime += Time.deltaTime * craftSpeedMod;
				onUpdateCraftTimeFill?.Invoke(currCraftTime / currCraft.craftTime);
			}

			if (currCraft.craftTime <= currCraftTime) {
				if (inventoryToAddItem.IsCanFitItems(currCraft.results)) {
					currCraftTime -= currCraft.craftTime;

					foreach (var result in currCraft.results)
						inventoryToAddItem.AddItem(result.CloneItem());

					currCraft = null;
					onEndCraft?.Invoke();
				}
				else {
					//TODO: Inventory full popup text if used by player
				}
			}
		}
	}

	public void Craft(CraftSO craftSO) {
		currCraft = craftSO;
	}

	public void AbortCraft() {
		currCraftTime = 0;
		currCraft = null;
		onEndCraft?.Invoke();
	}

	public void ResetCraftTime() {
		currCraftTime = 0;
	}
}
