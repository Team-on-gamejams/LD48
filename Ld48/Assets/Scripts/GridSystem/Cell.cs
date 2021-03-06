using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Cell : MonoBehaviour {
	public enum CellBlockType { Foreground, Background, Ore }
	public enum CellContentForegroud { None, Dirt, Stone, Bedrock, Brick, Ladder, Platform, IronOreBlock, GoldOreBlock, IronIgnotBlock, GoldIgnotBlock, Chest, Assembler, Furnace }
	public enum CellContentBackground { None, Dirt, Stone, Bedrock }
	public enum CellContentOre { None, OreIron, OreGold }

	const string debugTextName = "DebugText";

	public Grid MyGrid { get; set; }
	public Vector2Int Coord {
		get {
			return coord;
		}
		set {
			coord = value;

		}
	}
	Vector2Int coord;

	[NonSerialized] public PlacebleBlock foregroundBlock;
	[NonSerialized] public PlacebleBlock backgroundBlock;
	[NonSerialized] public PlacebleBlock oreBlock;

	[Header("Data"), Space]
	public CellContentForegroud foregroud;
	public CellContentBackground background;
	public CellContentOre ore;

	[Header("Refs"), Space]
	[SerializeField] TextMeshPro debugText;
	[SerializeField] SpriteRenderer hightlightSr;
	[SerializeField] BlockDestoyVisual destoyVisual;

	float currMineTime = 0.0f;

	public void Init() {
		CreateAllVisuals();
		CreateDebugText();

		transform.localScale = new Vector3(MyGrid.cellSize.x, MyGrid.cellSize.y, 1.0f);
		transform.position = new Vector3(MyGrid.cellSize.x * coord.x, MyGrid.cellSize.y * coord.y, 0.0f)/* - new Vector3(MyGrid.gridSize.x / 2 * MyGrid.cellSize.x, MyGrid.gridSize.y / 2 * MyGrid.cellSize.y, 0.0f)*/;
	}

	public void RecreateVisualAfterChangeType() {
		currMineTime = 0;
		destoyVisual.UpdateVisual(0);
		CreateAllVisuals();
	}

	public void Hightlight() {
		LeanTween.cancel(hightlightSr.gameObject, false);
		LeanTweenEx.ChangeAlpha(hightlightSr, 1.0f, 0.1f).setEase(LeanTweenType.easeInOutQuad);
	}

	public void UnHightlight() {
		LeanTween.cancel(hightlightSr.gameObject, false);
		LeanTweenEx.ChangeAlpha(hightlightSr, 0.0f, 0.1f).setEase(LeanTweenType.easeInOutQuad);
	}

	public bool IsPlayerInside() {
		if (GameManager.Instance.player.mover.collider.OverlapPoint(transform.position))
			return true;

		int count = 0;

		if (GameManager.Instance.player.mover.collider.OverlapPoint(transform.position + new Vector3(MyGrid.cellSize.x / 2, MyGrid.cellSize.y / 2)))
			++count;
		if (GameManager.Instance.player.mover.collider.OverlapPoint(transform.position + new Vector3(-MyGrid.cellSize.x / 2, -MyGrid.cellSize.y / 2)))
			++count;
		if (count >= 2)
			return true;

		if (GameManager.Instance.player.mover.collider.OverlapPoint(transform.position + new Vector3(-MyGrid.cellSize.x / 2, MyGrid.cellSize.y / 2)))
			++count;
		if (count >= 2)
			return true;

		if (GameManager.Instance.player.mover.collider.OverlapPoint(transform.position + new Vector3(MyGrid.cellSize.x / 2, -MyGrid.cellSize.y / 2)))
			++count;
		return count >= 2;
	}

	public void Mine(float deltaTime, float mineToolForce, bool isNeedLoot = true) {
		if (foregroundBlock) {
			currMineTime += deltaTime * (mineToolForce - foregroundBlock.neededForceToBroke + 1);
			float brokePersent = currMineTime / foregroundBlock.neededTimeToBroke;

			if (brokePersent >= 1) {
				currMineTime = 0;
				destoyVisual.UpdateVisual(0);

				if (isNeedLoot) {
					ItemOnGround.CreateOnGround(
						foregroundBlock.itemToDrop.item.CloneItem().SetCount(1),
						transform.position + new Vector3(Random.Range(-MyGrid.cellSize.x / 2 * 0.8f, MyGrid.cellSize.x / 2 * 0.8f), Random.Range(-MyGrid.cellSize.y / 2 * 0.8f, MyGrid.cellSize.y / 2 * 0.8f))
					);
				}

				if(foregroundBlock.craftPlace)
					foregroundBlock.craftPlace.OnMined();
				foregroud = CellContentForegroud.None;
				RecreateVisualAfterChangeType();
			}
			else {
				destoyVisual.UpdateVisual(brokePersent);
			}

		}
		else if (oreBlock && isNeedLoot) {
			currMineTime += deltaTime * (mineToolForce - oreBlock.neededForceToBroke + 1);
			float brokePersent = currMineTime / oreBlock.neededTimeToBroke;

			if (brokePersent >= 1) {
				currMineTime -= oreBlock.neededTimeToBroke;
				destoyVisual.UpdateVisual(0);

				ItemOnGround.CreateOnGround(
					oreBlock.itemToDrop.item.CloneItem().SetCount(1), 
					transform.position + new Vector3(Random.Range(-MyGrid.cellSize.x / 2 * 0.8f, MyGrid.cellSize.x / 2 * 0.8f), Random.Range(-MyGrid.cellSize.y / 2 * 0.8f, MyGrid.cellSize.y / 2 * 0.8f))
				);
			}
			else {
				destoyVisual.UpdateVisual(brokePersent);
			}
		}

		FillDebugText();
	}

	void CreateAllVisuals() {
		gameObject.transform.localScale = new Vector3(1, 1, 1.0f);

		if (foregroundBlock != null && foregroud != foregroundBlock.foregroud) {
			Destroy(foregroundBlock.gameObject);
			foregroundBlock = null;
		}

		if (backgroundBlock != null && background != backgroundBlock.background) {
			Destroy(backgroundBlock.gameObject);
			backgroundBlock = null;
		}

		if (oreBlock != null && ore != oreBlock.ore) {
			Destroy(oreBlock.gameObject);
			oreBlock = null;
		}

		if (foregroundBlock == null && foregroud != CellContentForegroud.None) {
			foregroundBlock = Instantiate(GameManager.Instance.GetCellForeground(foregroud), gameObject.transform.position, Quaternion.identity, gameObject.transform).GetComponent<PlacebleBlock>();
			foregroundBlock.MyCell = this;
		}

		if (backgroundBlock == null && background != CellContentBackground.None) {
			backgroundBlock = Instantiate(GameManager.Instance.GetCellBackground(background), gameObject.transform.position, Quaternion.identity, gameObject.transform).GetComponent<PlacebleBlock>();
			backgroundBlock.MyCell = this;
		}

		if (oreBlock == null && ore != CellContentOre.None) {
			oreBlock = Instantiate(GameManager.Instance.GetCellOre(ore), gameObject.transform.position, Quaternion.identity, gameObject.transform).GetComponent<PlacebleBlock>();
			oreBlock.MyCell = this;
		}
	}

	void CreateDebugText() {
		debugText.transform.SetParent(gameObject.transform, true);
		debugText.transform.localPosition = Vector3.zero;

		debugText.alignment = TextAlignmentOptions.Center;
		debugText.color = Color.black;
		debugText.sortingLayerID = UnityConstants.SortingLayers.Foreground;
		debugText.sortingOrder = -1;
		debugText.enableAutoSizing = true;
		debugText.fontSizeMax = 3.0f;
		debugText.fontSizeMin = 0.0f;
		debugText.enableWordWrapping = false;

		RectTransform textFieldrt = debugText.GetComponent<RectTransform>();
		textFieldrt.sizeDelta = MyGrid.cellSize;

		FillDebugText();
	}

	void FillDebugText() {
		debugText.text = $"{coord.x} {coord.y}";
		
		if (currMineTime != 0) {
			if (foregroundBlock)
				debugText.text = $"{coord.x} {coord.y}\n{currMineTime.ToString("0.00")} {foregroundBlock.neededTimeToBroke.ToString("0.00")}";
			else if (oreBlock)
				debugText.text = $"{coord.x} {coord.y}\n{currMineTime.ToString("0.00")} {oreBlock.neededTimeToBroke.ToString("0.00")}";
		}
	}
}
