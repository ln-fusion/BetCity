using UnityEngine;
using System.Collections.Generic;

public class PlayerState
{
    public int sanity;             // 理智值
    public Vector3 position;      // 玩家位置
    public int actionPoints;      // 行动点数
    public List<Item> inventory;  // 可扩展的道具系统

    // 构造函数
    public PlayerState(int sanity, Vector3 position, int actionPoints, List<Item> inventory = null)
    {
        this.sanity = sanity;
        this.position = position;
        this.actionPoints = actionPoints;
        this.inventory = inventory ?? new List<Item>(); // 如果没有传入inventory，则初始化为空列表
    }

    // 更新玩家状态
    public void UpdateState(int newSanity, Vector3 newPosition, int newActionPoints, List<Item> newInventory = null)
    {
        this.sanity = newSanity;
        this.position = newPosition;
        this.actionPoints = newActionPoints;
        this.inventory = newInventory ?? this.inventory;
    }
}
