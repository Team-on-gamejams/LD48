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
	public enum ItemMetaType : int { 
		BuildableForeground,
		MiningTool
	}
	public enum ItemType : int { 
		None = 0,

		//Tools
		ShovelaxeStone = 1,		
		ShovelaxeIron,
		ShovelaxeDiamond,

		//Tiles (use in world gen)
		Dirt = 500,
		Stone,
		Bedrock,

		//Ores (use in world gen).
		//Used for both ores and blocks
		OreIron = 1000,
		OreGold,

		//Ignot from ores
		IgnotIron = 1500,
		IgnotGold,

		//Hand-made blocks
		BlockBrick = 2000,
		BlockPlatform,
		BlockLadder,

		//Places for crafting
		PlaceChest = 3000,
		PlaceFurnace,
		PlaceAssembler,
	}

	[Header("Item data"), Space]
	public ItemMetaType metaType;
	public ItemType type;
	public int maxCount = 10;

	[Header("Visual"), Space]
	public string name = "Item name";
	[Multiline] public string description = "Item desc";
	public Sprite sprite;

	[Header("On ground data"), Space]
	public int singleMass = 500;
	public float scaleFactorOnGround = 1.0f;

	[Header("Mining item data"), Space]
	[NaughtyAttributes.ShowIf("IsMiningItem")] public int miningForce = 1;

	bool IsMiningItem() {
		return metaType == ItemMetaType.MiningTool;
	}
}