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
	public enum CellContentForegroud { None, Dirt, Stone, Bedrock, Brick, Ladder, Platform }
	public enum CellContentBackground { None, Dirt, Stone, Bedrock }
	public enum CellContentOre { None, OreIron, OreGold }

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
	[SerializeField] TextMeshPro debugText;

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
	}

	void CreateAllVisuals() {
		gameObject.transform.localScale = new Vector3(1, 1, 1.0f);

		if(foregroud != CellContentForegroud.None)
			Instantiate(GameManager.Instance.GetCellForeground(foregroud), Vector3.zero, Quaternion.identity, gameObject.transform);

		if (background != CellContentBackground.None)
			Instantiate(GameManager.Instance.GetCellBackground(background), Vector3.zero, Quaternion.identity, gameObject.transform);

		if (ore != CellContentOre.None)
			Instantiate(GameManager.Instance.GetCellOre(ore), Vector3.zero, Quaternion.identity, gameObject.transform);
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
}
