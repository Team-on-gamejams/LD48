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
	[Space]
	[SerializeField] Rect startRoom;
	[SerializeField] Rect startOreGold;
	[SerializeField] Rect startOreIron;
	[SerializeField] Vector2Int playerStartPos;

	Transform visualParent;

#if UNITY_EDITOR
	private void OnValidate() {

	}
#endif

	void Start() {
		visualParent = new GameObject("GridVisual").transform;
		visualParent.parent = transform;
		visualParent.localPosition = Vector3.zero;
		visualParent.eulerAngles = Vector3.zero;

		for (int x = 0; x < gridSize.x; ++x) {
			for (int y = 0; y < gridSize.y; ++y) {
				GameObject go = new GameObject($"Cell-{x}-{y}");
				go.transform.parent = visualParent;

				Cell c = go.AddComponent<Cell>();

				if(x <= Random.Range(0, 2) || y <= Random.Range(0, 2) || x >= gridSize.x - Random.Range(1, 3) || y >= gridSize.y -Random.Range(1, 3)) {
					c.foregroud = Cell.CellContentForegroud.Bedrock;
					c.background = Cell.CellContentBackground.Bedrock;
					c.ore = Cell.CellContentOre.None;
				}
				else if(y + Random.Range(-2, 2) < gridSize.y / 2) {
					c.foregroud = Cell.CellContentForegroud.Stone;
					c.background = Cell.CellContentBackground.Stone;
					c.ore = Cell.CellContentOre.None;
				}
				else {
					c.foregroud = Cell.CellContentForegroud.Dirt;
					c.background = Cell.CellContentBackground.Dirt;
					c.ore = Cell.CellContentOre.None;
				}

				if (c.background == Cell.CellContentBackground.Stone && RandomEx.GetEventWithChance(1)) {
					c.ore = Cell.CellContentOre.OreGold;
				}
				else if ((c.background == Cell.CellContentBackground.Stone || c.background == Cell.CellContentBackground.Dirt) && RandomEx.GetEventWithChance(1)) {
					c.ore = Cell.CellContentOre.OreIron;
				}

				if (startRoom.Contains(new Vector2(x, y))) {
					c.foregroud = Cell.CellContentForegroud.None;
					c.ore = Cell.CellContentOre.None;
				}

				if (startOreGold.Contains(new Vector2(x, y))) {
					c.ore = Cell.CellContentOre.OreGold;
				}
				else if (startOreIron.Contains(new Vector2(x, y))) {
					c.ore = Cell.CellContentOre.OreIron;
				}

				c.MyGrid = this;
				c.Coord = new Vector2Int(x, y);

				c.Init();

				if (x == playerStartPos.x && y == playerStartPos.y) {
					GameManager.Instance.player.mover.transform.position = c.transform.position;
				}
			}
		}
	}

	private void OnDestroy() {
		Destroy(visualParent.gameObject);
	}
}
