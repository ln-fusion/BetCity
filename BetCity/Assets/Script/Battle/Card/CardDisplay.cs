using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData; // ����ScriptableObject��CardData
    public SpriteRenderer artworkSprite; // ���ÿ���ͼ����SpriteRenderer

    void Start()
    {
        if (cardData != null)
        {
            artworkSprite.sprite = cardData.cardArtwork;
            // ����cardData���������Ը���UI��ʾ�����翨�����ơ�������
        }
    }

    // �����Ƶ������ק�Ƚ����߼�
}