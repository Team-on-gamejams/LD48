using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Popup : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] CanvasGroup cg;
	[SerializeField] TextMeshProUGUI textField;

	Transform defaultParent;

	private void Awake() {
		cg.alpha = 0.0f;
		defaultParent = transform.parent;
	}

	public void SetText(string text) {
		textField.text = text;
	}

	public void Show() {
		LeanTween.cancel(gameObject, false);
		LeanTweenEx.ChangeAlpha(cg, 1.0f, 0.1f);

		transform.SetParent(textField.canvas.transform);
	}

	public void Hide() {
		LeanTween.cancel(gameObject, false);
		LeanTweenEx.ChangeAlpha(cg, 0.0f, 0.1f);

		transform.SetParent(defaultParent);
	}
}
