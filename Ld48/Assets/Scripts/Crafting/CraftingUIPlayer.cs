using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CraftingUIPlayer : MonoBehaviour {
	[Header("UI"), Space]
	[SerializeField] bool isHideUnavaliablePlaceCrafts = true;

	[Header("Items Queue"), Space]
	[SerializeField] GameObject queueParent;
	Queue<CraftingQueueItemUI> itemsQueueUI = new Queue<CraftingQueueItemUI>(16);
	CraftingQueueItemUI currCraft;

	[Header("Refs prefabs"), Space]
	[SerializeField] GameObject recipeGoPrefab;
	[SerializeField] GameObject itemInQueuePrefab;

	Inventory inventoryToAddItem;
	Inventory inventoryToRemoveItem;
	CraftingPlace craftingPlace;

	List<CraftingItemUI> craftingItems;

	public void InitUI(CraftingPlace _craftingPlace, Inventory _inventoryToAddItem, Inventory _inventoryToRemoveItem, CraftSO.CraftPlaceType craftPlaceType) {
		craftingPlace = _craftingPlace;
		inventoryToAddItem = _inventoryToAddItem;
		inventoryToRemoveItem = _inventoryToRemoveItem;

		craftingItems = new List<CraftingItemUI>(GameManager.Instance.crafts.Length);
		foreach (var craft in GameManager.Instance.crafts) {
			if (isHideUnavaliablePlaceCrafts && !craft.place.HasFlag(craftPlaceType))
				continue;

			CraftingItemUI craftingItemUI = Instantiate(recipeGoPrefab, transform).GetComponent<CraftingItemUI>();
			craftingItemUI.Init(craft, craftPlaceType, OnClickOnItem);
			craftingItemUI.CheckIsEnoughIngradients(inventoryToRemoveItem);
			craftingItems.Add(craftingItemUI);
		}

		inventoryToRemoveItem.onInventoryChangeEvent += OnRemoveInventoryChange;
		craftingPlace.onUpdateCraftTimeFill += OnUpdateCraftTimeFill;
		craftingPlace.onEndCraft += OnEndCraft;
	}

	void OnDestroy() {
		inventoryToRemoveItem.onInventoryChangeEvent -= OnRemoveInventoryChange;
		craftingPlace.onUpdateCraftTimeFill -= OnUpdateCraftTimeFill;
		craftingPlace.onEndCraft -= OnEndCraft;
	}

	void OnRemoveInventoryChange() {
		foreach (var craftingItem in craftingItems) {
			craftingItem.CheckIsEnoughIngradients(inventoryToRemoveItem);
		}
	}

	void OnUpdateCraftTimeFill(float fill) {
		currCraft.UpdateCraftTime(fill);
	}

	void OnEndCraft() {
		Destroy(currCraft.gameObject);

		currCraft = null;
		while (currCraft == null && itemsQueueUI.Count != 0) {
			currCraft = itemsQueueUI.Dequeue();
		}

		if (currCraft != null) {
			craftingPlace.Craft(currCraft.craft);
		}
		else {
			craftingPlace.ResetCraftTime();
		}
	}

	void OnClickOnItem(CraftSO craft) {
		CraftingQueueItemUI newCraft = Instantiate(itemInQueuePrefab, queueParent.transform).GetComponent<CraftingQueueItemUI>();
		newCraft.Init(craft, OnClickOnItemInQueue);

		if (currCraft == null) {
			currCraft = newCraft;
			craftingPlace.Craft(currCraft.craft);
		}
		else {
			itemsQueueUI.Enqueue(newCraft);
		}

		foreach (var ingradient in craft.ingradients)
			inventoryToRemoveItem.RemoveItem(ingradient.CloneItem());
	}

	void OnClickOnItemInQueue(CraftingQueueItemUI craftItemUI) {
		foreach (var ingradient in craftItemUI.craft.ingradients) {
			ItemData leftItem = GameManager.Instance.player.inventory.AddItem(ingradient.CloneItem());

			if (leftItem.count != 0) {
				//TODO: Inventory full popup text
				Vector3 pos = GameManager.Instance.player.transform.position;
				ItemOnGround.CreateOnGround(leftItem, pos);
				leftItem.itemSO = null;
			}
		}

		if (craftItemUI == currCraft) {
			craftingPlace.AbortCraft();
		}
		else {
			Destroy(craftItemUI.gameObject);
		}
	}
}
