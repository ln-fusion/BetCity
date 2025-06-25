using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Card Game/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite cardArtwork;
    public CardOwner owner;
    public CardSeries series;
    public bool isActive;
    public int score;
}

public enum CardOwner
{
    PlayerA, // 红色
    PlayerB  // 蓝色
}

public enum CardSeries
{
    None,
    Memory, // 记忆系列
    Burn,   // 灼烧系列
}
