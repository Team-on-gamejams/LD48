using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class HotbarItem : MonoBehaviour, IPointerClickHandler {
	[NonSerialized] public byte id;

	[Header("Refs self"), Space]
	[SerializeField] Image itemImage;
	[SerializeField] Image selectedFrameLeftImage;
	[SerializeField] Image selectedFrameRightImage;
	[SerializeField] TextMeshProUGUI count;

	[Header("Refs"), Space]
	[SerializeField] Hotbar hotbar;

	protected void Awake() {

	}

	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Left) {
			hotbar.SetSelection(id, true);
		}
		else if (eventData.button == PointerEventData.InputButton.Right) {
			hotbar.SetSelection(id, false);
		}
		else if (eventData.button == PointerEventData.InputButton.Middle) {
			hotbar.SetSelection(id, true);
			hotbar.SetSelection(id, false);
		}
	}

	public void SetSelectedFrame(bool isLeftHand) {
		LeanTween.cancel(gameObject, false);

		if (isLeftHand) {
			LeanTweenEx.ChangeAlpha(selectedFrameLeftImage, 1.0f, 0.1f);
		}
		else {
			LeanTweenEx.ChangeAlpha(selectedFrameRightImage, 1.0f, 0.1f);
		}
	}

	public void RemoveSelectedFrame(bool isLeftHand) {
		LeanTween.cancel(gameObject, false);

		if (isLeftHand) {
			LeanTweenEx.ChangeAlpha(selectedFrameLeftImage, 0.0f, 0.1f);
		}
		else {
			LeanTweenEx.ChangeAlpha(selectedFrameRightImage, 0.0f, 0.1f);
		}
	}
}
