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
	[SerializeField] Inventory inventory;
	[SerializeField] DebugText debugText;
	[Space]
	public int Wallet = 1000;
	public InstrumentsDB inventoryDB;
	public List<DungeonObject> PlayerInventory = new List<DungeonObject>();
	bool isHoldShift = false;

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

	public void OnJump(InputAction.CallbackContext context) {
		Debug.Log("jumpBefore");

		switch (context.phase) {
			case InputActionPhase.Performed:
				Debug.Log("jump");
				mover.Jump();
				break;
		}
	}

	public void UseItemL(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				Debug.Log("Use item L - Started");
				break;
			case InputActionPhase.Performed:
				Debug.Log("Use item L - Performed");
				break;
			case InputActionPhase.Canceled:
				Debug.Log("Use item L - Canceled");
				break;
		}
	}

	public void UseItemR(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				Debug.Log("Use item R - Started");
				break;
			case InputActionPhase.Performed:
				Debug.Log("Use item R - Performed");
				break;
			case InputActionPhase.Canceled:
				Debug.Log("Use item R - Canceled");
				break;
		}
	}

	public void OnToggleInventory(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				inventory.ToggleVisible();
				break;
		}
	}

	public void OnHoldShift(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				isHoldShift = true;
				break;
			case InputActionPhase.Canceled:
				isHoldShift = false;
				break;
		}
	}

	public void OnHotbarScroll(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				Vector2 value = context.ReadValue<Vector2>();
				if (value.y > 0) {
					hotbar.MoveSelectionUp(!isHoldShift);
				}
				else if (value.y < 0) {
					hotbar.MoveSelectionDown(!isHoldShift);
				}
				break;
		}
	}

	public void OnSelectHotbar0(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(0, !isHoldShift);
				break;
		}
	}

	public void OnSelectHotbar1(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(1, !isHoldShift);
				break;
		}
	}

	public void OnSelectHotbar2(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(2, !isHoldShift);
				break;
		}
	}

	public void OnSelectHotbar3(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(3, !isHoldShift);
				break;
		}
	}

	public void OnSelectHotbar4(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(4, !isHoldShift);
				break;
		}
	}

	public void OnSelectHotbar5(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(5, !isHoldShift);
				break;
		}
	}
}
