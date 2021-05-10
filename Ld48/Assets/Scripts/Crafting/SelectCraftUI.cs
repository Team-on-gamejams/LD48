using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCraftUI : MonoBehaviour {
	[Header("UI"), Space]
	[SerializeField] bool isHideUnavaliablePlaceCrafts = true;

	[Header("UI refs"), Space]
	[SerializeField] CanvasGroup cgSelectCraft;
	[SerializeField] CanvasGroup cgCraftingPlace;
	[SerializeField] Image fillImage;

	[Header("Refs"), Space]
	[SerializeField] PlacebleBlock myBlock;

	[Header("Refs prefabs"), Space]
	[SerializeField] GameObject recipeGoPrefab;

	Inventory inventoryToAddItem;
	Inventory inventoryToRemoveItem;
	CraftingPlace craftingPlace;

	List<CraftingItemUI> craftingItems;

	CraftSO selectedCraft;
	CraftSO previousCraft;
	bool isCrafting = false;

#if UNITY_EDITOR
	private void OnValidate() {

	}
#endif

	public void InitUI(CraftingPlace _craftingPlace, Inventory _inventoryToAddItem, Inventory _inventoryToRemoveItem, CraftSO.CraftPlaceType craftPlaceType) {
		craftingPlace = _craftingPlace;
		inventoryToAddItem = _inventoryToAddItem;
		inventoryToRemoveItem = _inventoryToRemoveItem;

		craftingItems = new List<CraftingItemUI>(GameManager.Instance.crafts.Length);
		foreach (var craft in GameManager.Instance.crafts) {
			if (isHideUnavaliablePlaceCrafts && !craft.place.HasFlag(craftPlaceType))
				continue;

			CraftingItemUI craftingItemUI = Instantiate(recipeGoPrefab, transform).GetComponent<CraftingItemUI>();
			craftingItemUI.Init(craft, craftPlaceType, OnClickOnItem);
			craftingItems.Add(craftingItemUI);
		}

		cgSelectCraft.interactable = cgSelectCraft.blocksRaycasts = true;
		cgSelectCraft.alpha = 1.0f;

		cgCraftingPlace.interactable = cgCraftingPlace.blocksRaycasts = false;
		cgCraftingPlace.alpha = 0.0f;

		fillImage.fillAmount = 0;

		inventoryToRemoveItem.onInventoryChangeEvent += OnRemoveInventoryChange;
		craftingPlace.onUpdateCraftTimeFill += OnUpdateCraftTimeFill;
		craftingPlace.onEndCraft += OnEndCraft;
	}

	void OnDestroy() {
		inventoryToRemoveItem.onInventoryChangeEvent -= OnRemoveInventoryChange;
		craftingPlace.onUpdateCraftTimeFill -= OnUpdateCraftTimeFill;
		craftingPlace.onEndCraft -= OnEndCraft;
	}

	public void SelectOtherCraft() {
		previousCraft = selectedCraft;
		selectedCraft = null;

		if (isCrafting) {
			foreach (var ingradient in previousCraft.ingradients) {
				ItemData leftItem = GameManager.Instance.player.inventory.AddItem(ingradient.CloneItem());

				if (leftItem.count != 0) {
					//TODO: Inventory full popup text
					Vector3 pos = GameManager.Instance.player.transform.position;
					ItemOnGround.CreateOnGround(leftItem, pos);
					leftItem.itemSO = null;
				}
			}

			craftingPlace.AbortCraft();
		}

		inventoryToAddItem.GiveAllItemsToPlayerOrDrop(
			myBlock.MyCell.transform.position,
			new Vector2(-myBlock.MyCell.MyGrid.cellSize.x / 2, myBlock.MyCell.MyGrid.cellSize.x / 2) * 0.8f,
			new Vector2(-myBlock.MyCell.MyGrid.cellSize.y / 2, myBlock.MyCell.MyGrid.cellSize.y / 2) * 0.8f
		);
		inventoryToRemoveItem.GiveAllItemsToPlayerOrDrop(
			myBlock.MyCell.transform.position,
			new Vector2(-myBlock.MyCell.MyGrid.cellSize.x / 2, myBlock.MyCell.MyGrid.cellSize.x / 2) * 0.8f,
			new Vector2(-myBlock.MyCell.MyGrid.cellSize.y / 2, myBlock.MyCell.MyGrid.cellSize.y / 2) * 0.8f
		);


		cgCraftingPlace.interactable = cgCraftingPlace.blocksRaycasts = false;
		LeanTween.cancel(cgCraftingPlace.gameObject, false);
		LeanTweenEx.ChangeAlpha(cgCraftingPlace, 0.0f, 0.2f).setEase(LeanTweenType.easeInOutQuad);

		cgSelectCraft.interactable = cgSelectCraft.blocksRaycasts = true;
		LeanTween.cancel(cgSelectCraft.gameObject, false);
		LeanTweenEx.ChangeAlpha(cgSelectCraft, 1.0f, 0.2f).setEase(LeanTweenType.easeInOutQuad);
	}

	void OnRemoveInventoryChange() {
		TryCraft();
	}

	void OnUpdateCraftTimeFill(float fill) {
		fillImage.fillAmount = fill;
	}

	void OnEndCraft() {
		fillImage.fillAmount = 0;
		isCrafting = false;

		TryCraft();
	}

	void OnClickOnItem(CraftSO craft) {
		selectedCraft = craft;

		inventoryToAddItem.InitInvetory(selectedCraft.results.Length);
		inventoryToRemoveItem.InitInvetory(selectedCraft.ingradients.Length);

		for(int i = 0; i < selectedCraft.results.Length; ++i) {
			inventoryToAddItem.SetFilter(i, selectedCraft.results[i].itemSO);
		}

		for (int i = 0; i < selectedCraft.ingradients.Length; ++i) {
			inventoryToRemoveItem.SetFilter(i, selectedCraft.ingradients[i].itemSO);
		}

		cgSelectCraft.interactable = cgSelectCraft.blocksRaycasts = false;
		LeanTween.cancel(cgSelectCraft.gameObject, false);
		LeanTweenEx.ChangeAlpha(cgSelectCraft, 0.0f, 0.2f).setEase(LeanTweenType.easeInOutQuad);

		cgCraftingPlace.interactable = cgCraftingPlace.blocksRaycasts = true;
		LeanTween.cancel(cgCraftingPlace.gameObject, false);
		LeanTweenEx.ChangeAlpha(cgCraftingPlace, 1.0f, 0.2f).setEase(LeanTweenType.easeInOutQuad);

		craftingPlace.ResetCraftTime();
		TryCraft();

		previousCraft = null;
	}

	void TryCraft() {
		if (!isCrafting && selectedCraft) {
			if (inventoryToRemoveItem.CheckIsEnoughIngradients(selectedCraft)) {
				isCrafting = true;

				foreach (var ingradient in selectedCraft.ingradients)
					inventoryToRemoveItem.RemoveItem(ingradient.CloneItem());

				craftingPlace.Craft(selectedCraft);
			}
			else {
				craftingPlace.ResetCraftTime();
			}
		}
	}
}
