using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class BlockDestoyVisual : MonoBehaviour {
	[Header("Visual"), Space]
	[SerializeField] Sprite[] brokeSprites;

	[Header("Refs"), Space]
	[SerializeField] SpriteRenderer sr;

	private void Awake() {
		UpdateVisual(0.0f);
	}

	public void UpdateVisual(float persent) {
		if(persent == 0) {
			sr.color = sr.color.SetA(0.0f);
		}
		else {
			sr.color = sr.color.SetA(persent * 3);
		
			for (int i = brokeSprites.Length; i >= 0; --i) {
				if (((float)(i) / brokeSprites.Length) <= persent) {
					sr.sprite = brokeSprites[i];
					break;
				}
			}
		}
	}
}
