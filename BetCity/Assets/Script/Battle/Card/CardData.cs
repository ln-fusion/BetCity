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
    PlayerA, // ��ɫ
    PlayerB  // ��ɫ
}

public enum CardSeries
{
    None,
    Memory, // ����ϵ��
    Burn,   // ����ϵ��
}
