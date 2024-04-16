using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item/ItemData")]
public class ItemScriptableObject : ScriptableObject
{
    public int id;
    public int nextLevel;
    public int row;
    public int column;
    public string itemName;
    public string spriteName;
    public int minDamage;
    public int maxDamage;
    public int block;
    public int regenHp;
    public float regenStamina;
    public float staminaCost;
    public float cooldown;
    public ItemType itemType;
}
