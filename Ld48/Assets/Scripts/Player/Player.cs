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
	public PlayerMoving mover;
	[SerializeField] Hotbar hotbar;
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

	public void OnHotbarScroll(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				Debug.Log("Scroll Started");
				break;
			case InputActionPhase.Performed:
				Debug.Log("Scroll Performed");
				break;
			case InputActionPhase.Canceled:
				Debug.Log("Scroll Canceled");
				break;
		}
	}

	public void OnSelectHotbar0(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(0, true);
				break;
		}
	}

	public void OnSelectHotbar1(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(1, true);
				break;
		}
	}

	public void OnSelectHotbar2(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(2, true);
				break;
		}
	}

	public void OnSelectHotbar3(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(3, true);
				break;
		}
	}

	public void OnSelectHotbar4(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(4, true);
				break;
		}
	}

	public void OnSelectHotbar5(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(5, true);
				break;
		}
	}
}
