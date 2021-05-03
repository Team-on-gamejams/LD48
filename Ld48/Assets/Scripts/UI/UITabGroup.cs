using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class UITabGroup : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] protected CanvasGroup cg;

	bool isShowed;

	protected virtual void Start() {
		if (cg) {
			isShowed = false;
			cg.blocksRaycasts = cg.interactable = false;
			cg.alpha = 0.0f;
		}
		else {
			isShowed = true;
		}
	}

	public void ToggleVisible() {
		if (!cg)
			return;

		LeanTween.cancel(gameObject, false);

		if (isShowed) {
			isShowed = false;
			cg.blocksRaycasts = cg.interactable = false;
			LeanTweenEx.ChangeAlpha(cg, 0.0f, 0.1f).setEase(LeanTweenType.easeInOutQuad);
		}
		else {
			isShowed = true;
			cg.blocksRaycasts = cg.interactable = true;
			LeanTweenEx.ChangeAlpha(cg, 1.0f, 0.1f).setEase(LeanTweenType.easeInOutQuad);
		}
	}

}
