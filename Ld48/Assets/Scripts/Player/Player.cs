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
	[Header("Inventory Data"), Space]
	public float maxInteractDistance = 5.0f;

	[Header("Refs"), Space]
	public PlayerMoving mover;
	public Inventory inventory;
	[SerializeField] Hotbar hotbar;
	[SerializeField] DebugText debugText;
	[SerializeField] PlayerItemUser itemUser;
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
		switch (context.phase) {
			case InputActionPhase.Performed:
				mover.Jump();
				break;
		}
	}

	public void UseItemL(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				itemUser.StartUseLeftItem();
				break;
			case InputActionPhase.Canceled:
				itemUser.StoptUseLeftItem();
				break;
		}
	}

	public void UseItemR(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				itemUser.StartUseRightItem();
				break;
			case InputActionPhase.Canceled:
				itemUser.StoptUseRightItem();
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
