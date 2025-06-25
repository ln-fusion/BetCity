using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData; // 引用ScriptableObject的CardData
    public SpriteRenderer artworkSprite; // 引用卡牌图案的SpriteRenderer

    void Start()
    {
        if (cardData != null)
        {
            artworkSprite.sprite = cardData.cardArtwork;
            // 根据cardData的其他属性更新UI显示，例如卡牌名称、描述等
        }
    }

    // 处理卡牌点击、拖拽等交互逻辑
}