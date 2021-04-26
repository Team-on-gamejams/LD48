using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftSO", menuName = "Craft")]
public class CraftSO : ScriptableObject {
	public enum CraftPlaceType : int {
		Player = 1,
		Furnace = 2,
		Assembler = 4
	}

	[Header("Craft data"), Space]
	[EnumFlag] public CraftPlaceType place;
	public float craftTime;
	public ItemData[] ingradients;
	public ItemData[] results;
}
