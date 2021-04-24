using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Grid))]
public class GridVisual : MonoBehaviour {
	[Header("Tiles"), Space]
	[SerializeField] Sprite sprite;

	[Header("Refs"), Space]
	[SerializeField] Grid grid;

	Transform visualParent;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!grid)
			grid = GetComponent<Grid>();
	}
#endif

	private void Awake() {
		visualParent = new GameObject("GridVisual").transform;
		visualParent.parent = transform;
		visualParent.localPosition = Vector3.zero;
		visualParent.eulerAngles = Vector3.zero;

		for(int x = 0;  x < grid.gridSize.x; ++x) {
			for (int y = 0; y < grid.gridSize.y; ++y) {
				GameObject go = new GameObject($"Cell-{x}-{y}");
				go.transform.localScale = new Vector3(grid.cellSize.x, grid.cellSize.y, 1.0f);
				go.transform.parent = visualParent;
				go.transform.position = new Vector3(grid.cellSize.x * x, grid.cellSize.y * y, 0.0f) - new Vector3(grid.gridSize.x / 2 * grid.cellSize.x, grid.gridSize.y / 2 * grid.cellSize.y, 0.0f);

				SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
				sr.sprite = sprite;
				sr.sortingLayerID = UnityConstants.SortingLayers.Background;
				sr.sortingOrder = -100;

				GameObject coordgo = new GameObject($"Coord-{x}-{y}");
				coordgo.transform.parent = go.transform;
				coordgo.transform.localPosition = Vector3.zero;

				TextMeshPro textField = coordgo.AddComponent<TextMeshPro>();
				textField.text = $"({x} {y})";
				textField.alignment = TextAlignmentOptions.Center;
				textField.color = Color.black;
				textField.sortingLayerID = UnityConstants.SortingLayers.Background;
				textField.sortingOrder = -99;
				textField.enableAutoSizing = true;
				textField.fontSizeMax = 3.0f;
				textField.fontSizeMin = 0.0f;
				textField.enableWordWrapping = false;

				RectTransform textFieldrt = textField.GetComponent<RectTransform>();
				textFieldrt.sizeDelta = grid.cellSize;
			}
		}
	}

	private void OnDestroy() {
		Destroy(visualParent.gameObject);
	}
}
