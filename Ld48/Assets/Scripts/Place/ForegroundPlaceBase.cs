using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundPlaceBase : MonoBehaviour {
	static int canvasSortOrder = -1000;

	public Cell MyCell { get; set; }

	[Header("Refs"), Space]
	[SerializeField] Inventory[] inventories;

	[Header("UI"), Space]
	[SerializeField] Canvas canvas;
	[SerializeField] CanvasGroup cg;

	bool isShowed;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!canvas)
			canvas = GetComponent<Canvas>();
		if (!cg)
			cg = GetComponent<CanvasGroup>();
		if (inventories == null || inventories.Length != GetComponentsInChildren<Inventory>().Length) {
			inventories = GetComponentsInChildren<Inventory>();
		}
	}
#endif

	private void Awake() {
		isShowed = false;
		cg.interactable = cg.blocksRaycasts = false;
		cg.alpha = 0.0f;
	}

	virtual protected void Update() {
		if (isShowed) {
			if ((transform.position - GameManager.Instance.player.mover.transform.position).sqrMagnitude > GameManager.Instance.player.maxInteractDistanceSqr) {
				HideUI();
			}
		}
	}

	public void ToggleUI() {
		if (isShowed) {
			HideUI();
		}
		else {
			ShowUI();
		}
	}

	public virtual void ShowUI() {
		isShowed = true;
		
		canvas.sortingOrder = canvasSortOrder++;
		cg.interactable = cg.blocksRaycasts = true;

		LeanTween.cancel(cg.gameObject, false);
		LeanTweenEx.ChangeAlpha(cg, 1.0f, 0.2f).setEase(LeanTweenType.easeInOutQuad);
	}

	public virtual void HideUI() {
		isShowed = false;
		
		--canvasSortOrder;
		cg.interactable = cg.blocksRaycasts = false;

		LeanTween.cancel(cg.gameObject, false);
		LeanTweenEx.ChangeAlpha(cg, 0.0f, 0.2f).setEase(LeanTweenType.easeInOutQuad);
	}

	public void OnMined() {
		if (inventories != null) {
			foreach (var inventory in inventories) {
				inventory.DropAllItemsToGround(
					transform.position, 
					new Vector2(-MyCell.MyGrid.cellSize.x / 2, MyCell.MyGrid.cellSize.x / 2) * 0.8f,
					new Vector2(-MyCell.MyGrid.cellSize.y / 2, MyCell.MyGrid.cellSize.y / 2) * 0.8f
				);
			}
		}
	}
}
