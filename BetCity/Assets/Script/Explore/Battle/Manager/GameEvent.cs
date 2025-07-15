using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameEvent
{
    // 全局卡牌所有权变更事件
    public static Action<Card, CardOwner, CardOwner> OnCardOwnershipChanged;

    // 触发全局事件的方法
    public static void TriggerCardOwnershipChanged(Card card, CardOwner oldOwner, CardOwner newOwner)
    {
        OnCardOwnershipChanged?.Invoke(card, oldOwner, newOwner);
    }
}