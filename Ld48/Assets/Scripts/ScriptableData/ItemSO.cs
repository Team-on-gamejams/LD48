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

		Pickaxe = 1,

		Dirt = 500,
		Stone,
		Bedrock,

		OreIron = 1000,
		OreGold,

		IgnotIron = 1500,
		IgnotGold,

		BlockBrick = 2000,
		BlockPlatform,
		BlockLadder,
	}

	[Header("Item data"), Space]
	public ItemMetaType metaType;
	public ItemType type;
	public int maxCount = 10;

	[Header("Visual"), Space]
	public string name = "Item name";
	public string description = "Item desc";
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