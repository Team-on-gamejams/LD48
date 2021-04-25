using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
	[NonSerialized] public byte id;

	public ItemData item;

	[Header("Refs self"), Space]
	[SerializeField] protected Image itemImage;
	[SerializeField] protected TextMeshProUGUI count;

	[Header("Refs"), Space]
	[SerializeField] protected Inventory inventory;

	protected virtual void Awake() {
		itemImage.sprite = null;
		itemImage.color = itemImage.color.SetA(0.0f);
		count.text = "";

		if(item != null && item.itemSO != null) {
			DrawItem();
		}
	}

	public void OnBeginDrag(PointerEventData eventData) {
		throw new NotImplementedException();
	}

	public void OnDrag(PointerEventData eventData) {
		throw new NotImplementedException();
	}

	public void OnDrop(PointerEventData eventData) {
		throw new NotImplementedException();
	}

	public void OnEndDrag(PointerEventData eventData) {
		throw new NotImplementedException();
	}

	public void DrawItem() {
		itemImage.sprite = item.itemSO.sprite;
		count.text = item.count.ToString();

		LeanTween.cancel(itemImage.gameObject, false);
		LeanTweenEx.ChangeAlpha(itemImage, 1.0f, 0.05f);
	}
}
