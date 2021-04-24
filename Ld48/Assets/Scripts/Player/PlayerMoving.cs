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

	[Header("Refs"), Space]
	[SerializeField] Rigidbody2D rb;
	[SerializeField] DebugText debugText;

	Vector2 moveVector;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rb)
			rb = GetComponent<Rigidbody2D>();
		if (!debugText)
			debugText = GetComponentInChildren<DebugText>();
	}
#endif

	void Start() {
		enabled = false;
	}

	void FixedUpdate() {
		rb.velocity = moveVector * speed;

		debugText.SetText($"Speed: {rb.velocity.magnitude.ToString("0.0")} m/s");
	}

	public void OnMoveStart() {
		enabled = true;
	}

	public void OnMove(Vector2 vector) {
		moveVector = vector;
	}

	public void OnMoveStop() {
		enabled = false;

		rb.velocity = moveVector = Vector2.zero;

		debugText.SetText($"Speed: {rb.velocity.magnitude.ToString("0.0")} m/s");
	}
}
