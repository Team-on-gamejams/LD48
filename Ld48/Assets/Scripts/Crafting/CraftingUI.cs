using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CraftingUI : MonoBehaviour {
	[Header("Crafting data"), Space]
	public CraftSO.CraftPlaceType craftPlaceType;
	public float craftSpeedMod = 1.0f;
	public Inventory inventoryToAddItem;
	public Inventory inventoryToRemoveItem;

	[Header("Items Queue"), Space]
	public GameObject queueParent;
	Queue<CraftingQueueItemUI> itemsQueueUI = new Queue<CraftingQueueItemUI>(16);
	CraftingQueueItemUI currCraft;
	float currCraftTime;

	[Header("Refs prefabs"), Space]
	public GameObject recipeGoPrefab;
	public GameObject itemInQueuePrefab;

	List<CraftingItemUI> craftingItems;

	private void Awake() {
		craftingItems = new List<CraftingItemUI>(GameManager.Instance.crafts.Length);
		foreach (var craft in GameManager.Instance.crafts) {
			CraftingItemUI craftingItemUI = Instantiate(recipeGoPrefab, transform).GetComponent<CraftingItemUI>();
			craftingItemUI.Init(craft, craftPlaceType, OnClickOnItem);
			craftingItemUI.CheckIsEnoughIngradients(inventoryToRemoveItem);
			craftingItems.Add(craftingItemUI);
		}

		inventoryToRemoveItem.onInventoryChangeEvent += OnRemoveInventoryChange;
	}

	private void OnDestroy() {
		inventoryToRemoveItem.onInventoryChangeEvent -= OnRemoveInventoryChange;
	}

	private void Update() {
		if (currCraft) {
			if (currCraft.craft.craftTime > currCraftTime) {
				currCraftTime += Time.deltaTime * craftSpeedMod;
				currCraft.UpdateCraftTime(currCraftTime);
			}
				
			if (currCraft.craft.craftTime <= currCraftTime) {
				if (inventoryToAddItem.IsCanFitItems(currCraft.craft.results)) {
					currCraftTime -= currCraft.craft.craftTime;
					
					foreach (var result in currCraft.craft.results)
						inventoryToAddItem.AddItem(result.CloneItem());

					OnEndCraft();
				}
				else {
					//TODO: Inventory full popup text
				}
			}
		}
	}

	void OnRemoveInventoryChange() {
		foreach (var craftingItem in craftingItems) {
			craftingItem.CheckIsEnoughIngradients(inventoryToRemoveItem);
		}
	}

	public void OnClickOnItem(CraftSO craft) {
		CraftingQueueItemUI newCraft = Instantiate(itemInQueuePrefab, queueParent.transform).GetComponent<CraftingQueueItemUI>();

		newCraft.Init(craft, OnClickOnItemInQueue);

		if (currCraft == null)
			currCraft = newCraft;
		else
			itemsQueueUI.Enqueue(newCraft);

		foreach (var ingradient in currCraft.craft.ingradients)
			inventoryToRemoveItem.RemoveItem(ingradient.CloneItem());
	}

	void OnClickOnItemInQueue(CraftSO craft) {
		if (inventoryToRemoveItem.IsCanFitItems(currCraft.craft.ingradients)) {
			currCraftTime = 0;

			foreach (var ingradient in currCraft.craft.ingradients)
				inventoryToRemoveItem.AddItem(ingradient.CloneItem());

			OnEndCraft();
		}
		else {
			//TODO: Inventory full popup text
		}
	}

	void OnEndCraft() {
		Destroy(currCraft.gameObject);

		if (itemsQueueUI.Count != 0)
			currCraft = itemsQueueUI.Dequeue();
		else
			currCraft = null;
	}
}
