using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightOnSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("高光设置")]
    public Color highlightColor = new Color(1f, 1f, 0.8f, 1f); // 轻微泛黄的高光色
    public Image cardImage; // 卡牌的Image组件
    private Color originalColor; // 保存原始颜色
    private bool isSelected = false; // 是否被选中

    void Start()
    {
        originalColor = cardImage.color;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (cardImage != null)
        {
            cardImage.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (cardImage != null)
        {
            cardImage.color = originalColor;
        }
    }
}