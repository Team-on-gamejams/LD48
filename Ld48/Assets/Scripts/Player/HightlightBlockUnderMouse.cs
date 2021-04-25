using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class HightlightBlockUnderMouse : MonoBehaviour {
	Cell lastHightlightedCell;

	void Update() {
		Cell cell = GameManager.Instance.GetCellAtMousePosWithInteractClamp();

		if(cell != lastHightlightedCell) {
			if(lastHightlightedCell)
				lastHightlightedCell.UnHightlight();
			if(cell)
				cell.Hightlight();
			lastHightlightedCell = cell;
		}
	}
}
