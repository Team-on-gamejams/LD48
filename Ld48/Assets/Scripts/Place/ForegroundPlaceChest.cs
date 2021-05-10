using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundPlaceChest : ForegroundPlaceBase {
	[Header("Chest data"), Space]
	[SerializeField] int chestSize = 10;
	
	private void Start() {
		foreach (var inventory in inventories) {
			inventory.InitInvetory(chestSize);
		}
	}
}
