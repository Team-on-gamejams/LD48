using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Inventory : MonoBehaviour {
	public Action onInventoryChangeEvent;

	[Header("Init"), Space]
	[SerializeField] protected ItemData[] startItems;

	[Header("Data"), Space]
	[SerializeField] protected Inventory parentInventory;
	[SerializeField] protected Inventory delegatedInventory;

	[Header("Refs"), Space]
	[SerializeField] protected GameObject inventoryItemUIPrefab;

	protected int inventorySize = 10;
	protected InventoryItem[] items;

	protected virtual void Awake() {
		onInventoryChangeEvent += OnInventoryChange;
	}

	private void OnDestroy() {
		onInventoryChangeEvent -= OnInventoryChange;
	}

	public virtual void InitInvetory(int _inventorySize) {
		CleanInventoryBeforeReinit();

		inventorySize = _inventorySize;
		items = new InventoryItem[inventorySize];

		for (int i = 0; i < inventorySize; ++i) {
			InventoryItem inventoryItem = Instantiate(inventoryItemUIPrefab, transform).GetComponent<InventoryItem>();

			inventoryItem.inventory = this;
			inventoryItem.id = i;

			if (startItems != null && i < startItems.Length) {
				inventoryItem.item = startItems[i];
			}
			else {
				inventoryItem.item = new ItemData(null, 0);
			}

			items[i] = inventoryItem;
		}
	}

	public void SetFilter(int id, ItemSO itemso) {
		items[id].SetFilter(itemso);
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
			if (items[i].item.itemSO == null && items[i].IsCanContainItem(item.itemSO)) {
				items[i].item = item;
				items[i].DrawItem();
				onInventoryChangeEvent?.Invoke();
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

	public bool IsCanFitItems(ItemData[] items) {
		foreach (var item in items) {
			if (!IsCanFitItem(item))
				return false;
		}
		return true;
	}

	public bool IsCanFitItem(ItemData item) {
		int canFit = 0;

		for (byte i = 0; i < items.Length; ++i) {
			if (items[i].item.itemSO != null) {
				if (items[i].item.itemSO.type == item.itemSO.type)
					canFit += item.itemSO.maxCount - items[i].item.count;
			}
			else if(items[i].IsCanContainItem(item.itemSO)) {
				canFit += item.itemSO.maxCount;
			}

		}

		if (canFit < item.count && delegatedInventory != null) {
			for (byte i = 0; i < delegatedInventory.items.Length; ++i) {
				if (delegatedInventory.items[i].item.itemSO != null) {
					if (delegatedInventory.items[i].item.itemSO.type == item.itemSO.type)
						canFit += item.itemSO.maxCount - delegatedInventory.items[i].item.count;
				}
				else if (delegatedInventory.items[i].IsCanContainItem(item.itemSO)) {
					canFit += item.itemSO.maxCount;
				}
			}
		}

		return canFit >= item.count;
	}

	public bool CheckIsEnoughIngradients(CraftSO craft) {
		foreach (var ingradient in craft.ingradients)
			if (!ContainsItem(ingradient))
				return false;

		return true;
	}

	public bool ContainsItem(ItemData item) {
		int findCount = 0;

		for (byte i = 0; i < items.Length; ++i)
			if (items[i].item.itemSO != null && items[i].item.itemSO.type == item.itemSO.type && (findCount += items[i].item.count) > item.count)
				break;

		if (findCount < item.count && delegatedInventory != null)
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
			if (items[i].item.itemSO != null && items[i].item.itemSO.type == item.itemSO.type) {
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

	public void DropAllItemsToGround(Vector3 position, Vector2 rangeX, Vector2 rangeY) {
		if (items == null)
			return;

		foreach (var item in items) {
			if (item != null && item.item != null && item.item.itemSO != null) {
				ItemOnGround.CreateOnGround(
						item.item,
						position + new Vector3(rangeX.GetRandomValueFloat(), rangeY.GetRandomValueFloat())
				);

				item.item.itemSO = null;
				item.item.count = 0;
			}
		}
	}

	public void GiveAllItemsToPlayerOrDrop(Vector3 position, Vector2 rangeX, Vector2 rangeY) {
		if (items == null)
			return;

		foreach (var item in items) {
			if (item != null && item.item != null && item.item.itemSO != null) {
				ItemData leftItem = GameManager.Instance.player.inventory.AddItem(item.item);

				if (leftItem.count != 0) {
					//TODO: Inventory full popup text
					Vector3 pos = GameManager.Instance.player.transform.position;
					ItemOnGround.CreateOnGround(
						leftItem, 
						position + new Vector3(rangeX.GetRandomValueFloat(), rangeY.GetRandomValueFloat())
					);
					leftItem.itemSO = null;
				}

				item.item.itemSO = null;
				item.item.count = 0;
			}
		}
	}

	void OnInventoryChange() {
		if (parentInventory)
			parentInventory.onInventoryChangeEvent?.Invoke();
	}

	void CleanInventoryBeforeReinit() {
		if(items != null) {
			foreach (var item in items) {
				Destroy(item.gameObject);
			}
			items = null;
		}
	}
}
