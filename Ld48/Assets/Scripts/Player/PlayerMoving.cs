using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;
using DYP;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoving : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] Rigidbody2D rb;
	[SerializeField] DebugText debugText;
	[SerializeField] BasicMovementController2D controller;

	Vector2 moveVector;

	bool isPressJump;
	bool isReleaseJump;
	bool isHoldJump;

	bool isHoldDash;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rb)
			rb = GetComponent<Rigidbody2D>();
		if (!debugText)
			debugText = GetComponentInChildren<DebugText>();
	}
#endif

	private void Update() {
		controller.InputMovement(moveVector);

		controller.PressJump(isPressJump);
		controller.HoldJump(isHoldJump);
		controller.ReleaseJump(isReleaseJump);
		isPressJump = isReleaseJump = false;

		controller.HoldDash(isHoldDash);
	}

	void FixedUpdate() {
		debugText.SetText($"Speed: {rb.velocity.magnitude.ToString("0.0")} m/s");
	}

	public void OnMoveStart() {

	}

	public void OnMove(Vector2 vector) {
		moveVector = vector;
	}

	public void OnMoveStop() {
		moveVector = Vector2.zero;
	}

	public void JumpStart() {
		isPressJump = true;
		isHoldJump = true;
	}

	public void JumpEnd() {
		isReleaseJump = true;
		isHoldJump = false;
	}

	public void DashStart() {
		controller.PressDash(true);
		isHoldDash = true;
	}

	public void DashEnd() {
		isHoldDash = false;
	}
}
