using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

[Serializable]
public class ItemData : ICloneable {
	public ItemSO itemSO;
	public int count;

	public ItemData() {

	}

	public ItemData(ItemSO itemSO, int count) {
		this.itemSO = itemSO;
		this.count = count;
	}

	public bool IsMaxStack() {
		return count == itemSO.maxCount;
	}

	public bool UseItemWhileInHotbar() {
		switch (itemSO.metaType) {
			case ItemSO.ItemMetaType.BuildableForeground:
				if (!GameManager.Instance.SelectedCell || GameManager.Instance.SelectedCell.foregroud != Cell.CellContentForegroud.None)
					return false;

				PlacebleBlock placebleBlock = GameManager.Instance.GetItemPlacable(itemSO.type);

				if (placebleBlock.isCanPlaceOnPlayerPos || !GameManager.Instance.SelectedCell.IsPlayerInside()) {
					GameManager.Instance.SelectedCell.foregroud = placebleBlock.foregroud;
					GameManager.Instance.SelectedCell.RecreateVisualAfterChangeType();

					--count;
					if (count <= 0) {
						itemSO = null;
					}

					return true;
				}
				break;

			case ItemSO.ItemMetaType.MiningTool:
				if (!GameManager.Instance.SelectedCell)
					return false;

				if (
					(GameManager.Instance.SelectedCell.foregroud != Cell.CellContentForegroud.None && itemSO.miningForce >= GameManager.Instance.SelectedCell.foregroundBlock.neededForceToBroke) || 
					(GameManager.Instance.SelectedCell.ore != Cell.CellContentOre.None && itemSO.miningForce >= GameManager.Instance.SelectedCell.oreBlock.neededForceToBroke)
					) {
					GameManager.Instance.SelectedCell.Mine(Time.deltaTime, itemSO.miningForce);
				}
				break;

			default:
				Debug.LogError("Unsupported ItemMetaType for using");
				break;
		}

		return false;
	}

	public bool UseItemWhileInHotbarDualWield() {
		switch (itemSO.metaType) {
			case ItemSO.ItemMetaType.BuildableForeground:
				return UseItemWhileInHotbar();

			case ItemSO.ItemMetaType.MiningTool:
				if (!GameManager.Instance.SelectedCell)
					return false;

				if (
					(GameManager.Instance.SelectedCell.foregroud != Cell.CellContentForegroud.None && itemSO.miningForce >= GameManager.Instance.SelectedCell.foregroundBlock.neededForceToBroke) ||
					(GameManager.Instance.SelectedCell.ore != Cell.CellContentOre.None && itemSO.miningForce >= GameManager.Instance.SelectedCell.oreBlock.neededForceToBroke)
					) {
					GameManager.Instance.SelectedCell.Mine(Time.deltaTime, itemSO.miningForce + 2, false);
				}
				break;

			default:
				Debug.LogError("Unsupported ItemMetaType for using");
				break;
		}

		return false;
	}

	public string GetInfoForPopup() {
		string popupText = "";
		popupText += $"<b>{itemSO.name}</b>\n\n";
		popupText += $"{itemSO.description}\n\n";
		if (itemSO.maxCount == 1)
			popupText += $"Not stackable\n\n";
		else
			popupText += $"Max Stack: {itemSO.maxCount}\n\n";

		popupText += $"Meta type: {itemSO.metaType}";

		switch (itemSO.metaType) {
			case ItemSO.ItemMetaType.MiningTool:
				popupText += $"\nMining force: {itemSO.miningForce}";
				break;
		}

		return popupText;
	}

	public object Clone() {
		return MemberwiseClone();
	}

	public ItemData CloneItem() {
		return Clone() as ItemData;
	}

	public ItemData SetCount(int _count) {
		count = _count;
		return this;
	}
}
