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
	public enum CellContentForegroud { None, Dirt, Stone, Bedrock }
	public enum CellContentBackground { None, Dirt, Stone, Bedrock }
	public enum CellContentOre { None, OreIron, OreGold }

	const string foregroundGoName = "CellForeground";
	const string backgroundGoName = "CellBackground";
	const string oreGoName = "CellOre";
	const string debugTextName = "DebugText";

	Vector2 cellSize = Vector2.one;
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

	[Header("Data"), Space]
	public CellContentForegroud foregroud;
	public CellContentBackground background;
	public CellContentOre ore;

	[Header("Refs"), Space]
	[SerializeField] SpriteRenderer srBackdround;
	[SerializeField] SpriteRenderer srForeground;
	[SerializeField] SpriteRenderer srOre;
	[SerializeField] TextMeshPro debugText;
	[SerializeField] BoxCollider2D collider;

//#if UNITY_EDITOR
//	void OnValidate() {
//		CreateCellVisuals();
//		UnityEditor.EditorUtility.SetDirty(gameObject);
//	}
//#endif

	public void Init() {
		CreateCell();

		debugText.text = $"{coord.x} {coord.y}";

		transform.localScale = new Vector3(cellSize.x, cellSize.y, 1.0f);
		transform.position = new Vector3(cellSize.x * coord.x, cellSize.y * coord.y, 0.0f) - new Vector3(MyGrid.gridSize.x / 2 * cellSize.x, MyGrid.gridSize.y / 2 * cellSize.y, 0.0f);
	}

	void CreateCell() {
		CreateAllVisuals();
		CreateDebugText();
		CreateCollider();
	}

	void CreateAllVisuals() {
		gameObject.transform.localScale = new Vector3(1, 1, 1.0f);

		switch (foregroud) {
			case CellContentForegroud.None:
				CreateCellSingleVisual(foregroundGoName, null, UnityConstants.SortingLayers.BlockForeground, 0);
				break;
			case CellContentForegroud.Dirt:
				CreateCellSingleVisual(foregroundGoName, GameManager.Instance.foregroundDirtSprite, UnityConstants.SortingLayers.BlockForeground, 0);
				break;
			case CellContentForegroud.Stone:
				CreateCellSingleVisual(foregroundGoName, GameManager.Instance.foregroundStoneSprite, UnityConstants.SortingLayers.BlockForeground, 0);
				break;
			case CellContentForegroud.Bedrock:
				CreateCellSingleVisual(foregroundGoName, GameManager.Instance.foregroundBedrockSprite, UnityConstants.SortingLayers.BlockForeground, 0);
				break;
			default:
				Debug.LogError("Unknow foregroud type");
				break;
		}

		switch (background) {
			case CellContentBackground.None:
				CreateCellSingleVisual(backgroundGoName, null, UnityConstants.SortingLayers.BlockBackground, 0);
				break;
			case CellContentBackground.Dirt:
				CreateCellSingleVisual(backgroundGoName, GameManager.Instance.backgroundDirtSprite, UnityConstants.SortingLayers.BlockBackground, 0);
				break;
			case CellContentBackground.Stone:
				CreateCellSingleVisual(backgroundGoName, GameManager.Instance.backgroundStoneSprite, UnityConstants.SortingLayers.BlockBackground, 0);
				break;
			case CellContentBackground.Bedrock:
				CreateCellSingleVisual(backgroundGoName, GameManager.Instance.backgroundBedrockSprite, UnityConstants.SortingLayers.BlockBackground, 0);
				break;
			default:
				Debug.LogError("Unknow background type");
				break;
		}

		switch (ore) {
			case CellContentOre.None:
				CreateCellSingleVisual(oreGoName, null, UnityConstants.SortingLayers.BlockOre, 0);
				break;
			case CellContentOre.OreIron:
				CreateCellSingleVisual(oreGoName, GameManager.Instance.oreIronSprite, UnityConstants.SortingLayers.BlockOre, 0);
				break;
			case CellContentOre.OreGold:
				CreateCellSingleVisual(oreGoName, GameManager.Instance.oreGoldSprite, UnityConstants.SortingLayers.BlockOre, 0);
				break;
			default:
				Debug.LogError("Unknow ore type");
				break;
		}

		srForeground = transform.Find(foregroundGoName).GetComponent<SpriteRenderer>();
		srBackdround = transform.Find(backgroundGoName).GetComponent<SpriteRenderer>();
		srOre = transform.Find(oreGoName).GetComponent<SpriteRenderer>();
	}

	void CreateCellSingleVisual(string goName, Sprite sprite, int sortingLayerId, int sortingOrder) {
		Transform cellTrans = transform.Find(goName);
		GameObject cellGo;
		if (cellTrans)
			cellGo = cellTrans.gameObject;
		else
			cellGo = new GameObject(goName);
		cellGo.transform.parent = gameObject.transform;
		cellGo.transform.localPosition = Vector3.zero;

		SpriteRenderer sr = cellGo.GetComponent<SpriteRenderer>();
		if (!sr)
			sr = cellGo.AddComponent<SpriteRenderer>();

		sr.sprite = sprite;
		sr.sortingLayerID = sortingLayerId;
		sr.sortingOrder = sortingOrder;
	}

	void CreateDebugText() {
		Transform debugTransform = transform.Find(debugTextName);
		GameObject debugGO;
		if (debugTransform)
			debugGO = debugTransform.gameObject;
		else
			debugGO = new GameObject(debugTextName);
		debugGO.transform.SetParent(gameObject.transform, true);
		debugGO.transform.localPosition = Vector3.zero;

		debugText = debugGO.GetComponent<TextMeshPro>();
		if (!debugText)
			debugText = debugGO.AddComponent<TextMeshPro>();
		debugText.text = $"({0} {0})";
		debugText.alignment = TextAlignmentOptions.Center;
		debugText.color = Color.black;
		debugText.sortingLayerID = UnityConstants.SortingLayers.Foreground;
		debugText.sortingOrder = -1;
		debugText.enableAutoSizing = true;
		debugText.fontSizeMax = 3.0f;
		debugText.fontSizeMin = 0.0f;
		debugText.enableWordWrapping = false;

		DebugText debug = debugGO.GetComponent<DebugText>();
		if (!debug)
			debug = debugGO.AddComponent<DebugText>();

		RectTransform textFieldrt = debugGO.GetComponent<RectTransform>();
		if (!textFieldrt)
			textFieldrt = debugGO.AddComponent<RectTransform>();
		textFieldrt.sizeDelta = cellSize;
	}

	void CreateCollider() {
		collider = gameObject.GetComponent<BoxCollider2D>();
		if (!collider)
			collider = gameObject.AddComponent<BoxCollider2D>();

		collider.size = cellSize;

		collider.enabled = foregroud != CellContentForegroud.None;
	}
}
