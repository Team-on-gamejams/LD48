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

public class CraftingQueueItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
	[NonSerialized] public CraftSO craft;

	[Header("Refs"), Space]
	[SerializeField] protected Image itemImage;
	[SerializeField] protected Image fillCircle;
	[SerializeField] protected GameObject tooltipPrefab;

	protected Tooltip tooltip;

	Action<CraftingQueueItemUI> onClick;
	float currTime;

	private void Awake() {
		fillCircle.fillAmount = 0;
	}

	private void Start() {
		if (!tooltip && tooltipPrefab)
			tooltip = Instantiate(tooltipPrefab, transform).GetComponent<Tooltip>();
	}

	private void OnDestroy() {
		if (tooltip)
			Destroy(tooltip.gameObject);
	}

	public void Init(CraftSO _craft, Action<CraftingQueueItemUI> _onClick) {
		craft = _craft;
		onClick += _onClick;

		if (craft.resultSpriteOverride)
			itemImage.sprite = craft.resultSpriteOverride;
		else
			itemImage.sprite = craft.results[0].itemSO.sprite;
	}

	public void UpdateCraftTime(float fill) {
		currTime = craft.craftTime * fill;
		fillCircle.fillAmount = fill;

		if(tooltip.IsShowed)
			SetPopupText();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		tooltip.Show();
		SetPopupText();
	}

	public void OnPointerExit(PointerEventData eventData) {
		tooltip.Hide();
	}

	public void OnPointerDown(PointerEventData eventData) {
		onClick?.Invoke(this);
	}

	void SetPopupText() {
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

		if(currTime != 0) {
			popupText += $"Craft time: {currTime.ToString("0.0")}/{craft.craftTime}";
		}
		else {
			popupText += $"Craft time: {craft.craftTime}";
		}

		tooltip.SetText(popupText);
	}
}
