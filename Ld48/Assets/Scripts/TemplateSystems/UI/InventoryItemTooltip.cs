using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class InventoryItemTooltip : MonoBehaviour {
	public static int canvasSortOrder = 100;
	public const int charsToMaxWidth = 24;

	public bool IsShowed => isShowed;

	[Header("Refs"), Space]
	[SerializeField] RectTransform rt;
	[SerializeField] Canvas canvas;
	[SerializeField] CanvasGroup cg;
	[SerializeField] TextMeshProUGUI textField;
	[SerializeField] LayoutElement childLayoutElement;

	RectTransform parentRt;

	bool isShowed;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rt)
			rt = GetComponent<RectTransform>();
		if (!canvas)
			canvas = GetComponent<Canvas>();
		if (!cg)
			cg = GetComponent<CanvasGroup>();
	}
#endif

	private void Awake() {
		cg.alpha = 0.0f;
	}

	private void Start() {
		enabled = false;

		parentRt = transform.parent.GetComponent<RectTransform>();
	}

	public void SetText(string text) {
		textField.text = text;

		bool needMaxWidth = false;
		int charsInThisLine = 0;

		for (int i = 0; i < text.Length; ++i) {
			if(text[i] == '\n') {
				charsInThisLine = 0;
			}
			else {
				++charsInThisLine;
				if(charsInThisLine > charsToMaxWidth) {
					needMaxWidth = true;
					break;
				}
			}
		}

		childLayoutElement.enabled = needMaxWidth;
	}

	public void Show() {
		enabled = isShowed = true;

		canvas.sortingOrder = canvasSortOrder++;

		rt.anchorMin = new Vector2(0.0f, 1.0f);
		rt.anchorMax = new Vector2(0.0f, 1.0f);
		rt.pivot = new Vector2(0.0f, 1.0f);
		rt.anchoredPosition = new Vector3(parentRt.rect.width / 2, -parentRt.rect.height);

		if(rt.position.x + rt.rect.xMax >= Screen.width) {
			rt.anchoredPosition -= new Vector2(rt.rect.width, 0);
		}

		if (rt.position.y + rt.rect.yMin <= 0) {
			rt.anchoredPosition += new Vector2(0, rt.rect.height + parentRt.rect.height);
		}

		LeanTween.cancel(gameObject, false);
		LeanTweenEx.ChangeAlpha(cg, 1.0f, 0.1f).setEase(LeanTweenType.easeInOutQuad);
	}

	public void Hide() {
		isShowed = false;

		--canvasSortOrder;

		LeanTween.cancel(gameObject, false);
		LeanTweenEx.ChangeAlpha(cg, 0.0f, 0.1f).setEase(LeanTweenType.easeInOutQuad)
		.setOnComplete(()=> {
			enabled = false;
		});
	}
}
