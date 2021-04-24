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
public class PlayerMoving : MonoBehaviour
{
    [Header("Data"), Space]
    [SerializeField] float speed = 8.0f;

    [Header("Refs"), Space]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] DebugText debugText;

    public Vector2 moveVector;
    public float thrust = 100f;
    public CapsuleCollider2D collider2D;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rb)
			rb = GetComponent<Rigidbody2D>();
		if (!debugText)
			debugText = GetComponentInChildren<DebugText>();
	}
#endif

    void Start()
    {
        enabled = false;
    }

    void FixedUpdate()
    {
        Vector2 v2 = rb.velocity;
        v2.x = moveVector.x * speed;
        rb.velocity = v2;

        debugText.SetText($"Speed: {rb.velocity.magnitude.ToString("0.0")} m/s");
    }

    public void OnMoveStart()
    {
        enabled = true;
    }

    public void OnMove(Vector2 vector)
    {
        moveVector = vector;
    }

    public void OnMoveStop()
    {
        enabled = false;

        rb.velocity = moveVector = Vector2.zero;

        debugText.SetText($"Speed: {rb.velocity.magnitude.ToString("0.0")} m/s");
    }

    public void Jump()
    {
        Debug.Log("Fuck");
        if (IsGrounded())
            rb.AddForce(transform.up * thrust, ForceMode2DEx.VelocityChange);
    }

    public bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(collider2D.bounds.center, collider2D.bounds.size, 0f, Vector2.down * .1f);
        Debug.Log(raycastHit2D.collider);
        return raycastHit2D.collider != null;
    }
}
