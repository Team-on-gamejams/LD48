using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class ItemOnGround : MonoBehaviour {
	[NonSerialized] public ItemData item;

	[Header("Refs"), Space]

	[SerializeField] SpriteRenderer sr;
	[SerializeField] BoxCollider2D boxCollider;
	[SerializeField] Rigidbody2D rb;

	private void OnMouseUpAsButton() {
		ItemData leftItem = GameManager.Instance.player.inventory.AddItem(item);

		if (leftItem.count == 0) {
			Destroy(gameObject);
		}
		else {
			//TODO: Inventory full popup text
			item = leftItem;
			Init();
		}
	}

	void Init() {
		gameObject.transform.localScale = Vector2.Lerp(Vector2.one * 0.1f, Vector2.one, (float)(item.count) / item.itemSO.maxCount);

		sr = gameObject.GetComponent<SpriteRenderer>();
		if (!sr)
			sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = item.itemSO.sprite;

		boxCollider = gameObject.GetComponent<BoxCollider2D>();
		if (!boxCollider)
			boxCollider = gameObject.AddComponent<BoxCollider2D>();
		boxCollider.size = Vector2.one;

		rb = gameObject.GetComponent<Rigidbody2D>();
		if (!rb)
			rb = gameObject.AddComponent<Rigidbody2D>();
		rb.mass = item.count * item.itemSO.singleMass;
		rb.angularDrag = rb.drag = Mathf.Lerp(1.0f, 2.0f, (float)(item.count) / item.itemSO.maxCount);
	}

	static public ItemOnGround CreateOnGround(ItemData item, Vector3 pos) {
		pos.z = 0;

		GameObject go = new GameObject($"OnGroundItem-{item.itemSO.name}");
		go.transform.position = pos;

		ItemOnGround newItem = go.AddComponent<ItemOnGround>();
		newItem.item = item;
		newItem.Init();

		return newItem;
	}
}
