using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour {
    [Header("Data"), Space]
    public Vector2Int gridSize = new Vector2Int(128, 128);
    public Vector2 cellSize = Vector2.one;
}
