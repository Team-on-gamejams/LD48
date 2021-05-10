using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "CraftSO", menuName = "Craft")]
public class CraftSO : ScriptableObject {
	public enum CraftPlaceType : int {
		None = 0,
		Player = 1,
		Furnace = 2,
		Assembler = 4
	}

	[Header("Craft data"), Space]
	[EnumFlags] public CraftPlaceType place;
	public float craftTime;
	public ItemData[] ingradients;
	public ItemData[] results;

	[Header("Visual"), Space]
	public Sprite resultSpriteOverride;

	public string GetCraftPlaceTypeString() {
		string result = "";

		foreach (CraftPlaceType value in CraftPlaceType.GetValues(place.GetType())) {
			if (place.HasFlag(value)) {
				if(result.Length != 0) {
					result += ", ";
				}

				switch (value) {
					case CraftPlaceType.Player:
						result += "Player";
						break;

					case CraftPlaceType.Furnace:
						result += "Furnace";
						break;

					case CraftPlaceType.Assembler:
						result += "Assembler";
						break;

					case CraftPlaceType.None:
						if(place == CraftPlaceType.None) {
							Debug.LogError("Unknown CraftPlaceType");
							result += "null";
						}
						break;
					default:
						Debug.LogError("Unknown CraftPlaceType");
						result += "null";
						break;
				}
			}
		}

		

		return result;
	}
}
