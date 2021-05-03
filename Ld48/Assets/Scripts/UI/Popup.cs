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
	public bool IsShowed => isShowed;

	[Header("Refs"), Space]
	[SerializeField] CanvasGroup cg;
	[SerializeField] TextMeshProUGUI textField;

	bool isShowed;

	private void Awake() {
		cg.alpha = 0.0f;
	}

	public void SetText(string text) {
		textField.text = text;
	}

	public void Show() {
		isShowed = true;

		LeanTween.cancel(gameObject, false);
		LeanTweenEx.ChangeAlpha(cg, 1.0f, 0.1f).setEase(LeanTweenType.easeInOutQuad);
	}

	public void Hide() {
		isShowed = false;

		LeanTween.cancel(gameObject, false);
		LeanTweenEx.ChangeAlpha(cg, 0.0f, 0.1f).setEase(LeanTweenType.easeInOutQuad);
	}
}
