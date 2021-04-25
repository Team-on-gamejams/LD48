using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

[Serializable]
public struct ItemData {
	public ItemSO itemSO;
	public int count;

	public bool IsMaxStack() {
		return count == itemSO.maxCount;
	}
}
