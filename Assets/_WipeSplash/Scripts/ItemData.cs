using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    ATTACK,
    DEFENSE,
    SUPPORT
}

[Serializable]
public struct ItemData
{
    public int id;
    public int nextLevel;
    public int row;
    public int column;
    public string name;
    public string spriteName;
    public int minDamage;
    public int maxDamage;
    public int block;
    public int regenHp;
    public int regenStamina;
    public int staminaCost;
    public int cooldown;
    public ItemType itemType;
}
