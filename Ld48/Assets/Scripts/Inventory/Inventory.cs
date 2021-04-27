using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour {
	public Action onInventoryChangeEvent;

	[Header("Refs"), Space]
	[SerializeField] protected Inventory parentInventory;
	[SerializeField] protected Inventory delegatedInventory;
	[SerializeField] protected InventoryItem[] items;

	private void Awake() {
		onInventoryChangeEvent += OnInventoryChange;
	}

	private void OnDestroy() {
		onInventoryChangeEvent -= OnInventoryChange;
	}

	protected virtual void Start() {
		InventoryItem[] items = GetComponentsInChildren<InventoryItem>(true);
		for (byte i = 0; i < items.Length; ++i) {
			items[i].id = i;
		}
	}

	public virtual ItemData AddItem(ItemData item) {
		item = AddItemToExistingStackOnly(item);
		if (item.count == 0) {
			onInventoryChangeEvent?.Invoke();
			return item;
		}

		if (delegatedInventory) {
			item = delegatedInventory.AddItem(item);
			if (item.count == 0) {
				onInventoryChangeEvent?.Invoke();
				return item;
			}
		}

		for (byte i = 0; i < items.Length; ++i) {
			if (items[i].item.itemSO == null) {
				items[i].item = item;
				items[i].DrawItem();
				return new ItemData();
			}
		}

		onInventoryChangeEvent?.Invoke();
		return item;
	}

	public virtual ItemData AddItemToExistingStackOnly(ItemData item) {
		if (delegatedInventory) {
			item = delegatedInventory.AddItemToExistingStackOnly(item);
			if (item.count == 0) {
				onInventoryChangeEvent?.Invoke();
				return item;
			}
		}

		for (byte i = 0; i < items.Length; ++i) {
			if (items[i].item.itemSO != null && items[i].item.itemSO.type == item.itemSO.type) {
				if (!items[i].item.IsMaxStack()) {
					items[i].item.count += item.count;
					item.count = 0;
					if (items[i].item.count > items[i].item.itemSO.maxCount) {
						item.count = items[i].item.count - items[i].item.itemSO.maxCount;
						items[i].item.count = items[i].item.itemSO.maxCount;
					}
					items[i].DrawItem();
				}

				if (item.count == 0) 
					break;
			}
		}

		onInventoryChangeEvent?.Invoke();
		return item;
	}

	public bool ContainsItem(ItemData item) {
		int findCount = 0;

		for (byte i = 0; i < items.Length; ++i)
			if (items[i].item.itemSO != null && items[i].item.itemSO.type == item.itemSO.type && (findCount += items[i].item.count) > item.count)
				break;

		if(findCount < item.count && delegatedInventory != null)
			for (byte i = 0; i < delegatedInventory.items.Length; ++i)
				if (delegatedInventory.items[i].item.itemSO != null && delegatedInventory.items[i].item.itemSO.type == item.itemSO.type && (findCount += delegatedInventory.items[i].item.count) > item.count)
					break;

		return findCount >= item.count;
	}

	public virtual ItemData RemoveItem(ItemData item) {
		if (delegatedInventory) {
			item = delegatedInventory.RemoveItem(item);
			if (item.count == 0) {
				onInventoryChangeEvent?.Invoke();
				return item;
			}
		}

		for (byte i = 0; i < items.Length; ++i) {
			if(items[i].item.itemSO != null && items[i].item.itemSO.type == item.itemSO.type) {
				if (items[i].item.count > item.count) {
					items[i].item.count -= item.count;

					item.count = 0;
					items[i].DrawItem();
				}
				else {
					item.count -= items[i].item.count;

					items[i].item.count = 0;
					items[i].item.itemSO = null;
					items[i].DrawItem();
				}
			}

			if (item.count == 0)
				break;
		}

		onInventoryChangeEvent?.Invoke();

		return item;
	}

	void OnInventoryChange() {
		if (parentInventory)
			parentInventory.onInventoryChangeEvent?.Invoke();
	}
}
