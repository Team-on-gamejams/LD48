using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
	static protected InventoryItem draggingSlot;

	[NonSerialized] public byte id;

	public ItemData item;

	[Header("Refs self"), Space]
	[SerializeField] protected Image itemImage;
	[SerializeField] protected TextMeshProUGUI count;

	[Header("Refs"), Space]
	[SerializeField] protected Inventory inventory;

	protected virtual void Awake() {
		DrawItem();
	}

	public void OnBeginDrag(PointerEventData eventData) {
		if (item.itemSO == null)
			return;

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

		Vector3 newItemPos = TemplateGameManager.Instance.Camera.ScreenToWorldPoint(eventData.position);
		Vector3 diff = Vector3.ClampMagnitude(newItemPos - GameManager.Instance.player.transform.position, GameManager.Instance.player.maxInteractDistance); ;

		//OnGroundItem.CreateOnGround(item, newItemPos);
		item.itemSO = null;
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
	}

	public void DrawItem() {
		LeanTween.cancel(itemImage.gameObject, false);

		itemImage.transform.SetParent(transform, true);
		count.transform.SetParent(transform, true);

		itemImage.transform.localPosition = Vector3.zero;
		count.rectTransform.anchoredPosition = Vector3.zero;

		if (item.itemSO != null) {
			if (item.itemSO.maxCount == 1) {
				count.text = "";
			}
			else {
				count.text = item.count.ToString();
			}

			itemImage.sprite = item.itemSO.sprite;
			LeanTweenEx.ChangeAlpha(itemImage, 1.0f, 0.05f);
		}
		else {
			count.text = "";
			
			itemImage.sprite = null;
			itemImage.color = itemImage.color.SetA(0.0f);
		}
	}
}
