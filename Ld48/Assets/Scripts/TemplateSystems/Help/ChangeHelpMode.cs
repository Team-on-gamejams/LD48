using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHelpMode : MonoBehaviour {
	public void CycleHelpLevel() {
		if(GameManager.Instance.HelpLevelMode == GameManager.Instance.minHelpLevel) {
			GameManager.Instance.HelpLevelMode = GameManager.Instance.maxHelpLevel;
		}
		else {
			--GameManager.Instance.HelpLevelMode;
		}
	}
}
