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
	[Header("Item data"), Space]
	public ItemData item;

	[Header("Place data"), Space]
	public PlacebleBlock placebleBlock;

	[Header("Refs"), Space]
	[SerializeField] SpriteRenderer sr;
	[SerializeField] BoxCollider2D boxCollider;
	[SerializeField] Rigidbody2D rb;

	public void Init() {
		gameObject.transform.localScale = Vector2.one * item.itemSO.scaleFactorOnGround * Mathf.Lerp(0.2f, 0.8f, (float)(item.count) / item.itemSO.maxCount);

		rb.mass = item.count * item.itemSO.singleMass;
		rb.angularDrag = rb.drag = Mathf.Lerp(1.0f, 2.0f, (float)(item.count) / item.itemSO.maxCount);
	}

	static public ItemOnGround CreateOnGround(ItemData item, Vector3 pos) {
		pos.z = 0;

		ItemOnGround groundItem = Instantiate(GameManager.Instance.GetItemOnGround(item.itemSO.type), pos, Quaternion.identity, null).GetComponent<ItemOnGround>();
		groundItem.item.count = item.count;
		groundItem.Init();

		return groundItem;
	}
}
