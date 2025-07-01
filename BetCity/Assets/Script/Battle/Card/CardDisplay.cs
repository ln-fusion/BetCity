using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [Header("Data Reference")]
    public Card cardData;

    [Header("Visual Components")]
    public SpriteRenderer cardBackgroundSprite; // 卡牌背景
    public SpriteRenderer cardArtworkSprite;    // 卡牌艺术图
    public TextMeshProUGUI cardNameText;        // 卡牌名称
    public TextMeshProUGUI scoreText;           // 分数
    public TextMeshProUGUI descriptionText;     // 描述

    [Header("Ownership Backgrounds")]
    public Sprite playerABackground; // 玩家A的卡牌背景
    public Sprite playerBBackground; // 玩家B的卡牌背景

    void Start()
    {
        UpdateCardDisplay();
        RegisterCardEvents();
    }

    void OnValidate()
    {
        UpdateCardDisplay();
    }

    void OnEnable()
    {
        RegisterCardEvents();
    }

    void OnDisable()
    {
        UnregisterCardEvents();
    }

    public void UpdateCardDisplay()
    {
        if (cardData == null)
            return;

        // 更新卡牌背景（所有卡牌都有归属权）
        if (cardBackgroundSprite != null)
        {
            cardBackgroundSprite.sprite = cardData.owner == CardOwner.PlayerA
                ? playerABackground
                : playerBBackground;
        }

        // 更新卡牌名称
        if (cardNameText != null)
        {
            cardNameText.text = cardData.cardName;
        }

        // 更新分数（仅怪兽卡有分数）
        if (scoreText != null && cardData is MonsterCard monster)
        {
            scoreText.text = monster.score.ToString();
        }
        else if (scoreText != null)
        {
            scoreText.text = ""; // 非怪兽卡隐藏分数
        }

        // 更新卡牌描述
        if (descriptionText != null)
        {
            descriptionText.text = cardData.description;
        }

        // 更新卡牌艺术图
        if (cardArtworkSprite != null)
        {
            cardArtworkSprite.sprite = cardData.cardArtwork;
        }

        // 仅怪兽卡根据激活状态旋转
        if (cardData is MonsterCard monsterCard)
        {
            transform.rotation = monsterCard.isActive
                ? Quaternion.Euler(0, 0, 180f)
                : Quaternion.Euler(0, 0, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0f); // 非怪兽卡保持正常方向
        }
    }

    // 注册卡牌事件监听
    private void RegisterCardEvents()
    {
        if (cardData != null)
        {
            cardData.OnOwnerChanged += OnOwnerChanged;
        }
    }

    // 取消注册卡牌事件监听
    private void UnregisterCardEvents()
    {
        if (cardData != null)
        {
            cardData.OnOwnerChanged -= OnOwnerChanged;
        }
    }

    // 处理所有权变更事件
    private void OnOwnerChanged(Card card, CardOwner oldOwner, CardOwner newOwner)
    {
        if (card == cardData)
        {
            UpdateCardDisplay();
            Debug.Log($"卡牌 '{card.cardName}' 所有权从 {oldOwner} 变更为 {newOwner}");
        }
    }
}