using UnityEngine;

public class Item
{
    public string itemName;  // 物品名称
    public int itemID;       // 物品ID（可以用于区分不同的物品）
    public string description; // 物品描述

    // 构造函数
    public Item(string itemName, int itemID, string description = "")
    {
        this.itemName = itemName;
        this.itemID = itemID;
        this.description = description;
    }

    // 你可以根据需要扩展更多的物品属性，比如价格、耐久度、图标等
}
