using UnityEngine;

public abstract class Card
{
    public int id;
    public string cardName;
    public string description;
    public int cardArtworkid;
    public CardSeries series;
    public bool isActive = false;
    public Sprite cardArtwork;

    public Card(int id, string cardName, string description, int cardArtworkid, CardSeries series)
    {
        this.id = id;
        this.cardName = cardName;
        this.description = description;
        this.cardArtworkid = cardArtworkid;
        this.series = series;
        LoadCardArtwork();
    }

    // 加载卡图的方法
    public void LoadCardArtwork()
    {
        // 从Resources目录加载图片
        string path = $"/Image/CardImage";
        cardArtwork = Resources.Load<Sprite>(path);
    }
}

// 怪兽卡类
public class MonsterCard : Card
{
    public int score;
    private CardOwner _owner;

    public CardOwner owner
    {
        get => _owner;
        set
        {
            if (_owner != value)
            {
                CardOwner oldOwner = _owner;
                _owner = value;
                OnOwnerChanged?.Invoke(this, oldOwner, value);
                GameEvent.TriggerCardOwnershipChanged(this, oldOwner, value);
            }
        }
    }

    public System.Action<MonsterCard, CardOwner, CardOwner> OnOwnerChanged;

    public MonsterCard(int id, string cardName, string description,
        int cardArtworkid, int score, CardOwner owner, CardSeries series)
        : base(id, cardName, description, cardArtworkid, series)
    {
        this.score = score;
        this.owner = owner;
    }
}

// 魔法卡类
public class SpellCard : Card
{
    public SpellCard(int id, string cardName, string description,
        int cardArtworkid, CardSeries series)
        : base(id, cardName, description, cardArtworkid, series)
    {
    }
}

// 卡牌所有者枚举
public enum CardOwner
{
    PlayerA,
    PlayerB
}

// 卡牌系列枚举
public enum CardSeries
{
    None,
    Memory, // 记忆系列
    Burn,   // 灼烧系列
    Root,
}