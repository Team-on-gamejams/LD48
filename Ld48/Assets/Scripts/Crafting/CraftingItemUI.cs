using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CraftingItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
	[Header("Refs"), Space]
	[SerializeField] protected Image itemImage;
	[SerializeField] protected GameObject tooltipPrefab;

	protected Tooltip tooltip;

	bool isNeededPlace = false;
	bool isEnoughIngradients;
	CraftSO craft;
	Action<CraftSO> onClick;

	private void Awake() {
		if (!tooltip && tooltipPrefab)
			tooltip = Instantiate(tooltipPrefab, transform).GetComponent<Tooltip>();
	}

	private void OnDestroy() {
		if (tooltip)
			Destroy(tooltip.gameObject);
	}

	public void Init(CraftSO _craft, CraftSO.CraftPlaceType _myPlace, Action<CraftSO> _onClick) {
		craft = _craft;
		isNeededPlace = craft.place.HasFlag(_myPlace);
		onClick += _onClick;

		isEnoughIngradients = true;
		UpdateVisual();
	}

	public void CheckIsEnoughIngradients(Inventory inventory) {
		isEnoughIngradients = inventory.CheckIsEnoughIngradients(craft);
		UpdateVisual();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		tooltip.Show();
	}

	public void OnPointerExit(PointerEventData eventData) {
		tooltip.Hide();
	}

	public void OnPointerDown(PointerEventData eventData) {
		if (isNeededPlace) {
			if (isEnoughIngradients) {
				onClick?.Invoke(craft);
			}
			else {
				//TODO: popup - not Enough Ingradients
			}
		}
		else {
			//TODO: popup - craft this in other place
		}
	}

	void UpdateVisual() {
		if (craft.resultSpriteOverride)
			itemImage.sprite = craft.resultSpriteOverride;
		else
			itemImage.sprite = craft.results[0].itemSO.sprite;

		if (!isNeededPlace) {
			itemImage.color = Color.gray.SetA(0.5f);
		}
		else {
			if (isEnoughIngradients) {
				itemImage.color = Color.white;
			}
			else {
				itemImage.color = Color.gray.SetA(0.5f);
			}
		}

		string popupText = "";

		popupText += $"<b>Results:</b>\n";
		foreach (var result in craft.results) {
			if (result.count == 1) {
				popupText += $"{result.itemSO.name}\n";
			}
			else {
				popupText += $"{result.count} x {result.itemSO.name}\n";
			}
		}
		popupText += "\n";

		popupText += $"<b>Ingradients:</b>\n";
		foreach (var ingradient in craft.ingradients) {
			if (ingradient.count == 1) {
				popupText += $"{ingradient.itemSO.name}\n";
			}
			else {
				popupText += $"{ingradient.count} x {ingradient.itemSO.name}\n";
			}
		}
		popupText += "\n";

		popupText += $"Craft tool: {craft.GetCraftPlaceTypeString()}\n";
		popupText += $"Craft time: {craft.craftTime}";

		tooltip.SetText(popupText);
	}
}
