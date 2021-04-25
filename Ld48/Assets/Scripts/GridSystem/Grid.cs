using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour {

	[Header("Data"), Space]
    public Vector2Int gridSize = new Vector2Int(128, 128);
	public Vector2 cellSize = Vector2.one;

	[Header("Positions"), Space]
	[SerializeField] Rect startRoom;
	[SerializeField] Rect startOreGold;
	[SerializeField] Rect startOreIron;
	[SerializeField] Vector2Int playerStartPos;

	[Header("Refs"), Space]
	[SerializeField] GameObject cellPrefab;

	Transform visualParent;

	List<Cell> cells;

#if UNITY_EDITOR
	private void OnValidate() {

	}
#endif

	private void Awake() {
		cells = new List<Cell>(gridSize.x * gridSize.y);
		GameManager.Instance.grid = this;
	}

	void Start() {
		visualParent = new GameObject("GridVisual").transform;
		visualParent.parent = transform;
		visualParent.localPosition = Vector3.zero;
		visualParent.eulerAngles = Vector3.zero;

		for (int x = 0; x < gridSize.x; ++x) {
			for (int y = 0; y < gridSize.y; ++y) {
				GameObject go = Instantiate(cellPrefab, visualParent);
				go.name = $"Cell-{x}-{y}";

				Cell cell = go.GetComponent<Cell>();
				cell.MyGrid = this;
				cell.Coord = new Vector2Int(x, y);

				if (x <= Random.Range(0, 2) || y <= Random.Range(0, 2) || x >= gridSize.x - Random.Range(1, 3) || y >= gridSize.y -Random.Range(1, 3)) {
					cell.foregroud = Cell.CellContentForegroud.Bedrock;
					cell.background = Cell.CellContentBackground.Bedrock;
					cell.ore = Cell.CellContentOre.None;
				}
				else if(y + Random.Range(-2, 2) < gridSize.y / 2) {
					cell.foregroud = Cell.CellContentForegroud.Stone;
					cell.background = Cell.CellContentBackground.Stone;
					cell.ore = Cell.CellContentOre.None;
				}
				else {
					cell.foregroud = Cell.CellContentForegroud.Dirt;
					cell.background = Cell.CellContentBackground.Dirt;
					cell.ore = Cell.CellContentOre.None;
				}

				if (cell.background == Cell.CellContentBackground.Stone && RandomEx.GetEventWithChance(1)) {
					cell.ore = Cell.CellContentOre.OreGold;
				}
				else if ((cell.background == Cell.CellContentBackground.Stone || cell.background == Cell.CellContentBackground.Dirt) && RandomEx.GetEventWithChance(1)) {
					cell.ore = Cell.CellContentOre.OreIron;
				}

				if (startRoom.Contains(new Vector2(x, y))) {
					cell.foregroud = Cell.CellContentForegroud.None;
					cell.ore = Cell.CellContentOre.None;
				}

				if (startOreGold.Contains(new Vector2(x, y))) {
					cell.ore = Cell.CellContentOre.OreGold;
				}
				else if (startOreIron.Contains(new Vector2(x, y))) {
					cell.ore = Cell.CellContentOre.OreIron;
				}

				cell.Init();
				cells.Add(cell);

				if (x == playerStartPos.x && y == playerStartPos.y) {
					GameManager.Instance.player.mover.transform.position = cell.transform.position;
				}
			}
		}
	}

	private void OnDestroy() {
		Destroy(visualParent.gameObject);
	}

	public Cell GetCellWorldPos(Vector3 pos) {
		Vector2Int cellCoord = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

		foreach (var cell in cells) {
			if (cell.Coord == cellCoord)
				return cell;
		}

		return null;
	}
}
