using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItemSO : ScriptableObject
{
    public TileBase tile;
    public string itemName;
    public Sprite icon;
    public string description;
    public ItemType itemType;
    public ActionType actionType;
    public int Amount;
    public int Value;
    public AudioSource audioSource;
    public GameObject objectPrefab;
}

public enum ItemType
{
    HP,
    MP,
    Speed,
    Gun,
    Ammo
} 

public enum ActionType
{
    Shoot,
    Buff,
    Improve
}