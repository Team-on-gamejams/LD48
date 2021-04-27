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
	[SerializeField] UITabGroup uiTabGroup;

	bool isHoldShift = false;
	ItemOnGround buttonDownItem;

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
			case InputActionPhase.Started:
				mover.JumpStart();

				break;
			case InputActionPhase.Canceled:
				mover.JumpEnd();
				break;
		}
	}

	public void OnDash(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				mover.DashStart();

				break;
			case InputActionPhase.Canceled:
				mover.DashEnd();
				break;
		}
	}

	public void UseItemL(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				if (TryPickupItemOnGroundOnMouseDown())
					return;

				if(IsCanUseItemInToolbar())
					itemUser.StartUseLeftItem();
				break;
			case InputActionPhase.Canceled:
				TryPickupItemOnGroundOnMouseUp();
				itemUser.StoptUseLeftItem();
				break;
		}
	}

	public void UseItemR(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				if (IsCanUseItemInToolbar())
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
				uiTabGroup.ToggleVisible();
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

	public void OnSelectHotbar6(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(6, !isHoldShift);
				break;
		}
	}

	public void OnSelectHotbar7(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(7, !isHoldShift);
				break;
		}
	}

	public void OnSelectHotbar8(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(8, !isHoldShift);
				break;
		}
	}

	public void OnSelectHotbar9(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Performed:
				hotbar.SetSelection(9, !isHoldShift);
				break;
		}
	}

	bool TryPickupItemOnGroundOnMouseDown() {
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero, 0, UnityConstants.Layers.ItemsOnGroundMask); 
		if(
			hit.collider != null && 
			((Vector3)hit.point - GameManager.Instance.player.mover.transform.position).magnitude <= GameManager.Instance.player.maxInteractDistance
		) {
			buttonDownItem = hit.collider.GetComponent<ItemOnGround>();
			return true;
		}
		return false;
	}

	void TryPickupItemOnGroundOnMouseUp() {
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero, 0, UnityConstants.Layers.ItemsOnGroundMask);
		if (
			hit.collider != null &&
			((Vector3)hit.point - GameManager.Instance.player.mover.transform.position).magnitude <= GameManager.Instance.player.maxInteractDistance &&
			hit.collider.GetComponent<ItemOnGround>() == buttonDownItem
		) {
			ItemData leftItem = GameManager.Instance.player.inventory.AddItem(buttonDownItem.item);

			if (leftItem.count == 0) {
				Destroy(buttonDownItem.gameObject);
			}
			else {
				//TODO: Inventory full popup text
				buttonDownItem.item = leftItem;
				buttonDownItem.Init();
			}

			buttonDownItem = null;
		}
	}

	bool IsCanUseItemInToolbar() {
		bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
		if (isOverUI)
			return false;

		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero, 0, UnityConstants.Layers.ItemsOnGroundMask);
		bool isOverItemOnGround = hit.collider != null;
		if (isOverItemOnGround)
			return false;

		return true;
	}
}
