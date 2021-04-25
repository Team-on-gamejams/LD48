using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
	static protected InventoryItem draggingSlot;

	[NonSerialized] public byte id;

	public ItemData item;

	[Header("Refs self"), Space]
	[SerializeField] protected Image itemImage;
	[SerializeField] protected TextMeshProUGUI count;

	[Header("Refs"), Space]
	[SerializeField] protected Inventory inventory;
	[SerializeField] protected Popup popup;

	protected virtual void Awake() {
		DrawItem();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		if (item.itemSO != null)
			popup.Show();
	}

	public void OnPointerExit(PointerEventData eventData) {
		if (item.itemSO != null)
			popup.Hide();
	}

	public void OnBeginDrag(PointerEventData eventData) {
		if (item.itemSO == null)
			return;

		popup.Hide();

		draggingSlot = this;

		itemImage.transform.SetParent(GameManager.Instance.draggedParent.transform, true);
		count.transform.SetParent(GameManager.Instance.draggedParent.transform, true);
		itemImage.raycastTarget = false;
		count.raycastTarget = false;

		itemImage.transform.position += (Vector3)eventData.delta;
		count.transform.position += (Vector3)eventData.delta;
	}

	public void OnDrag(PointerEventData eventData) {
		if (item.itemSO == null)
			return;

		itemImage.transform.position += (Vector3)eventData.delta;
		count.transform.position += (Vector3)eventData.delta;
	}

	public void OnEndDrag(PointerEventData eventData) {
		if (item.itemSO == null)
			return;

		if (eventData.hovered.Count != 0) {
			DrawItem();
			return;
		}

		Cell cell = GameManager.Instance.GetCellAtMousePosWithInteractClamp(out Vector2 dropPos);

		if(cell && cell.foregroud == Cell.CellContentForegroud.None) {
			ItemOnGround.CreateOnGround(item, dropPos);
			item.itemSO = null;
			inventory?.onInventoryChange();
		}

		DrawItem();
	}

	public void OnDrop(PointerEventData eventData) {
		if (draggingSlot == this || draggingSlot.item.itemSO == null)
			return;

		if (item.itemSO == null || (item.itemSO.type != draggingSlot.item.itemSO.type) || item.IsMaxStack() || draggingSlot.item.IsMaxStack()) {
			ItemData temp = item;
			item = draggingSlot.item;
			draggingSlot.item = temp;
		}
		else if (item.itemSO.type == draggingSlot.item.itemSO.type) {
			item.count += draggingSlot.item.count;
			if (item.count > item.itemSO.maxCount) {
				draggingSlot.item.count = item.count - item.itemSO.maxCount;
				item.count = item.itemSO.maxCount;
			}
			else {
				draggingSlot.item.itemSO = null;
			}
		}

		DrawItem();
		draggingSlot.DrawItem();

		draggingSlot = null;
		inventory?.onInventoryChange();
	}

	public void DrawItem() {
		LeanTween.cancel(itemImage.gameObject, false);

		itemImage.transform.SetParent(transform, true);
		itemImage.transform.SetAsFirstSibling();
		count.transform.SetParent(transform, true);
		count.transform.SetAsLastSibling();

		itemImage.transform.localPosition = Vector3.zero;
		count.rectTransform.anchoredPosition = Vector3.zero;

		if (item.itemSO != null) {
			if (item.itemSO.maxCount == 1) {
				count.text = "";
			}
			else {
				count.text = item.count.ToString();
			}

			if(item.itemSO.maxCount == 1)
				popup.SetText($"<b>{item.itemSO.name}</b>\n\n{item.itemSO.description}\n\nNot stackable");
			else
				popup.SetText($"<b>{item.itemSO.name}</b>\n\n{item.itemSO.description}\n\nMax Stack: {item.itemSO.maxCount}");

			itemImage.sprite = item.itemSO.sprite;
			LeanTweenEx.ChangeAlpha(itemImage, 1.0f, 0.05f);
		}
		else {
			count.text = "";

			popup.SetText("");
			
			itemImage.sprite = null;
			itemImage.color = itemImage.color.SetA(0.0f);
		}
	}
}
