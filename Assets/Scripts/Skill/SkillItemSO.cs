using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Skill/Item")]
public class SkillItemSO : ScriptableObject
{
    public int id;
    public string upgradeName;
    public string description;
    public Rarity rarity;
    public System.Action applyEffect;
    public Sprite icon;
}
public enum Rarity
{
    Common,
    Rare,
    Epic
}
