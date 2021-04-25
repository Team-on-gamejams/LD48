using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using yaSingleton;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "GameManager", menuName = "Singletons/GameManager")]
public class GameManager : Singleton<GameManager> {
	public bool IsDebugMode {
		get => isDebugMode;
		set {
			if (isDebugMode != value) {
				isDebugMode = value;
				OnDebugModeChange?.Invoke(isDebugMode);
			}
		}
	}
	public Action<bool> OnDebugModeChange;
	bool isDebugMode = true;

	[NonSerialized]  public Player player;
	[NonSerialized]  public GameObject draggedParent;

	[Header("Sprites"), Space]
	public Sprite foregroundDirtSprite;
	public Sprite backgroundDirtSprite;

	public Sprite foregroundStoneSprite;
	public Sprite backgroundStoneSprite;
	
	public Sprite foregroundBedrockSprite;
	public Sprite backgroundBedrockSprite;

	public Sprite oreGoldSprite;
	public Sprite oreIronSprite;

	protected override void Initialize() {
		base.Initialize();
	}

	protected override void Deinitialize() {
		base.Deinitialize();
	}
}
