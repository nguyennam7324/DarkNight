using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;

[System.Serializable]
public class LootItem{
    public GameObject itemPrefab;
    [Range(0,100)]public float dropChange;
}