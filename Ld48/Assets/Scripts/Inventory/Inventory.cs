using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour {
	public Action onInventoryChange;

	[Header("Refs"), Space]
	[SerializeField] protected Inventory delegatedInventory;
	[SerializeField] protected InventoryItem[] items;

	protected virtual void Start() {
		InventoryItem[] items = GetComponentsInChildren<InventoryItem>(true);
		for (byte i = 0; i < items.Length; ++i) {
			items[i].id = i;
		}
	}

	public virtual ItemData AddItem(ItemData item) {
		item = AddItemToExistingStackOnly(item);
		if (item.count == 0)
			return item;

		if (delegatedInventory) {
			item = delegatedInventory.AddItem(item);
			if (item.count == 0)
				return item;
		}

		for (byte i = 0; i < items.Length; ++i) {
			if (items[i].item.itemSO == null) {
				items[i].item = item;
				items[i].DrawItem();
				return new ItemData();
			}
		}

		onInventoryChange?.Invoke();

		return item;
	}

	public virtual ItemData AddItemToExistingStackOnly(ItemData item) {
		if (delegatedInventory) {
			item = delegatedInventory.AddItemToExistingStackOnly(item);
			if (item.count == 0)
				return item;
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
					return item;
			}
		}

		onInventoryChange?.Invoke();

		return item;
	}

	public bool ContainsItem(ItemData item) {
		//ushort findCount = 0;

		//for (byte i = 0; i < items.Length; ++i)
		//	if (items[i]?.Type == item.Type && (findCount += items[i].Count) > item.Count)
		//		break;

		//for (byte i = 0; i < delegatedInventory.Items.Length; ++i)
		//	if (delegatedInventory.Items[i]?.Type == item.Type && (findCount += delegatedInventory.Items[i].Count) > item.Count)
		//		break;

		//return findCount >= item.Count;
		return false;
	}

	public virtual void RemoveItem(ItemData item) {
		//delegatedInventory?.RemoveItem(item);

		//for (byte i = 0; i < items.Length; ++i) {
		//	if (items[i]?.Type == item.Type) {
		//		if (items[i].Count >= item.Count) {
		//			items[i].Count -= item.Count;
		//			item.Count = 0;
		//		}
		//		else {
		//			item.Count -= items[i].Count;
		//			items[i] = null;
		//		}
		//	}

		//	if (item.Count == 0)
		//		break;
		//}

		onInventoryChange?.Invoke();
	}
}
