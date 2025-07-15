using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SanityUIController : MonoBehaviour
{
    [SerializeField] private Slider sanitySlider;  // UI中的滑动条
    [SerializeField] private TextMeshProUGUI sanityText;  // UI中的文字
    [SerializeField] private Button increaseButton;
    [SerializeField] private Button decreaseButton;

    private void Start()
    {
        // 注册到 onSanityChanged 事件
        SanityManager.Instance.onSanityChanged.AddListener(UpdateSanityUI);

        // 如果有 UI 按钮，添加点击事件
        if (increaseButton != null)
        {
            increaseButton.onClick.AddListener(() => SanityManager.Instance.IncreaseSanity(10));
        }

        if (decreaseButton != null)
        {
            decreaseButton.onClick.AddListener(() => SanityManager.Instance.DecreaseSanity(10));
        }

        // 初始化 UI
        UpdateSanityUI();
    }

    private void OnDestroy()
    {
        // 取消订阅事件
        if (SanityManager.Instance != null)
        {
            SanityManager.Instance.onSanityChanged.RemoveListener(UpdateSanityUI);
        }
    }

    // 更新 UI
    private void UpdateSanityUI()
    {
        sanitySlider.value = SanityManager.Instance.CurrentSanity;
        sanityText.text = $"{SanityManager.Instance.CurrentSanity}/{SanityManager.Instance.MaxSanity}";
    }
}
