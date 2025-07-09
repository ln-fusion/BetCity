using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [Header("Data Reference")]
    public Card card;

    [Header("Visual Components")]
    public Image cardBackgroundSprite; // 卡牌背景
    public Image cardArtworkImage; // 卡牌艺术图
    public TextMeshProUGUI cardNameText; // 卡牌名称
    public TextMeshProUGUI scoreText; // 分数
    public TextMeshProUGUI descriptionText; // 描述

    [Header("Ownership Backgrounds")]
    public Sprite playerABackground; // 玩家A的背景图
    public Sprite playerBBackground; // 玩家B的背景图

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
        if (card == null)
            return;

        // 更新卡牌背景
        if (cardBackgroundSprite != null)
        {
            cardBackgroundSprite.sprite = card.owner == CardOwner.PlayerA
                ? playerABackground
                : playerBBackground;
        }

        // 更新卡牌名称
        if (cardNameText != null)
        {
            cardNameText.text = card.cardName;
        }

        // 更新分数
        if (scoreText != null && card is MonsterCard monster)
        {
            scoreText.text = monster.score.ToString();
        }
        else if (scoreText != null)
        {
            scoreText.text = "";
        }

        // 更新卡牌描述（）
        if (descriptionText != null)
        {
            descriptionText.text = card.description;
        }

        // 更新卡牌艺术图
        if (cardArtworkImage != null)
        {
            cardArtworkImage.sprite = card.cardArtwork; // Image通过sprite属性赋值
            cardArtworkImage.enabled = card.cardArtwork != null; // 图片为空时隐藏
        }

        // 仅怪兽卡根据激活状态旋转
        if (card is MonsterCard monsterCard)
        {
            transform.rotation = monsterCard.isActive
                ? Quaternion.Euler(0, 0, 180f)
                : Quaternion.Euler(0, 0, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0f);
        }
    }

    // 事件相关方法 
    private void RegisterCardEvents()
    {
        if (card != null)
        {
            card.OnOwnerChanged += OnOwnerChanged;
        }
    }

    private void UnregisterCardEvents()
    {
        if (card != null)
        {
            card.OnOwnerChanged -= OnOwnerChanged;
        }
    }

    private void OnOwnerChanged(Card card, CardOwner oldOwner, CardOwner newOwner)
    {
        if (card == this.card)
        {
            UpdateCardDisplay();
            Debug.Log($"卡牌 '{card.cardName}' 所有权从 {oldOwner} 变更为 {newOwner}");
        }
    }
}