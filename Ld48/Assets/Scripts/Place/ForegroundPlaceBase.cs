using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundPlaceBase : MonoBehaviour {
	static int canvasSortOrder = -1000;

	[Header("UI"), Space]
	[SerializeField] Canvas canvas;
	[SerializeField] CanvasGroup cg;

	bool isShowed;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!canvas)
			canvas = GetComponent<Canvas>();
		if (!cg)
			cg = GetComponent<CanvasGroup>();
	}
#endif

	private void Awake() {
		isShowed = false;
		cg.interactable = cg.blocksRaycasts = false;
		cg.alpha = 0.0f;
	}

	virtual protected void Update() {
		if (isShowed) {
			if ((transform.position - GameManager.Instance.player.mover.transform.position).sqrMagnitude > GameManager.Instance.player.maxInteractDistanceSqr) {
				HideUI();
			}
		}
	}

	public void ToggleUI() {
		if (isShowed) {
			HideUI();
		}
		else {
			ShowUI();
		}
	}

	public virtual void ShowUI() {
		isShowed = true;
		
		canvas.sortingOrder = canvasSortOrder++;
		cg.interactable = cg.blocksRaycasts = true;

		LeanTween.cancel(cg.gameObject, false);
		LeanTweenEx.ChangeAlpha(cg, 1.0f, 0.2f).setEase(LeanTweenType.easeInOutQuad);
	}

	public virtual void HideUI() {
		isShowed = false;
		
		--canvasSortOrder;
		cg.interactable = cg.blocksRaycasts = false;

		LeanTween.cancel(cg.gameObject, false);
		LeanTweenEx.ChangeAlpha(cg, 0.0f, 0.2f).setEase(LeanTweenType.easeInOutQuad);
	}
}
