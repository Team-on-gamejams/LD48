using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "ItemSO", menuName = "Item")]
public class ItemSO : ScriptableObject {
	public enum ItemType : int { 
		None = 0,

		Pickaxe = 1,

		Dirt = 500,
		Stone,

		OreIron = 1000,
		OreGold,

		IgnotIron = 1500,
		IgnotGold,

		BlockBrick = 2000,
		BlockPlatform,
		BlockLadder,
	}

	public ItemType type;
	[Space]
	public string name = "Item name";
	public string description = "Item desc";
	public Sprite sprite;
	[Space]
	public int maxCount = 10;
}