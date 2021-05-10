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
	[SerializeField] protected CanvasGroup cgAlt;

	bool isShowed;

	protected virtual void Start() {
		if (cg) {
			isShowed = false;
			cg.blocksRaycasts = cg.interactable = false;
			cg.alpha = 0.0f;

			if (cgAlt) {
				cgAlt.blocksRaycasts = cgAlt.interactable = true;
				cgAlt.alpha = 1.0f;
			}
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

			if (cgAlt) {
				cgAlt.blocksRaycasts = cgAlt.interactable = true;
				LeanTweenEx.ChangeAlpha(cgAlt, 1.0f, 0.1f).setEase(LeanTweenType.easeInOutQuad);
			}
		}
		else {
			isShowed = true;
			cg.blocksRaycasts = cg.interactable = true;
			LeanTweenEx.ChangeAlpha(cg, 1.0f, 0.1f).setEase(LeanTweenType.easeInOutQuad);

			if (cgAlt) {
				cgAlt.blocksRaycasts = cgAlt.interactable = false;
				LeanTweenEx.ChangeAlpha(cgAlt, 0.0f, 0.1f).setEase(LeanTweenType.easeInOutQuad);
			}
		}
	}

}
