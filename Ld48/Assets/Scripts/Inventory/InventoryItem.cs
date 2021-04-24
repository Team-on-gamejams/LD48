using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
	[NonSerialized] public byte id;

	[Header("Refs self"), Space]
	[SerializeField] protected Image itemImage;
	[SerializeField] protected TextMeshProUGUI count;

	[Header("Refs"), Space]
	[SerializeField] protected Inventory inventory;

	protected virtual void Awake() {
		itemImage.sprite = null;
		itemImage.color = itemImage.color.SetA(0.0f);
		count.text = "";
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
}
