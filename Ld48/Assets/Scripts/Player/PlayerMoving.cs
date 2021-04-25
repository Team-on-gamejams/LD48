using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoving : MonoBehaviour {
	[Header("Data"), Space]
	[SerializeField] float speed = 8.0f;
	[SerializeField] float jumpForce = 6.5f;

	[Header("Refs"), Space]
	[SerializeField] Rigidbody2D rb;
	[SerializeField] DebugText debugText;
	[SerializeField] CapsuleCollider2D collider2D;

	Vector2 moveVector;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rb)
			rb = GetComponent<Rigidbody2D>();
		if (!debugText)
			debugText = GetComponentInChildren<DebugText>();
	}
#endif

	void FixedUpdate() {
		rb.velocity = rb.velocity.SetX(moveVector.x * speed);

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

	public void Jump() {
		if (IsGrounded())
			rb.AddForce(transform.up * jumpForce, ForceMode2DEx.Acceleration);
	}

	public bool IsGrounded() {
		RaycastHit2D raycastHit2D = Physics2D.BoxCast(collider2D.bounds.center, collider2D.bounds.size, 0f, Vector2.down * .1f);
		return raycastHit2D.collider != null;
	}
}
