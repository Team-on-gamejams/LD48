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
public class ItemData {
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

				GameManager.Instance.SelectedCell.foregroud = GameManager.Instance.GetItemPlacable(itemSO.type).foregroud;
				GameManager.Instance.SelectedCell.RecreateVisualAfterChangeType();

				--count;
				if (count <= 0) {
					itemSO = null;
				}

				return true;

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
}
