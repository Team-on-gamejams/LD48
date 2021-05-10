using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
	static protected InventoryItem draggingSlot;

	[NonSerialized] public int id;
	[NonSerialized] public Inventory inventory;

	[NonSerialized] public ItemData item;
	[NonSerialized] public ItemSO filter;

	[Header("Refs self"), Space]
	[SerializeField] protected Image itemImage;
	[SerializeField] protected Image itemFilterImage;
	[SerializeField] protected TextMeshProUGUI count;

	[Header("Refs"), Space]
	[SerializeField] protected Popup popup;

	protected virtual void Awake() {

	}

	protected virtual void Start() {
		DrawItem();
	}

	private void OnDestroy() {
		if (popup)
			Destroy(popup.gameObject);
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
		if (item == null || item.itemSO == null)
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
		if (draggingSlot == null)
			return;

		itemImage.transform.position += (Vector3)eventData.delta;
		count.transform.position += (Vector3)eventData.delta;
	}

	public void OnEndDrag(PointerEventData eventData) {
		if (draggingSlot == null)
			return;

		if (eventData.hovered.Count == 0) {
			Cell cell = GameManager.Instance.GetCellAtMousePosWithInteractClamp(out Vector2 dropPos);
			
			if (cell && cell.foregroud == Cell.CellContentForegroud.None) {
				ItemOnGround.CreateOnGround(item, dropPos);
				item.itemSO = null;
			}
		}

		draggingSlot = null;
		DrawItem();
		inventory?.onInventoryChangeEvent();
	}

	public void OnDrop(PointerEventData eventData) {
		if (draggingSlot == null || draggingSlot == this|| draggingSlot.item.itemSO == null) {
			draggingSlot = null;
			DrawItem();
			inventory?.onInventoryChangeEvent();
			return;
		}

		if (!IsCanContainItem(draggingSlot.item.itemSO)) {
			DrawItem();
			inventory?.onInventoryChangeEvent();
			return;
		}

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
		inventory.onInventoryChangeEvent?.Invoke();
	}

	public void SetFilter(ItemSO itemso) {
		filter = itemso;
	}

	public bool IsCanContainItem(ItemSO itemso) {
		if (!filter)
			return true;

		switch (filter.metaType) {
			case ItemSO.ItemMetaType.BuildableForeground:
				return itemso.type == filter.type;
			case ItemSO.ItemMetaType.MiningTool:
				return itemso.type == filter.type;
			default:
				Debug.LogWarning("Unsupported ItemMetaType");
				break;
		}

		return false;
	}

	public void DrawItem() {
		LeanTween.cancel(itemImage.gameObject, false);

		if(draggingSlot != this) {
			itemImage.transform.SetParent(transform, true);
			itemImage.transform.SetAsFirstSibling();
			count.transform.SetParent(transform, true);
			count.transform.SetAsLastSibling();

			itemImage.transform.localPosition = Vector3.zero;
			count.rectTransform.anchoredPosition = Vector3.zero;
		}

		if (filter) {
			itemFilterImage.sprite = filter.sprite;
			itemFilterImage.color = itemFilterImage.color.SetA(0.5f);
		}
		else {
			itemFilterImage.sprite = null;
			itemFilterImage.color = itemFilterImage.color.SetA(0.0f);
		}

		if (item != null && item.itemSO != null) {
			if (item.itemSO.maxCount == 1) {
				count.text = "";
			}
			else {
				count.text = item.count.ToString();
			}

			itemImage.sprite = item.itemSO.sprite;
			LeanTweenEx.ChangeAlpha(itemImage, 1.0f, 0.05f).setEase(LeanTweenType.easeInOutQuad);

			popup.SetText(item.GetInfoForPopup());

		}
		else {
			count.text = "";

			popup.SetText("");
			
			itemImage.sprite = null;
			itemImage.color = itemImage.color.SetA(0.0f);
		}
	}
}
