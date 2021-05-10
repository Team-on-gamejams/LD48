using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class PlacebleBlock : MonoBehaviour {
	public Cell MyCell { 
		get => myCell;
		set{
			myCell = value;
			if (craftPlace) {
				craftPlace.MyCell = myCell;
			}
		}
	}
	Cell myCell;


	[Header("Drop data"), Space]
	public ItemOnGround itemToDrop;

	[Header("Break data"), Space]
	public int neededForceToBroke = 1;
	public float neededTimeToBroke = 1.0f;

	[Header("Place data"), Space]
	public Cell.CellBlockType type;
	[NaughtyAttributes.ShowIf("IsForeground")] public Cell.CellContentForegroud foregroud;
	[NaughtyAttributes.ShowIf("IsForeground")] public bool isCanPlaceOnPlayerPos = false;
	[NaughtyAttributes.ShowIf("IsBackground")] public Cell.CellContentBackground background;
	[NaughtyAttributes.ShowIf("IsOre")] public Cell.CellContentOre ore;

	[Header("Refs"), Space]
	public ForegroundPlaceBase craftPlace;

#if UNITY_EDITOR
	private void OnValidate() {
		if(!craftPlace)
			craftPlace = GetComponent<ForegroundPlaceBase>();
	}
#endif

	public bool IsForeground() {
		return type == Cell.CellBlockType.Foreground;
	}

	public bool IsBackground() {
		return type == Cell.CellBlockType.Background;
	}

	public bool IsOre() {
		return type == Cell.CellBlockType.Ore;
	}
}
