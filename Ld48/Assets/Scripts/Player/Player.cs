using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] PlayerMoving mover;
	[SerializeField] DebugText debugText;
	public int Wallet = 1000;
	public Inventory inventory;
	public List<DungeonObject> PlayerInventory = new List<DungeonObject>();

#if UNITY_EDITOR
	private void OnValidate() {
		if (!mover)
			mover = GetComponentInChildren<PlayerMoving>();
		if (!debugText)
			debugText = GetComponentInChildren<DebugText>();
	}
#endif

	private void Awake() {
		GameManager.Instance.player = this;
	}

	public void OnMove(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				mover.OnMoveStart();
				break;
			case InputActionPhase.Performed:
				mover.OnMove(context.ReadValue<Vector2>());
				break;
			case InputActionPhase.Canceled:
				mover.OnMoveStop();
				break;
		}
	}
}
